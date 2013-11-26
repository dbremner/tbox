using System;

namespace Common.MT
{
	public interface IUpdater
	{
		void Update(float value);
		void Update(string caption, float value);
		bool UserPressClose { get; }
		void Update(Func<int, string> caption, int current, int total);
	}
}
