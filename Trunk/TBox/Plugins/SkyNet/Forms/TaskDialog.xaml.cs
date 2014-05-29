using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Mnk.Library.Common;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.MT;
using Mnk.Library.ScriptEngine;
using Mnk.Library.WpfControls;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Plugins.SkyNet.Forms
{
    /// <summary>
    /// Interaction logic for TaskDialog.xaml
    /// </summary>
    public partial class TaskDialog
    {
        private readonly ITaskExecutor taskExecutor;
        private readonly IServicesFacade servicesFacade;
        private readonly IScriptsHelper scriptsHelper;
        private readonly DispatcherTimer timer;
        private readonly ILog log = LogManager.GetLogger<TaskDialog>();

        public TaskDialog(ITaskExecutor taskExecutor, IServicesFacade servicesFacade, IPluginContext context, IScriptsHelper scriptsHelper)
        {
            this.taskExecutor = taskExecutor;
            this.servicesFacade = servicesFacade;
            this.scriptsHelper = scriptsHelper;
            InitializeComponent();
            timer = new DispatcherTimer{Interval = new TimeSpan(0,0,0,5)};
            timer.Tick += (o,e)=>Refresh();
        }

        private void Refresh()
        {
            try
            {
                var tasks = servicesFacade.GetServiceTasks();
                ExistTasks.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't retrieve tasks state. Please, verify is server service started and you have correctly configured firewall.");
                Close();
            }
        }

        public void ShowDialog(SingleFileOperation operation)
        {
            if (!IsVisible)
            {
                timer.Start();
                DataContext = operation;
            }
            ShowAndActivate();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            timer.Stop();
            base.OnClosing(e);
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            ExceptionsHelper.HandleException(
                () => taskExecutor.Execute((SingleFileOperation) DataContext),
                ()=>"Can't start task", log
                );
        }

        private void CancelTask(object sender, RoutedEventArgs e)
        {
            var task = GetServerTask(sender);
            ExceptionsHelper.HandleException(
                () => servicesFacade.Cancel(task.Id),
                () => "Can't cancel task", log
                );
        }

        private void TerminateTask(object sender, RoutedEventArgs e)
        {
            var task = GetServerTask(sender);
            ExceptionsHelper.HandleException(
                () => servicesFacade.Terminate(task.Id),
                () => "Can't terminate task", log
                );
        }

        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            var task = GetServerTask(sender);
            ExceptionsHelper.HandleException(
                () => servicesFacade.DeleteTask(task.Id),
                () => "Can't delete task", log
                );
        }

        private static ServerTask GetServerTask(object sender)
        {
            return (ServerTask) ((Button) sender).DataContext;
        }

        private void ConfigureScriptClick(object sender, RoutedEventArgs e)
        {
            scriptsHelper.ShowParameters((SingleFileOperation)DataContext);
        }

    }
}
