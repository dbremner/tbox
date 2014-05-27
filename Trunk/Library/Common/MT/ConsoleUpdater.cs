using System;

namespace Mnk.Library.Common.MT
{
    public sealed class ConsoleUpdater : IUpdater
    {
        private readonly int time = Environment.TickCount;

        public ConsoleUpdater()
        {
            UserPressClose = false;
        }

        public void Update(float value)
        {
            Update(string.Empty, value);
        }

        public void Update(string caption, float value)
        {
            System.Console.WriteLine(
                "[{0}]{1}", 
                PrepareProgress(value), 
                string.IsNullOrEmpty(caption) ? string.Empty : ":\t" + caption);
        }

        public bool UserPressClose { get; private set; }
        public void Update(Func<int, string> caption, int current, int total)
        {
            Update(
                caption((Environment.TickCount - time) / 1000), 
                current / (float)total);
        }

        private static float PrepareProgress(float value)
        {
            return (int)(value * 100);
        }

        public void Dispose()
        {
        }
    }
}
