using System.IO;
using System.Text;
using Searcher.Code.Finders.Scanner;
using Searcher.Code.Settings;

namespace Searcher.Code.Finders.Parsers
{
	sealed class Parser : IParser
	{
		private readonly IAdder adder;
		private readonly IndexSettings settings;
		public char[] Whitespaces = new[] { ' ', '\t', '\r', '\n' };

		private static bool IsSpace(char ch)
		{
			return (ch == ' ') || (ch == '\t') || (ch == '\n') || (ch == '\r');
		}

		private static void GoFirstNoSpace(string file, ref int i)
		{
			while (i < file.Length && IsSpace(file[i])) { i++; }
		}

		private static bool SkipComments(string file, ref int i)
		{
			char ch = file[i];
			if (ch == '/')
			{
				if (i + 1 < file.Length)
				{
					if (file[i + 1] == '/')
					{
						i += 2;
						while (i < file.Length)
						{
							ch = file[i];
							if (ch == '\r' || ch == '\n')
								break;
							i++;
						}
						return true;
					}
					if (file[i + 1] == '*')
					{
						i += 2;
						while (i < file.Length)
						{
							ch = file[i];
							if (ch == '*')
							{
								if (i + 1 < file.Length && file[i + 1] == '/')
								{
									i++;
									break;
								}
							}
							i++;
						}
						return true;
					}
				}
			}
			return false;
		}

		private static bool IsString(char ch)
		{
			return ch == '\'' || ch == '"';
		}

		private void DecodeString(string word, int begin, int fileId)
		{
			var id = 0;
			do
			{
				var prevId = id;
				id = word.IndexOfAny(Whitespaces, id);
				if (id < 0)
					id = word.Length;
				if (prevId == id)
					break;
				adder.AddWord(word.Substring(prevId, id - prevId), fileId);
				GoFirstNoSpace(word, ref id);
			} while (true);
		}

		private bool ExtractString(string file, int fileId, ref int i)
		{
			var strId = file[i];
			if (IsString(file[i]))
			{
				i++;
				var begin = i;
				while (i < file.Length && (strId != file[i])) { i++; }
				if (i != begin)
				{
					DecodeString(file.Substring(begin, i - begin), begin, fileId);
					i++;
					return true;
				}
				i++;
			}
			return false;
		}

		private static bool IsWord(char ch)
		{
			return (ch >= 'a' && ch <= 'z') ||
					(ch >= 'A' && ch <= 'Z') ||
					(ch >= 'а' && ch <= 'я') || ch == 'ё' ||
					(ch >= 'А' && ch <= 'Я') || ch == 'Ё' ||
					(ch >= '0' && ch <= '9') ||
					(ch == '_');
		}

		private bool ExtractWord(string file, int fileId, ref int i)
		{
			if (IsWord(file[i]))
			{
				var begin = i;
				i++;
				while (i < file.Length && IsWord(file[i])) { i++; }
				adder.AddWord(file.Substring(begin, i - begin), fileId);
				return true;
			}
			return false;
		}

		public Parser(IAdder adder, IndexSettings settings)
		{
			this.adder = adder;
			this.settings = settings;
		}

		public bool Parse(AddInfo info)
		{
			try
			{
			    using (var s = new StreamReader(File.OpenRead(info.Path), Encoding.UTF8))
			    {
			        while (!s.EndOfStream)
			        {
			            ParseFileData(s.ReadLine(), info.Id);
			        }
			    }
			}
			catch
			{
				return false;
			}
			return true;
		}
		public void ParseFileData(string data, int fileId)
		{
			var i = 0;
			while (i < data.Length)
			{
				GoFirstNoSpace(data, ref i);
				if (i >= data.Length) continue;
				if (settings.SkipComments && SkipComments(data, ref i)) continue;
				if (settings.DecodeStrings && ExtractString(data, fileId, ref i))
					continue;
				if (ExtractWord(data, fileId, ref i))
					continue;
				i++;
			}
		}
	}
}
