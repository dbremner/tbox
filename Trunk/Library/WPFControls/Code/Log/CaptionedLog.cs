using System;
using Mnk.Library.Common.Log;

namespace Mnk.Library.WpfControls.Code.Log
{
	public abstract class CaptionedLog : AbstractLog
	{
		protected abstract void ShowMessage(string caption, string value);

		public override void Write(string value)
		{
			var id = value.IndexOf(Environment.NewLine, StringComparison.Ordinal);
			var nextId = id + Environment.NewLine.Length;
			if (id > 0 && nextId < value.Length)
			{
				ShowMessage(value.Substring(0, id),
					value.Substring(nextId, value.Length - nextId));
			}
			ShowMessage(string.Empty, value);
		}
	}
}
