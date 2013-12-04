using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ParallelNUnit.Core;

namespace NUnitRunner.Components
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

        public void SetItems(IEnumerable<IHasChildren> items)
        {
            nodes.Clear();
            if (items != null)
            {
                foreach (var item in items)
                {
                    nodes.Add(BuildTree(item));
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
                node.Data.Children.Clear();
                foreach (var child in CollectNodes(node.Children))
                {
                    node.Data.Children.Add(child);
                }
                yield return node.Data;
            }
        }

        private static Node BuildTree(IHasChildren item, params Node[] parents)
        {
            var node = new Node { Data = item };
            foreach (var parent in parents)
            {
                node.Parent.Add(parent);
            }
            foreach (var ch in item.Children)
            {
                node.Children.Add(BuildTree(ch, node));
            }
            return node;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
        /* so slow :(
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
         * */
    }
}
