using System.Collections.Generic;

namespace Mnk.TBox.Core.PluginsShared.Encoders
{
    public static class CStringFinder
    {
		class FindContext
		{
			public int Pos;
			public bool SingleQuote;
			public bool DoubleQuote;

			public FindContext(int start)
			{
				Pos = start;
				SingleQuote = false;
				DoubleQuote = false;
			}
		}
		public static int FindFirstNotString(this string s, char target, int start)
		{
			var ctx = new FindContext(start);
			for (; ctx.Pos < s.Length; ++ctx.Pos)
			{
				char ch;
				if (!SkipQuotesAndComments(s, out ch, ctx)) continue;
				if (ch == target) return ctx.Pos;
			}
			return -1;
		}


		public static int FindFirstNotString(this string s, char target, int start, KeyValuePair<char, char> extraCharsToSkip)
		{
			var ctx = new FindContext(start);
			var extraChars = -1;
			for (; ctx.Pos < s.Length; ++ctx.Pos)
			{
				char ch;
				if (!SkipQuotesAndComments(s, out ch, ctx)) continue;
				if (ch == extraCharsToSkip.Key)
				{
					++extraChars;
				}
				else if (extraChars != -1 && ch == extraCharsToSkip.Value)
				{
					--extraChars;
				}
				else if (extraChars == -1 && ch == target) return ctx.Pos;
			}
			return -1;
		}

		private static bool SkipQuotesAndComments(string s, out char ch, FindContext ctx)
		{
			ch = s[ctx.Pos];
			if ((ctx.SingleQuote || ctx.DoubleQuote) && ch == '\\')
			{
				if (ctx.Pos + 2 < s.Length)
				{
					ctx.Pos += 2;
					return SkipQuotesAndComments(s, out ch, ctx);
				}
				ctx.Pos++;
				return false;
			}
			if (ch == '\'' && !ctx.DoubleQuote)
			{
				ctx.SingleQuote = !ctx.SingleQuote;
			}
			else if (ch == '\"' && !ctx.SingleQuote)
			{
				ctx.DoubleQuote = !ctx.DoubleQuote;
			}
			else if (!ctx.SingleQuote && !ctx.DoubleQuote)
			{
				if (ch == '/')
				{
					if (ctx.Pos + 1 < s.Length)
					{
						++ctx.Pos;
						ch = s[ctx.Pos];
						if (ch == '/')
						{
							return GoToSymbol("\n", s, ref ch, ctx);
						}
						if (ch == '*')
						{
							return GoToSymbol("*/", s, ref ch, ctx);
						}
					}
					return false;
				}
				return true;
			}
			return false;
		}
	    private static bool GoToSymbol(string target, string s, ref char ch, FindContext ctx)
	    {
			var id = s.IndexOf(target, ctx.Pos+1, System.StringComparison.Ordinal);
		    if (id != -1)
		    {
				ctx.Pos = id + target.Length;
				return (ctx.Pos < s.Length) &&
			           SkipQuotesAndComments(s, out ch, ctx);
		    }
			ctx.Pos = s.Length;
		    return false;
	    }
    }
}
