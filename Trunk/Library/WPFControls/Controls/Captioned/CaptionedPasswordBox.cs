using System.Windows;
using System.Windows.Controls;
using WPFControls.Tools;

namespace WPFControls.Controls.Captioned
{
	public sealed class CaptionedPasswordBox : CaptionedControl
	{
        private readonly PasswordBox child = new PasswordBox{Margin = new Thickness(0)};

        public CaptionedPasswordBox()
		{
			child.PasswordChanged += OnValueChanged;
            child.PasswordChanged += (o, e) => SetValue(ValueProperty, SecurePassword);
			Panel.Children.Add(child);
		}

		public static readonly DependencyProperty ValueProperty =
            DpHelper.Create<CaptionedPasswordBox, string>("Value", (s, v) => s.Value = v);
		public string Value
		{
            get
            {
                return (string)GetValue(ValueProperty);
            }
			set
			{
				SetValue(ValueProperty, value);
                using (var s = value.DecryptString())
                {
                    var pswd = s.ToInsecureString();
                    if (!string.Equals(pswd, child.Password))
                    {
                        child.Password = pswd;
                    }
                }
			}
		}

	    private string SecurePassword
	    {
	        get { return child.SecurePassword.EncryptString(); }
	    }

	    public new void Focus()
		{
			child.Focus();
		}
	}
}
