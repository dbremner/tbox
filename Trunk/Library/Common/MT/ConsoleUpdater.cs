using System;

namespace Common.MT
{
	public class ConsoleUpdater : IUpdater
	{
		private readonly int time = Environment.TickCount;
		public void Update(float value)
		{
			System.Console.WriteLine("[{0}]", PrepareProgress(value));
		}

		public void Update(string caption, float value)
		{
			System.Console.WriteLine("[{0}]: {1}", PrepareProgress(value), caption);
		}

		public bool UserPressClose { get; private set; }
		public void Update(Func<int, string> caption, int current, int total)
		{
			System.Console.WriteLine("[{0}]: {1}",
				PrepareProgress(current / (float)total),
				caption((Environment.TickCount - time)/1000));
		}

		private static float PrepareProgress(float value)
		{
			return (int)(value * 100);
		}

	}
}
