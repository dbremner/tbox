using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Mnk.Library.Common.Models;
using Mnk.Library.Common.UI.ModelsContainers;

namespace Mnk.Library.WpfControls.Components.CheckableTreeView
{
    /// <summary>
    /// Interaction logic for CheckableTreeView.xaml
    /// </summary>
    public partial class CheckableTreeView
    {
        private readonly ObservableCollection<Node> nodes;
        public CheckableTreeView()
        {
            nodes = new ObservableCollection<Node>();
            InitializeComponent();
            treeView.ItemsSource = nodes;
            this.SetValue(TreeViewResourcesProperty,treeView.Resources);
        }

        public static readonly DependencyProperty TreeViewResourcesProperty =
            DpHelper.Create<CheckableTreeView, ResourceDictionary>("TreeViewResources", new ResourceDictionary(), 
            (o, v) => o.TreeViewResources = v);
        [Ambient]
        public ResourceDictionary TreeViewResources
        {
            get { return (ResourceDictionary)GetValue(TreeViewResourcesProperty); }
            set
            {
                if(treeView.Resources == value)return;
                treeView.Resources = new ResourceDictionary();
                foreach (var item in value.Keys)
                {
                    treeView.Resources.Add(item, value[item]);
                }
                this.SetValue(TreeViewResourcesProperty, treeView.Resources);
            }
        }

        public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged
        {
            add { treeView.SelectedItemChanged += value; }
            remove { treeView.SelectedItemChanged -= value; }
        }

        public void Refresh()
        {
            treeView.Items.Refresh();
        }

        public IHasChildren SelectedValue
        {
           get 
           { 
               var node = (Node)treeView.SelectedValue;
               return node == null ? null : node.Data;
           }
        }

        public IEnumerable<IHasChildren> GetChecked()
        {
            return CollectNodes(nodes);
        }

        public void SetChecked(IList<IHasChildren> values)
        {
            SetChecked(nodes, values);
        } 

        public void SetChecked(IEnumerable<Node> items, IList<IHasChildren> values)
        {
            foreach (var node in items)
            {
                node.IsChecked = values.Contains(node.Data);
                SetChecked(node.Children, values);
            }
            treeView.Items.Refresh();
        } 

        public void SetItems(IEnumerable<IHasChildren> items)
        {
            nodes.Clear();
            if (items != null)
            {
                foreach (var item in items)
                {
                    nodes.Add(BuildTree(item, false));
                }
            }
            treeView.Items.Refresh();
        }

        public bool IsEmpty
        {
            get { return !nodes.Any(); }
        }

        private static IEnumerable<IHasChildren> CollectNodes(IEnumerable<Node> items)
        {
            foreach (var node in items.Where(x=>x.IsChecked != false))
            {
                foreach (var child in CollectNodes(node.Children))
                {
                    yield return child;
                }
                yield return node.Data;
            }
        }

        private static Node BuildTree(IHasChildren item, bool childrensFinded, params Node[] parents)
        {
            var node = new Node { Data = item , IsExpanded = !childrensFinded};
            var r = item as IRefreshable;
            if (r != null) r.OnRefresh += node.RefreshData;
            foreach (var parent in parents)
            {
                node.Parent.Add(parent);
            }
            foreach (var ch in item.Children)
            {
                node.Children.Add(BuildTree(ch, childrensFinded || item.Children.Count > 1, node));
            }
            return node;
        }

        public static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var currentCheckBox = (CheckBox)sender;
            CheckBoxId.checkBoxId = currentCheckBox.Uid;
        }

        private void ButtonCheckAllClick(object sender, RoutedEventArgs e)
        {
            CheckedTree(nodes, true);
        }

        private void ButtonUncheckAllClick(object sender, RoutedEventArgs e)
        {
            CheckedTree(nodes, false);
        }

        private static void CheckedTree(IEnumerable<Node> items, bool isChecked)
        {
            foreach (var item in items)
            {
                item.IsChecked = isChecked;
                if (item.Children.Count != 0) CheckedTree(item.Children, isChecked);
            }
        }

        private void ButtonExpandClick(object sender, RoutedEventArgs e)
        {
            ExpandTree(nodes, true);
        }

        private void ButtonCollapseClick(object sender, RoutedEventArgs e)
        {
            ExpandTree(nodes, false);
        }

        private static void ExpandTree(IEnumerable<Node> items, bool isExpanded)
        {
            foreach (var item in items)
            {
                item.IsExpanded = isExpanded;
                if (item.Children.Count != 0) ExpandTree(item.Children, isExpanded);
            }
        }
    }
}
