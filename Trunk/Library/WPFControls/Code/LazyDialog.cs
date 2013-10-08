using System;
using LibsLocalization.WPFControls;
using WPFControls.Dialogs;

namespace WPFControls.Code
{
	public sealed class LazyDialog<TDialog> : Lazy<TDialog>, IDisposable
		where TDialog : DialogWindow
	{
		public string Id { get; private set; }
		public LazyDialog(Func<TDialog> creator, string id) : base(creator)
		{
			Id = id;
		}

		public bool IsVisible
		{
			get { return IsValueCreated && Value.IsVisible; }
		}

		public void Do(Action<Action> syncronizer, Action<TDialog> action)
		{
			if (!IsValueCreated)
			{
				DialogsCache.ShowProgress(u => syncronizer(()=>action(Value)), WPFControlsLang.CreateDialog);
			}
			else action(Value);
		}

		public void Hide()
		{
			if (IsValueCreated) Value.Hide();
		}

		public void Dispose()
		{
			if (!IsValueCreated) return;
			Value.Close();
			var d = Value as IDisposable;
			if(d!=null)d.Dispose();
		}
	}
}
