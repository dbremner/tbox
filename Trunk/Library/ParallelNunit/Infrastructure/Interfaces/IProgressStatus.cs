using ParallelNUnit.Core;
using ParallelNUnit.Execution;

namespace ParallelNUnit.Infrastructure.Interfaces
{
	public interface IProgressStatus
	{
		void Update(int allCount, Result[] items, int failed);
		void Update(string text);
		bool UserPressClose { get; }
	}
}
