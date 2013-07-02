using System;

namespace WPFControls.Dialogs
{
	public interface IDialog : IDisposable
	{
		bool IsVisible { get; }
	}
}
