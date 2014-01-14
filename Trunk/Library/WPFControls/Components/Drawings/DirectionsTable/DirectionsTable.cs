using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Mnk.Library.Common.Tools;
using Mnk.Library.WPFControls.Components.Drawings.Background;
using Mnk.Library.WPFControls.Tools;

namespace Mnk.Library.WPFControls.Components.Drawings.DirectionsTable
{
	public sealed class DirectionsTable : GraphPanel
	{
		private readonly IList<IDirectionable> items = new List<IDirectionable>();
		private readonly List<string> columns = new List<string>();
		private int lastCalculated = -1;
		private int selected = -1;
		private readonly Pen pen = new Pen(Brushes.Black, 1);
		private readonly Pen directionPen = new Pen(Brushes.DarkBlue, 2);
		private readonly Pen selectionPen = new Pen(Brushes.Lime, 1);
		private readonly Brush columnBrush = Brushes.LightCyan;
		private const int ColumnHeight = 20;
		private const int RowHeight = 40;
		private readonly object locker = new object();

		public int MaxCount = 10;
		public event Action<object, IDirectionable> Selected;
		public event EventHandler Cleared;

		private void OnCleared()
		{
			var handler = Cleared;
			if (handler != null) handler(this, null);
		}

		private void OnSelected(object sender, IDirectionable arg)
		{
			var handler = Selected;
			if (handler != null) handler(sender, arg);
		}

		public DirectionsTable()
		{
			Back = new SolidBackground(Brushes.White, new Pen(Brushes.White,0));
			RenderOptions.SetEdgeMode(this, EdgeMode.Unspecified);
		}

		public void Clear()
		{
			lock (locker)
			{
				lastCalculated = -1;
				selected = -1;
				items.Clear();
				columns.Clear();
			}
			OnSelected(this, null);
			OnCleared();
		}

		public void Add(IDirectionable item)
		{
			lock (locker)
			{
				columns.InsertIfNotContains(item.From);
				columns.InsertIfNotContains(item.To);
				items.Add((IDirectionable)item.Clone());
				if (items.Count > MaxCount)
				{
					items.RemoveAt(0);
					--selected;
					--lastCalculated;
				}
			}
		}

		public override bool Any
		{
			get { lock (locker){return items.Any();} }
		}

		public int EntriesCount
		{
			get { lock (locker){return items.Count;} }
		}

		protected override void Update(Rect r)
		{
			lock (locker)
			{
				lastCalculated = items.Count;
				MinHeight = ColumnHeight + ((lastCalculated < 0) ? 0 : lastCalculated) * RowHeight;
			}
		}

		protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			IDirectionable dir = null;
			lock (locker)
			{
				if (lastCalculated < 0) return;
				var p = e.GetPosition(this);
				var id = (p.Y - ColumnHeight)/RowHeight;
				selected = (id < 0 || id >= lastCalculated) ? -1 : (int)id;
				dir = (selected == -1) ? null : (IDirectionable)items[selected].Clone();
			}
			OnSelected(this, dir);
			Redraw();
		}

		protected override void Render(DrawingContext dc, Rect r)
		{
			lock (locker)
			{
				var cx = (int)(r.Width / columns.Count);
				RenderColumns(dc, cx);
				RenderRows(dc, cx);
				RenderSelected(dc, r, selected);
			}
		}

		private void RenderColumns(DrawingContext dc, int cx)
		{
			var x = 0.0;
			var cx2 = cx/2;
			foreach (var title in columns)
			{
				dc.DrawRectangle(columnBrush, pen, new Rect(x+1, 1, cx-2, ColumnHeight-2));
				dc.DrawAlignedText(new Point(x + cx2, ColumnHeight / 2), title);
				dc.DrawLine(pen, new Point(x + cx2, ColumnHeight), new Point(x + cx2, ColumnHeight + RowHeight*lastCalculated));
				x += cx;
			}
		}

		private void RenderRows(DrawingContext dc, int cx)
		{
			if (lastCalculated < 0) return;
			var x = cx/2;
			var y = ColumnHeight + RowHeight/2;
			foreach (var item in items.Take(lastCalculated))
			{
				var begin = columns.BinarySearch(item.From);
				var end = columns.BinarySearch(item.To);
				dc.DrawArrow(directionPen, new Point(x + cx * begin, y), new Point(x + cx * end, y));
				dc.DrawAlignedText(new Point(x+cx*(begin+end)/2.0, y), item.Title);
				y += RowHeight;
			}
		}

		private void RenderSelected(DrawingContext dc, Rect r, int sel)
		{
			if (sel < 0) return;
			dc.DrawRectangle(Brushes.Transparent, selectionPen,
				new Rect(0, ColumnHeight + sel*RowHeight, r.Width, RowHeight));
		}

	}
}
