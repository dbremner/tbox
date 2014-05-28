using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.WpfControls.Code.Dialogs;

namespace Mnk.Library.WpfControls.Components.Units
{
	public interface IUnit
	{
		void Configure<T>(Collection<T> items, BaseDialog dialog)
			where T : Data, ICloneable, new();
		void Unconfigure();
        Control Control { get; }
	}
}
