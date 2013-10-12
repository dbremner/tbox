using System;
using System.Linq;
using System.Windows;
using Common.Base.Log;
using Interface;
using Localization.Plugins.TeamManager;
using TeamManager.Code;
using TeamManager.Code.Settings;
using WPFControls.Code.OS;
using WPFControls.Dialogs;

namespace TeamManager.Forms
{
    /// <summary>
    /// Interaction logic for TimeReportDialog.xaml
    /// </summary>
    public partial class TimeReportDialog
    {
        private readonly ILog log = LogManager.GetLogger<ILog>();
        private readonly IConfigManager<Config> cm;
        private readonly Runner runner;

        public TimeReportDialog(IConfigManager<Config> cm, Runner runner)
        {
            this.cm = cm;
            this.runner = runner;

            InitializeComponent();
            Panel.View = Persons;
        }

        public void ShowReportDialog()
        {
            DataContext = cm.Config.Report;
            Persons.ItemsSource = cm.Config.Persons;
            base.SafeShowDialog();
        }

        private void Generate_OnClick(object sender, RoutedEventArgs e)
        {
            DialogsCache.ShowProgress(u=>DoGenerate(), TeamManagerLang.CalculatingTimeReport, this, false);
        }

        private void DoGenerate()
        {
            try
            {
                var cfg = cm.Config.Report;
                var report = runner.GetTimeTable(cfg.DateFrom, cfg.DateTo, cm.Config.Persons.CheckedItems.Select(x => x.Key).ToArray());
                Mt.Do(this, ()=>Results.Value=report); 
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't receive timer report");
            }
        }
    }
}
