using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Common.MT;
using WPFControls.Code.OS;

namespace WPFControls.Components.Updater
{
	/// <summary>
	/// Interaction logic for SimpleProgress.xaml
	/// </summary>
	public partial class SimpleProgress : UserControl
	{
		protected object Locker { get; private set; }
		protected bool UserPressClose { get; set; }
		public double Value { get { return pbValue.Value; } }

		public SimpleProgress()
		{
			Locker = new object();
			InitializeComponent();
		}

		public virtual void Reset()
		{
			pbValue.Value = 0;
			pbValue.IsIndeterminate = true;
		}

		internal bool IsUserPressClose()
		{
			lock ( Locker )
			{
				return UserPressClose;
			}
		}
		
		protected virtual IUpdater CreateUpdater()
		{
			return new SimpleUpdater<SimpleProgress>(this);
		}

		public void Start(Action<IUpdater> func, Action endAction=null)
		{
			UserPressClose = false;
			var u = CreateUpdater();
			ThreadPool.QueueUserWorkItem(o=>
				{
					try
					{
						func(u);
					}
					finally
					{
						if (endAction != null) endAction();
						Mt.SetEnabled(Button, true);
					}
				});
		}

		internal void Button_Click( object sender, RoutedEventArgs e )
		{
			lock ( Locker )
			{
				UserPressClose = true;
			}
			Button.IsEnabled = false;
		}

	}
}
