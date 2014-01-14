using System;

namespace Mnk.Library.Common.MT
{
	public interface IUpdater : IDisposable
	{
		void Update(float value);
		void Update(string caption, float value);
		bool UserPressClose { get; }
		void Update(Func<int, string> caption, int current, int total);
	}
}
