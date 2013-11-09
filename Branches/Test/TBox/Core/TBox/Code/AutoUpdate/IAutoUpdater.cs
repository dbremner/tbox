namespace TBox.Code.AutoUpdate
{
	public interface IAutoUpdater
	{
		bool TryUpdate(bool manual = false);
	}
}
