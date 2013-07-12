using Common.UI.ModelsContainers;
using ConsoleUnitTestsRunner.Code.Settings;

namespace ConsoleUnitTestsRunner.Code.Interfaces
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
