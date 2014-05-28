﻿using System.Windows.Media;
using Mnk.Library.WpfControls.Components.Drawings.Graphics;
using Mnk.Library.WpfControls.Components.Drawings.Graphics.Painters;

namespace Mnk.TBox.Core.PluginsShared.LoadTesting.Statistic
{
	public class GraphicsInfo
	{
		public IGraphic AverageTime { get; private set; }
		public IGraphic MinTime { get; private set; }
		public IGraphic MaxTime { get; private set; }

		public GraphicsInfo()
		{
			AverageTime = new Graphic(new Pen(Brushes.GreenYellow, 2), new PolylinesStretchGraphicPainter());
			MinTime = new Graphic(new Pen(Brushes.Blue, 1), new PolylinesStretchGraphicPainter());
			MaxTime = new Graphic(new Pen(Brushes.Red, 1), new PolylinesStretchGraphicPainter());
		}

		public void Clear()
		{
			AverageTime.Clear();
			MinTime.Clear();
			MaxTime.Clear();
		}

		public void Draw(OperationStatistic s)
		{
			if (s.Count == 0) return;
			AverageTime.Add(s.Time / (float)s.Count);
			MinTime.Add(s.MinTime);
			MaxTime.Add(s.MaxTime);
		}
	}
}
