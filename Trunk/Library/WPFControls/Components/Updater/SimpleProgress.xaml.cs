using System;
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
		private Action onEnd;

		public SimpleProgress()
		{
			Locker = new object();
			InitializeComponent();
		}

		public virtual void Reset()
		{
			pbValue.Value = 0;
			pbValue.IsIndeterminate = true;
			ProgressModel = new ProgressModeMultithreaded();
		}

		public IProgressModel ProgressModel { get; set; }

		public double Value { get { return pbValue.Value; } }

		internal bool IsUserPressClose()
		{
			lock ( Locker )
			{
				return UserPressClose;
			}
		}
		
		protected virtual IUpdater CreateUpdater(Action action)
		{
			return new SimpleUpdater<SimpleProgress>(this, action);
		}

		public void Start(Action<IUpdater> func, Action endAction=null)
		{
			UserPressClose = false;
			onEnd = endAction;
			ProgressModel.Start(
				CreateUpdater(ProgressModel.DoEvents), func, OnProgressEnd);
		}

		private void OnProgressEnd()
		{
			if ( onEnd != null ) onEnd();
			Mt.SetEnabled(Button, true);
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
