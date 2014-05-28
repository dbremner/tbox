using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Mnk.Library.WpfControls.Components.Drawings.Background;

namespace Mnk.Library.WpfControls.Components.Drawings
{
	public abstract class GraphPanel : UserControl
	{
		public IBackground Back { get; set; }
		private bool shouldRecalc;
		private readonly DispatcherTimer resizeTimer =
			new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 1000), IsEnabled = false };

		protected GraphPanel()
		{
			shouldRecalc = false;
			resizeTimer.Tick += ResizeTimerOnTick;
		}

		private void ResizeTimerOnTick(object sender, EventArgs eventArgs)
		{
			resizeTimer.IsEnabled = false;
			Redraw();
		}

		public void Redraw()
		{
			shouldRecalc = true;
			InvalidateVisual();
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			resizeTimer.IsEnabled = true;
			resizeTimer.Stop();
			resizeTimer.Start();
			InvalidateVisual();
		}

		protected override void OnRender(DrawingContext dc)
		{
			var r = new Rect(0, 0, ActualWidth, ActualHeight);
			Back.Draw(dc, r);
			if (Any)
			{
				if (shouldRecalc)
				{
					Update(r);
					shouldRecalc = false;
				}
				Render(dc, r);
			}
			base.OnRender(dc);
		}

		public abstract bool Any { get; }
		protected abstract void Update(Rect rect);
		protected abstract void Render(DrawingContext dc, Rect rect);
	}
}
