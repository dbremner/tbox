namespace Common.AutoUpdate
{
	public interface IUpdater
	{
		bool NeedUpdate();
		void Copy(string newPath);
		void Update();
	}
}
