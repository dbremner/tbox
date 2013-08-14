using System;
using System.Windows.Controls;

namespace TBox.Code.Objects
{
	class ItemContainer
	{
		public PluginName Key { get; private set; }
		public Func<Control> Getter { get; private set; }

		public ItemContainer(PluginName key, Func<Control> getter)
		{
			Key = key;
			Getter = getter;
		}

		public override string ToString()
		{
			return Key.Name;
		}
	}
}
