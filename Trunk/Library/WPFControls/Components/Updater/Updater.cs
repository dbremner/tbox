using System;

namespace WPFControls.Components.Updater
{
	class Updater : SimpleUpdater<Progress>
	{
		private readonly long begin = Environment.TickCount;
		private long lastTime = 0;
		public Updater(Progress owner):base(owner){}

		public override void Update(string caption, float value)
		{
			if (CanRun()) return;
			Owner.SetMessage(caption);
			Update(value);
		}

		private bool CanRun()
		{
			var time = Environment.TickCount;
			if (time - lastTime < 100) return true;
			lastTime = time;
			return false;
		}

		public override void Update(Func<int, string> caption, int current, int total)
		{
			if (CanRun()) return;
			var message = caption((int)(lastTime - begin)/1000);
			Owner.SetMessage(message);
			base.Update(caption, current, total);
		}
	}
}
