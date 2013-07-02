using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Base.Log;

namespace WFControls.Log
{
	public abstract class CaptionedLog : AbstractLog
	{
		protected abstract void ShowMessage(string caption, string value);
		public override void Write(string value)
		{
			int id = value.IndexOf(Environment.NewLine);
			int nextId = id + Environment.NewLine.Length;
			if (id > 0 && nextId < value.Length)
			{
				ShowMessage(value.Substring(0, id), 
					value.Substring(nextId, value.Length - nextId));
			}
			ShowMessage(string.Empty, value);
		}
	}
}
