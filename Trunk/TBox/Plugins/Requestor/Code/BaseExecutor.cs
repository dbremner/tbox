using System;
using System.Windows;
using Common.Network;
using Localization.Plugins.Requestor;
using Requestor.Code.Settings;
using WPFControls.Dialogs;

namespace Requestor.Code
{
	public class BaseExecutor
	{
		private readonly Request r = new Request();

		public void Execute(Window owner, Op operation, Action<ResponseInfo> onEnd)
		{
			DialogsCache.ShowProgress(
				u => Work(
					(Op)operation.Clone(),
					onEnd
                    ), RequestorLang.MakeRequestTo + operation.Key, owner);
		}

		private void Work(Op op, Action<ResponseInfo> onEnd)
		{
			var response = r.GetResult(op.Request.Url, op.Request.Method, op.Request.Body, op.Request.Headers);
			onEnd(response);
		}
	}
}
