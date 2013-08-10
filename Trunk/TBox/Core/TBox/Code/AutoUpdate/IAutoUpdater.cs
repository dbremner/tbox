namespace TBox.Code.AutoUpdate
{
	interface IAutoUpdater
	{
		bool TryUpdate(bool manual = false);
	}
}
