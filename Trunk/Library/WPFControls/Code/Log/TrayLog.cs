using System;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;

namespace Mnk.Library.WpfControls.Code.Log
{
	public sealed class TrayLog : CaptionedLog, ICaptionedLog
	{
		private readonly TrayIcon notifyIcon;
		private readonly int timeout;
		private readonly TipIcon icon;

		public TrayLog(TrayIcon notifyNotifyIcon,
			int timeout = 1000, TipIcon icon = TipIcon.Info)
		{
			notifyIcon = notifyNotifyIcon;
			this.timeout = timeout;
			this.icon = icon;
		}

		protected override void ShowMessage(string caption, string value)
		{
			notifyIcon.ShowBalloonTip(timeout, caption, value, icon);
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
