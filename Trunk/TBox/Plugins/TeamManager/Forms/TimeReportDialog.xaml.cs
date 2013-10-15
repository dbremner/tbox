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
            ValueChanged(null, null);
            ShowAndActivate();
        }

        private void Generate_OnClick(object sender, RoutedEventArgs e)
        {
            Results.Clear();
            DialogsCache.ShowProgress(u=>DoGenerate(), TeamManagerLang.CalculatingTimeReport, this, false);
        }

        private void DoGenerate()
        {
            try
            {
                var cfg = cm.Config.Report;
                if(!cfg.DateFrom.HasValue || !cfg.DateTo.HasValue)
                    throw new ArgumentException("Please specify dates");
                var emails = cm.Config.Persons.CheckedItems.Select(x => x.Key).ToList();
                var report = runner.GetTimeTable(cfg.DateFrom.Value, cfg.DateTo.Value, ref emails);
                report += Environment.NewLine + Environment.NewLine + TeamManagerLang.PeopleIncludedInTheReport +Environment.NewLine + string.Join(";", emails);
                Mt.Do(this, ()=>Results.Value=report); 
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't receive timer report");
            }
        }

        private void ValueChanged(object sender, RoutedEventArgs e)
        {
            btnGenerate.IsEnabled =
                DateFrom.Value.HasValue && DateTo.Value.HasValue &&
                Persons.Items.Count > 0;
        }
    }
}
