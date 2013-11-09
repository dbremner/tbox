using System.Collections.Specialized;

namespace Common.UI.ModelsContainers
{
	public interface ICheckableDataCollection : ICheckableList, INotifyCollectionChanged
	{
		bool GetCheck(string name);
		int[] GetCheckedIndexes();
	}
}
