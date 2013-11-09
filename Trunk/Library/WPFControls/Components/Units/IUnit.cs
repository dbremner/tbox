﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Common.UI.Model;
using WPFControls.Code.Dialogs;

namespace WPFControls.Components.Units
{
	public interface IUnit
	{
		void Configure<T>(Collection<T> items, BaseDialog dialog)
			where T : Data, ICloneable, new();
		void Unconfigure();
        Control Control { get; }
	}
}
