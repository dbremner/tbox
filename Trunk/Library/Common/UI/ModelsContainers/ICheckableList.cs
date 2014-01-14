namespace Mnk.Library.Common.UI.ModelsContainers
{
	public interface ICheckableList : ICheckable
	{
		int CheckedValuesCount { get; }
		int ValuesCount { get; }
		void SetCheck(int id, bool isChecked = true);
		bool GetCheck(int id);
	}
}
