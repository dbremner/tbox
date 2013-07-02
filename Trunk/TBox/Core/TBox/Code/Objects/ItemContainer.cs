using System;
using System.Windows.Controls;

namespace TBox.Code.Managers
{
	class ItemContainer
	{
		public string Name { get; private set; }
		public Func<Control> Getter { get; private set; }

		public ItemContainer(string name, Func<Control> getter)
		{
			Name = name;
			Getter = getter;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
