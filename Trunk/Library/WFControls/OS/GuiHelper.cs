using System.Windows.Forms;

namespace WFControls.OS
{
	public static class GuiHelper
	{
		public static void ArrayToComponent(string[] names, ListBox component)
		{
			component.Items.Clear();
			if (names != null)
			{
				foreach (string name in names)
				{
					component.Items.Add(name);
				}
			}
		}
		public static void ArrayToComponent(string[] names, ListView component)
		{
			component.Items.Clear();
			if (names != null)
			{
				foreach (string name in names)
				{
					component.Items.Add(name);
				}
			}
		}
		public static void ArrayToComponent(string[] names, ComboBox component)
		{
			component.Items.Clear();
			if (names != null)
			{
				foreach (string name in names)
				{
					component.Items.Add(name);
				}
			}
		}

		public static void ComponentToArray(out string[] names, ListBox component)
		{
			if (component.Items.Count == 0)
			{
				names = null;
			}
			else
			{
				names = new string[component.Items.Count];
				for (int i = 0; i < component.Items.Count; i++)
				{
					names[i] = (string)component.Items[i];
				}
			}
		}
		public static void ComponentToArray(out string[] names, ListView component)
		{
			if (component.Items.Count == 0)
			{
				names = null;
			}
			else
			{
				names = new string[component.Items.Count];
				for (int i = 0; i < component.Items.Count; i++)
				{
					names[i] = component.Items[i].Text;
				}
			}
		}
		public static void ComponentToArray(out string[] names, ComboBox component)
		{
			if (component.Items.Count == 0)
			{
				names = null;
			}
			else
			{
				names = new string[component.Items.Count];
				for (int i = 0; i < component.Items.Count; i++)
				{
					names[i] = component.Items[i].ToString();
				}
			}
		}
	}
}
