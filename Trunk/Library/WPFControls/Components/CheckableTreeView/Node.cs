using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Mnk.Library.Common.Models;

namespace Mnk.Library.WpfControls.Components.CheckableTreeView
{

    public class Node : INotifyPropertyChanged
    {
        public Node()
        {
            Id = Guid.NewGuid().ToString();
        }

        private readonly ObservableCollection<Node> children = new ObservableCollection<Node>();
        private readonly ObservableCollection<Node> parent = new ObservableCollection<Node>();
        private IHasChildren data;
        private bool? isChecked = true;
        private bool isExpanded;

        public ObservableCollection<Node> Children
        {
            get { return children; }
        }

        public ObservableCollection<Node> Parent
        {
            get { return parent; }
        }

        public bool? IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }

        public IHasChildren Data
        {
            get { return data; }
            set
            {
                data = value;
                RefreshData();
            }
        }

        public void RefreshData()
        {
            RaisePropertyChanged("Data");
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }

        public string Id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            const int countCheck = 0;
            if (propertyName != "IsChecked") return;
            if (Id == CheckBoxId.checkBoxId && Parent.Count == 0 && Children.Count != 0)
            {
                CheckedTreeParent(Children, IsChecked);
            }
            if (Id == CheckBoxId.checkBoxId && Parent.Count > 0 && Children.Count > 0)
            {
                CheckedTreeChildMiddle(Parent, Children, IsChecked);
            }
            if (Id == CheckBoxId.checkBoxId && Parent.Count > 0 && Children.Count == 0)
            {
                CheckedTreeChild(Parent, countCheck);
            }
        }

        private static void CheckedTreeChildMiddle(IEnumerable<Node> itemsParent, IEnumerable<Node> itemsChild, bool? isChecked)
        {
            CheckedTreeParent(itemsChild, isChecked);
            CheckedTreeChild(itemsParent, 0);
        }

        private static void CheckedTreeParent(IEnumerable<Node> items, bool? isChecked)
        {
            foreach (var item in items)
            {
                item.IsChecked = isChecked;
                if (item.Children.Count != 0) CheckedTreeParent(item.Children, isChecked);
            }
        }

        private static void CheckedTreeChild(IEnumerable<Node> items, int countCheck)
        {
            var isNull = false;
            foreach (var paren in items)
            {
                foreach (Node child in paren.Children)
                {
                    if (child.IsChecked == true || child.IsChecked == null)
                    {
                        countCheck++;
                        if (child.IsChecked == null)
                            isNull = true;
                    }
                }
                if (countCheck != paren.Children.Count && countCheck != 0) paren.IsChecked = null;
                else if (countCheck == 0) paren.IsChecked = false;
                else if (countCheck == paren.Children.Count && isNull) paren.IsChecked = null;
                else if (countCheck == paren.Children.Count && !isNull) paren.IsChecked = true;
                if (paren.Parent.Count != 0) CheckedTreeChild(paren.Parent, 0);
            }
        }
    }

    public struct CheckBoxId
    {
        public static string checkBoxId;
    }
}
