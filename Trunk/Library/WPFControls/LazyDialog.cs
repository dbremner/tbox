using System;
using System.Windows;
using Mnk.Library.Localization.WPFControls;
using Mnk.Library.WpfControls.Dialogs;

namespace Mnk.Library.WpfControls
{
	public sealed class LazyDialog<TDialog> : Lazy<TDialog>, IDisposable
		where TDialog : Window, IDisposable
	{
		public string Id { get; private set; }
		public LazyDialog(Func<TDialog> creator) : base(creator)
		{
			Id = typeof(TDialog).FullName;
		}

		public bool IsVisible
		{
			get { return IsValueCreated && Value.IsVisible; }
		}

		public void Do(Action<Action> syncronizer, Action<TDialog> action)
		{
			if (!IsValueCreated)
			{
				DialogsCache.ShowProgress(u => syncronizer(()=>action(Value)), WPFControlsLang.CreateDialog, null);
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
