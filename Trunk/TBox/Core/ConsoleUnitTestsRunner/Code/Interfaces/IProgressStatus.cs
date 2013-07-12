namespace ConsoleUnitTestsRunner.Code.Interfaces
{
	public interface IProgressStatus
	{
		void Update(int allCount, int count, int failed);
		void Update(string text);
		bool UserPressClose { get; }
	}
}
