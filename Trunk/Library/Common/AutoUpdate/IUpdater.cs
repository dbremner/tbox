namespace Mnk.Library.Common.AutoUpdate
{
	public interface IApplicationUpdater
	{
		bool NeedUpdate();
		void Copy(string newPath);
		void Update();
	}
}
