using System;
using System.Globalization;
using System.Windows;

namespace Mnk.Library.WpfControls.Code.Dialogs
{
	public abstract class BaseDialog
	{
	    private readonly Func<Window> ownerGetter; 
		protected Func<string, bool> Validator { get; set; }
		protected string Caption { get; private set; }
		protected Templates Buttons { get; private set; }
        protected Window Owner { get { return ownerGetter() ?? Application.Current.MainWindow; } }

        protected BaseDialog(string caption, Templates templates, Func<string, bool> validator, Func<Window> ownerGetter)
		{
			Caption = caption;
			Buttons = templates;
			Validator = validator;
            this.ownerGetter = ownerGetter;
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
			return Edit(name, out newName);
		}

		public virtual bool Del(string[] names)
		{
			return MessageBox.Show(string.Format(CultureInfo.InvariantCulture, Buttons.Del, string.Join(Environment.NewLine, names)), Caption,
				MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
		}

		public virtual bool Clear()
		{
			return MessageBox.Show(Buttons.Clear, Caption,
				MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
		}
	}
}
