using System;
using Mnk.Library.WPFWinForms;

namespace Mnk.Library.WPFControls.Code.Log
{
	public sealed class TrayLog : CaptionedLog, ICaptionedLog
	{
		private readonly TrayIcon notifyIcon;
		private readonly int timeOut;
		private readonly TipIcon icon;

		public TrayLog(TrayIcon notifyNotifyIcon,
			int timeOut = 1000, TipIcon icon = TipIcon.Info)
		{
			notifyIcon = notifyNotifyIcon;
			this.timeOut = timeOut;
			this.icon = icon;
		}

		protected override void ShowMessage(string caption, string value)
		{
			notifyIcon.ShowBalloonTip(timeOut, caption, value, icon);
		}

		public string HoverText
		{
			get { return notifyIcon.HoverText; }
			set { notifyIcon.HoverText = value; }
		}

		public void Write(string caption, string value)
		{
			ShowMessage(caption, value);
		}
	}
}
