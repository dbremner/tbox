using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Base.Log;

namespace WFControls.Log
{
	public sealed class TrayLog : CaptionedLog
	{
		private readonly NotifyIcon m_notifyIcon;
		private readonly int m_timeout;
		private readonly ToolTipIcon m_icon;
		
		public TrayLog(NotifyIcon notifyNotifyIcon, 
			int timeout=1000, ToolTipIcon icon=ToolTipIcon.Info)
		{
			m_notifyIcon = notifyNotifyIcon;
			m_timeout = timeout;
			m_icon = icon;
		}

		protected override void ShowMessage(string caption, string value)
		{
			m_notifyIcon.ShowBalloonTip(m_timeout, caption, value, m_icon);
		}
	}
}
