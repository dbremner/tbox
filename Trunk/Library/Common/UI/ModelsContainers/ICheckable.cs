namespace Mnk.Library.Common.UI.ModelsContainers
{
	public interface ICheckable
	{
		void SetCheck(bool isChecked = true);
		bool? IsChecked { get; }
	}
}
