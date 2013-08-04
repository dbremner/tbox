using System;
using System.Windows;
using Common.Network;
using Requestor.Code.Settings;
using Requestor.Components;
using WPFControls.Code;
using WPFControls.Code.OS;
using WPFControls.Dialogs.StateSaver;

namespace Requestor.Code
{
	public sealed class Executor : BaseExecutor, IDisposable
	{
		private readonly LazyDialog<RequestDialog> message = new LazyDialog<RequestDialog>(
			() => new RequestDialog(), "message");

		public void Execute(Window owner, Op operation, Config config, Action onEnd = null)
		{
			if (message.IsVisible) message.Hide();
			base.Execute(owner, operation, i => Finish(i, owner, operation.Key, config, onEnd));
		}

		private void Finish(ResponseInfo response, Window owner, string name, Config config, Action onEnd)
		{
			if (onEnd != null) onEnd();
			Mt.Do(owner,
				() =>
					{
						message.LoadState(config.States);
						message.Value.ShowDialog(string.Format("Request '{0}' results, time: {1}", name, response.Time/1000.0), response, owner);
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
