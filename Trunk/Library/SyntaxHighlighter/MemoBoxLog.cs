using System;
using Common.Base.Log;
using WFControls.OS;

namespace SyntaxHighlighter
{
	public partial class MemoBoxLog : MemoBox, IBaseLog
	{
		public MemoBoxLog()
		{
			InitializeComponent();
			MaxLogLength = 1000000;
		}

		public int MaxLogLength { get; set; }
		public void Write(string data)
		{
			Mt.Do(shBox, () =>
			{
				var txt = shBox.tbText;
				int textLength = 0;
				Mt.Do(txt, () => txt.AppendText(Environment.NewLine + data));
				Mt.Do(txt, () => textLength = txt.TextLength);
				if(textLength>MaxLogLength)
				{
					Mt.Do(txt, () => txt.Text = 
						txt.Text.Substring(textLength-MaxLogLength, MaxLogLength) );
					textLength = MaxLogLength;
				}
				Mt.Do(txt, () => txt.Caret.Position = textLength);
				Mt.Do(txt, txt.Scrolling.ScrollToCaret);
			});
		}

		public void Write(Exception ex, string value)
		{
			Write(string.Format("{0}{1}{2}", value, Environment.NewLine, ex));
		}
	}
}
