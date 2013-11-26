using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Common.Network;
using Localization.Plugins.Requestor;
using Requestor.Code.Settings;
using Requestor.Components;
using WPFControls.Code;
using WPFControls.Code.OS;
using WPFControls.Dialogs.StateSaver;
using WPFWinForms.Icons;

namespace Requestor.Code
{
	public sealed class Executor : BaseExecutor, IDisposable
	{
		private readonly LazyDialog<RequestDialog> message = new LazyDialog<RequestDialog>(
			() => new RequestDialog(), "message");

		public void Execute(Window owner, Op operation, Config config, Action onEnd, ImageSource icon)
		{
			if (message.IsVisible) message.Hide();
		    message.Value.Icon = icon;
			base.Execute(owner, operation, i => Finish(i, owner, operation.Key, config, onEnd), icon);
		}

		private void Finish(ResponseInfo response, Window owner, string name, Config config, Action onEnd)
		{
			if (onEnd != null) onEnd();
			Mt.Do(owner,
				() =>
					{
						message.LoadState(config.States);
                        message.Value.ShowDialog(string.Format(RequestorLang.RequestTemplate, name, response.Time / 1000.0), response, owner);
					});
		}

		public void Save(Config config)
		{
			message.SaveState(config.States);
		}

		public void Dispose()
		{
			message.Dispose();
		}
	}
}
