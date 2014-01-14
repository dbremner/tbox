using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.WPFControls.Code.Dialogs;

namespace Mnk.Library.WPFControls.Components.Units
{
	public interface IUnit
	{
		void Configure<T>(Collection<T> items, BaseDialog dialog)
			where T : Data, ICloneable, new();
		void Unconfigure();
        Control Control { get; }
	}
}
