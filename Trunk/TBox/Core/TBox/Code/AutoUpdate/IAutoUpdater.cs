namespace Mnk.TBox.Core.Application.Code.AutoUpdate
{
	public interface IAutoUpdater
	{
		bool TryUpdate(bool manual = false);
	}
}
