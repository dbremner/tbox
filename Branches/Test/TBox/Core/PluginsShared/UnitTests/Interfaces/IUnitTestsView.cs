using Common.UI.ModelsContainers;
using PluginsShared.UnitTests.Settings;

namespace PluginsShared.UnitTests.Interfaces
{
	public interface IUnitTestsView
	{
		void UpdateFilter(bool onlyFailed);
		void SetItems(CheckableDataCollection<Result> items);
		//ItemsSource=items;
		void Clear();
		//ItemsSource=null;
	}
}
