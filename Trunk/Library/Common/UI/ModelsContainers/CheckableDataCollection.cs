using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common.UI.Model;

namespace Common.UI.ModelsContainers
{
	[Serializable]
	public class CheckableDataCollection<T> : ObservableCollection<T>, ICheckableDataCollection
		where T : class, ICheckableData
	{
		public CheckableDataCollection(){}
		public CheckableDataCollection(IEnumerable<T> collection) : base(collection) { }
		public CheckableDataCollection(List<T> collection) : base(collection) { }

		public void SetCheck(bool isChecked = true)
		{
			foreach (var t in this)
			{
				t.IsChecked = isChecked;
			}
		}

		public bool? IsChecked
		{
			get
			{
				if (Count > 0)
				{
					var count = CheckedValuesCount;
					if (count == 0) return false;
					if (count == Count) return true;
				}
				return null;
			}
		}

		public int CheckedValuesCount
		{
			get { return this.Sum(x => x.IsChecked ? 1 : 0); }
		}

		public int ValuesCount
		{
			get { return Items.Count; }
		}

		public void SetCheck(int id, bool isChecked = true)
		{
			this[id].IsChecked = isChecked;
		}

		public void SetCheck(string name, bool isChecked = true)
		{
			this.Where(x => x.Key == name).FirstOrDefault().IsChecked = isChecked;
		}

		public bool GetCheck(int id)
		{
			return this[id].IsChecked;
		}

		public bool GetCheck(string name)
		{
			return this.Where(x => x.Key == name).Select(i => i.IsChecked).FirstOrDefault();
		}

		public int[] GetCheckedIndexes()
		{
			var ret = new List<int>();
			for (var i=0; i<Count; ++i)
			{
				if (this[i].IsChecked)
					ret.Add(i);
			}
			return ret.ToArray();
		}

		public IEnumerable<T> CheckedItems
		{
			get { return this.Where(x => x.IsChecked); }
		}

		public IEnumerable<T> UnCheckedItems
		{
			get { return this.Where(x => !x.IsChecked); }
		}
	}
}
