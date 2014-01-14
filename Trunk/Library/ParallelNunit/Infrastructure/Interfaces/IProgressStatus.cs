using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Execution;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Interfaces
{
	public interface IProgressStatus
	{
		void Update(int allCount, Result[] items, int failed);
		void Update(string text);
		bool UserPressClose { get; }
	}
}
