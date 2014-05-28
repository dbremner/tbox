using System;

namespace Mnk.Library.WpfControls.Components.Updater
{
	class Updater : SimpleUpdater<Progress>
	{
		private readonly long begin = Environment.TickCount;
		public Updater(Progress owner):base(owner){}

		public override void Update(string caption, float value)
		{
			Owner.SetMessage(caption);
			Update(value);
		}

		public override void Update(Func<int, string> caption, int current, int total)
		{
            var message = caption((int)(Environment.TickCount - begin) / 1000);
			Owner.SetMessage(message);
			base.Update(caption, current, total);
		}
	}
}
