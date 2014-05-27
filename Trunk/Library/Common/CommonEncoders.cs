using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mnk.Library.Common
{
	public static class CommonEncoders
	{
		private const char Fill = '\t';

		public static StringBuilder AppendIndent(this StringBuilder sb, int indent)
		{
			if (sb.Length != 0)
				sb.AppendLine();
			return sb.Append(new string(Fill, indent));
		}

		public static StringBuilder AppendIndentedText(this StringBuilder sb, string text, int indent)
		{
			foreach (var line in text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
			{
				sb.AppendIndent(indent).Append(line);
			}
			return sb;
		}

		public static string Minimize(string value)
		{
			var text = value.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
			while (text.Contains("  "))
			{
				text = text.Replace("  ", " ");
			}
			return text;
		}

		private readonly static string[] CharsToEscape
			= new[] { "\\", "\"", "\'" };

		private static readonly IDictionary<string, string> ControlChars
			= new Dictionary<string, string>
				  {
					  {"\t", "t"},{"\r", "r"},{"\n", "n"},
					  {"\a", "a"},{"\b", "b"},{"\v", "v"},
					  {"\0", "0"},
				  };

		public static string EncodeString(string value)
		{
			var sb = new StringBuilder(value);
			foreach (var ch in CharsToEscape)
			{
				sb.Replace(ch, "\\" + ch);
			}
			foreach (var ch in ControlChars)
			{
				sb.Replace(ch.Key, "\\" + ch.Value);
			}
			return sb.ToString();
		}

		public static string DecodeString(string value)
		{
			var sb = new StringBuilder(value);
			for (var i = 0; i < sb.Length-1; ++i)
			{
				if (sb[i] != '\\')continue;
				var next = "" + sb[i + 1];
				var id = Array.IndexOf(CharsToEscape, next);
				if (id != -1)
				{
					sb.Remove(i,1);
					continue;
				}
				var items = ControlChars.Where(x => string.Equals(x.Value, next)).ToArray();
				if (!items.Any()) continue;
				sb.Remove(i,1);
				sb[i] = items.Single().Key[0];
			}
			return sb.ToString();
		}
	}
}
