using System;

namespace Common.MT
{
	public class NullUpdater : IUpdater
	{
		public void Update(float value)
		{
		}

		public void Update(string caption, float value)
		{
		}

		public bool UserPressClose { get; set; }
		public void Update(Func<int, string> caption, int current, int total)
		{
		}
	}
}
