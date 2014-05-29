using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Mnk.Library.Common.MT;
using Mnk.Library.Common.Tools;
using Mnk.Library.Localization.WPFControls;
using Mnk.Library.WpfControls.Components;
using Mnk.Library.WpfControls.Tools;

namespace Mnk.Library.WpfControls.Dialogs.PerfomanceCounters
{
	/// <summary>
	/// Interaction logic for SelectPerfomanceCounters.xaml
	/// </summary>
	public partial class SelectPerfomanceCounters
	{
		private ObservableCollection<Entity> knownCounters;
		private ObservableCollection<SelectedEntity> userCounters;
		public static readonly DependencyProperty ItemsSourceProperty =
			DpHelper.Create<SelectPerfomanceCounters, ObservableCollection<SelectedEntity>>
				("UserCounters", (s, v) => s.UserCounters = v);
		public ObservableCollection<SelectedEntity> UserCounters
		{
			get { return userCounters; }
			set { 
				AddedCounters.ItemsSource = userCounters = value;
				OnOnDataChanged();
				OnSelectedCounter(null, null);
				AddedCounters_OnSelectedItemChanged(null, null);
			}
		}
		
		public event EventHandler OnDataChanged;

		protected virtual void OnOnDataChanged()
		{
			var handler = OnDataChanged;
			if (handler != null) handler(this, null);
		}

		public SelectPerfomanceCounters()
		{
			knownCounters = new ObservableCollection<Entity>();
			InitializeComponent();
		}

		public string BuildCurrentName()
		{
			var selected = Counters.SelectedItem as Entity;
			if (selected == null) return string.Empty;
			var ret = selected.Title;
			if (selected.Parent!=null)
				return selected.Parent.Title + '/' + ret;
			return ret;
		}

		private void OnSelectedCounter(object sender, RoutedEventArgs e)
		{
			var selected = Counters.SelectedItem as Entity;
			if (selected != null)
			{
				Info.Text = selected.ToolTip;
				Instances.ItemsSource = (selected.Parent==null) ? selected.Instances : selected.Parent.Instances;
				btnAdd.IsEnabled = true;
			}
			else
			{
				Instances.ItemsSource = null;
				Info.Text = string.Empty;
				btnAdd.IsEnabled = false;
			}
		}

		private void RefreshClick(object sender, RoutedEventArgs e)
		{
			Counters.ItemsSource = null;
			knownCounters = new ObservableCollection<Entity>();
			DialogsCache.ShowProgress(
				FillCounters, WPFControlsLang.ScanningForCounters, ControlsExtensions.GetParentWindow(this));
		}

		private void FillCounters(IUpdater u)
		{
			var i = 0;
			var cats = PerformanceCounterCategory.GetCategories().AsParallel().OrderBy(x => x.CategoryName).ToArray();
			foreach (var cat in cats)
			{
				var item = new Entity
					           {
						           Title = cat.CategoryName, 
								   ToolTip = cat.CategoryType + Environment.NewLine + cat.CategoryHelp
					           };
				var names = cat.GetInstanceNames().OrderBy(x=>x).ToArray();
				if (names.Length > 0)
				{
					item.Instances = new ObservableCollection<string>(names);
					foreach (var p in cat.GetCounters(names.First()).OrderBy(x => x.CounterName))
					{
						AddCounter(item, p);
					}
				}
				else
				{
					foreach (var p in cat.GetCounters().OrderBy(x => x.CounterName))
					{
						AddCounter(item, p);
					}
				}
				u.Update(cat.CategoryName, i++/(float)cats.Length);
				knownCounters.Add(item);
			}
			Mt.Do(Counters, ()=>Counters.ItemsSource = knownCounters);
		}

		private static void AddCounter(Entity item, PerformanceCounter p)
		{
			using (p)
			{
				item.Children.Add(new Entity
				{
					Title = p.CounterName,
					Parent = item,
					ToolTip = p.CounterType + Environment.NewLine + GetHelp(p)
				});
			}
		}

		private static string GetHelp(PerformanceCounter p)
		{
			try
			{
				return p.CounterHelp;
			}
			catch
			{
				return string.Empty;
			}
		}

		private void AddClick(object sender, RoutedEventArgs e)
		{
			var counter = Counters.SelectedItem as Entity;
			if(counter == null)return;
			if (counter.Parent != null)
			{
				AppendCounters(counter);
			}
			else
			{
				foreach (var child in counter.Children)
				{
					AppendCounters(child);
				}
			}
			OnOnDataChanged();
		}

		private void AppendCounters(Entity counter)
		{
			foreach (var instance in GetInstances())
			{
				var name = GenerateName(counter, instance);
				if (!UserCounters.Contains(name, x => x.ToString()))
				{
					UserCounters.Insert(
						new SelectedEntity
							{
								Category = counter.Parent.Title,
								Name = counter.Title,
								Instance = instance
							}, x => x.ToString());
				}
			}
		}

		private IEnumerable<string> GetInstances()
		{
			if (Instances.Items.Count == 0) return new[] {string.Empty};
			return (Instances.SelectedItems.Count == 0 ? 
				Instances.Items.Cast<string>() : 
				Instances.SelectedItems.Cast<string>()).ToArray();
		}

		public string GenerateName(Entity e, string instanceName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}\t{1}\t{2}", e.Parent.Title, e.Title, instanceName);
		}

		private void RemoveClick(object sender, RoutedEventArgs e)
		{
			foreach (var entity in AddedCounters.SelectedItems.Cast<SelectedEntity>().ToArray())
			{
				UserCounters.Remove(entity);
			}
			OnOnDataChanged();
		}

		private void AddedCounters_OnSelectedItemChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
		{
			btnRemove.IsEnabled = AddedCounters.SelectedItem!=null;
		}
	}
}
