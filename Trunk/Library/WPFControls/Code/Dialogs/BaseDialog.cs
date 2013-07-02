using System;
using System.Windows;

namespace WPFControls.Code.Dialogs
{
	public abstract class BaseDialog
	{
		protected Func<string, bool> Validator { get; set; }
		protected string Caption { get; private set; }
		protected Templates Buttons { get; private set; }
		protected Window Owner { get; private set; }

		protected BaseDialog(string caption, Templates templates, Func<string, bool> validator, Window owner)
		{
			Caption = caption;
			Buttons = templates;
			Validator = validator;
			Owner = owner??Application.Current.MainWindow;
		}
		public bool IsAddVisible { get { return !string.IsNullOrEmpty(Buttons.Add); } }
		public bool IsClearVisible { get { return !string.IsNullOrEmpty(Buttons.Clear); } }
		public bool IsCloneVisible { get { return !string.IsNullOrEmpty(Buttons.Clone); } }
		public bool IsDelVisible { get { return !string.IsNullOrEmpty(Buttons.Del); } }
		public bool IsEditVisible { get { return !string.IsNullOrEmpty(Buttons.Edit); } }

		public abstract bool Add(out string[] newNames);

		public abstract bool Edit(string name, out string newName);

		public virtual bool Clone(string name, out string newName)
		{
			var id = 0;
			while (!Validator(name + ++id)){}
			return Edit(name + id, out newName);
		}

		public virtual bool Del(string name)
		{
			return MessageBox.Show(string.Format(Buttons.Del, name), Caption,
				MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
		}

		public virtual bool Clear()
		{
			return MessageBox.Show(Buttons.Clear, Caption,
				MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
		}
	}
}
