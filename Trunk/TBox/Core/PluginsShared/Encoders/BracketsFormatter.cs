using System.Collections.Generic;
using System.Text;
using Common.Encoders;

namespace PluginsShared.Encoders
{
	public class BracketsFormatter
	{
		private readonly char begin;
		private readonly char end;
		public BracketsFormatter(char begin = '{', char end = '}')
		{
			this.begin = begin;
			this.end = end;
		}

		public string Format(string s)
		{
			var sb = new StringBuilder();
			var pos = 0;
			while (true)
			{
				var start = s.FindFirstNotString(begin, pos);
				if (start != -1)
				{
					var last = s.FindFirstNotString(end, start + 1, new KeyValuePair<char, char>(begin, end));
					if (last != -1)
					{
						var block = s.Substring(start + 1, last - start - 1);
						AppendText(sb, s.Substring(pos, start - pos));
						sb.AppendIdent(0).Append(begin);
						sb.AppendIdentedText(Format(block), 1);
						sb.AppendIdent(0).Append(end);
						pos = last + 1;
						continue;
					}
				}
				AppendText(sb, s.Substring(pos, s.Length - pos));
				break;
			}
			return sb.ToString();
		}

		protected virtual void AppendText(StringBuilder sb, string s)
		{
			s = s.Trim();
			if(string.IsNullOrWhiteSpace(s))return;
			sb.AppendIdent(0).Append(s);
		}
	}
}
