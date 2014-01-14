using System.Text;

namespace Mnk.TBox.Core.PluginsShared.Encoders
{
	public class CCodeFormatter : BracketsFormatter
	{
		private readonly char lineDivider;
		public CCodeFormatter(char begin = '{', char end = '}', char lineDivider = ';') : base(begin, end)
		{
			this.lineDivider = lineDivider;
		}

		protected override void AppendText(StringBuilder sb, string s)
		{
			s = s.Trim();
			if (string.IsNullOrWhiteSpace(s)) return;
			var pos = 0;
			while(true)
			{
				var last = s.FindFirstNotString(lineDivider, pos);
				if (last != -1)
				{
					var text = s.Substring(pos, last - pos + 1).Trim();
					if (text.Length == 1 && IsDivider(text[0]))
					{
						sb.Append(lineDivider);
					}
					else
					{
						base.AppendText(sb, text);
					}
					pos = last + 1;
					continue;
				}
				base.AppendText(sb, s.Substring(pos, s.Length - pos));
				break;
			}
		}

		private bool IsDivider(char ch)
		{
			return ch == lineDivider || ch == ',';
		}
	}
}
