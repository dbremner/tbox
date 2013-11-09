﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Common.Base.Log;
using Common.MT;
using Interface;
using Localization.Plugins.TeamManager;
using Microsoft.Win32;
using PluginsShared.ReportsGenerator;
using ScriptEngine.Core;
using TeamManager.Code;
using TeamManager.Code.Modifiers;
using TeamManager.Code.Reports;
using TeamManager.Code.Reports.Contracts;
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
        private Profile profile;
        private readonly ReportReceiver reportReceiver;
        private FullReport fullReport;
        private IList<SpecialDay> specialDays;
        private string stylesFolder;
        private string validatorsFolder;

        public TimeReportDialog(ReportReceiver reportReceiver)
        {
            this.reportReceiver = reportReceiver;

            InitializeComponent();
            Panel.View = Persons;
            OpPanel.View = Operations;
        }

        public void ShowReportDialog(Profile p, IPluginContext context, IList<SpecialDay> sDays )
        {
            if (!IsVisible)
            {
                fullReport = null;
                specialDays = sDays;
                stylesFolder = Path.Combine(context.DataProvider.ReadOnlyDataPath, "Styles");
                StylesList.ItemsSource = GetFiles(stylesFolder, "*.css");
                validatorsFolder = Path.Combine(context.DataProvider.ReadOnlyDataPath, "Validators");
                Validators.ItemsSource = GetFiles(validatorsFolder, "*.cs");
                DataContext = profile = p;
                PrintReport(fullReport);
            }
            ShowAndActivate();
            ValueChanged(null, null);
        }

        private static string[] GetFiles(string folder, string mask)
        {
            return new DirectoryInfo(folder)
                .GetFiles(mask)
                .Select(x => x.Name)
                .ToArray();
        }

        private void Generate_OnClick(object sender, RoutedEventArgs e)
        {
            SetHtml(string.Empty);
            DialogsCache.ShowProgress(DoGenerate, TeamManagerLang.CalculatingTimeReport, this, false);
        }

        private void DoGenerate(IUpdater u)
        {
            try
            {
                var cfg = profile.Report;
                if(!cfg.DateFrom.HasValue || !cfg.DateTo.HasValue)
                    throw new ArgumentException("Please specify dates");
                var emails = profile.Persons.CheckedItems.Select(x => x.Key).ToList();
                try
                {
                    fullReport = reportReceiver.GetTimeReport(cfg.DateFrom.Value, cfg.DateTo.Value, emails, profile.Operations.CheckedItems.ToArray(), u);
                    u.Update(TeamManagerLang.PrepareReports, 1);
                }
                catch (Exception ex)
                {
                    fullReport = null;
                    log.Write(ex, TeamManagerLang.UnexpectedException);
                }
                Mt.Do(this, () => PrintReport(fullReport)); 
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't receive timer report");
            }
        }

        private void PrintReport(FullReport report)
        {
            var htmlReport = string.Empty;
            try
            {
                if (report != null)
                {
                    var info = BuildInfo(report);
                    htmlReport = PrintReportToHtml(report, info);
                }
            }
            catch (Exception ex)
            {
                report = null;
                htmlReport = string.Empty;
                log.Write(ex, TeamManagerLang.UnexpectedException);
            }
            btnToEmail.IsEnabled = btnRefresh.IsEnabled = 
                btnToExcel.IsEnabled = btnCopy.IsEnabled = 
                    report != null;
            SetHtml(htmlReport);
        }

        private string PrintReportToHtml(FullReport report, IList<ReportPerson> info)
        {
            var result = string.Empty;
            var printer = new HtmlReport(text => result = text,
                                         File.ReadAllText(Path.Combine(stylesFolder, profile.Report.Style)));
            printer.Print(info, report.Time);
            return result;
        }

        private IList<ReportPerson> BuildInfo(FullReport report)
        {
            if(report == null)return new ReportPerson[0];
            report = report.Clone();
            if (profile.Report.AddNotPresentDays)
            {
                new AddNotPresentDaysModier().Modify(report);
            }
            var compiler = new ScriptCompiler<IDayStatusStrategy>();
            var validator = compiler.Compile(File.ReadAllText(Path.Combine(validatorsFolder, profile.Report.DayStatusStrategy)));
            var reportBuilder = new ReportsBuilder(profile.Report.WorkingHoursPerDay, validator, new DayTypeProvider(specialDays));
            var info = reportBuilder.BuildLoggedTimeReport(report);
            if (profile.Report.FilterResultsByErrors)
            {
                info = info.Where(x => x.Days.Any(o => string.Equals( o.Status, DayTypes.Error) )).ToList();
            }
            return info;
        }

        private void SetHtml(string value)
        {
            HtmlReport.NavigateToString(string.IsNullOrEmpty(value) ? " " : value);
        }

        private void ValueChanged(object sender, RoutedEventArgs e)
        {
            btnGenerate.IsEnabled =
                DateFrom.Value.HasValue && DateTo.Value.HasValue &&
                Operations.Items.Count > 0 &&
                Persons.Items.Count > 0 &&
                profile.Persons.CheckedValuesCount > 0 &&
                profile.Operations.CheckedValuesCount > 0;
        }

        private void CopyClick(object sender, RoutedEventArgs e)
        {
            dynamic doc = HtmlReport.Document;
            Clipboard.SetText(doc.documentElement.InnerHtml);
        }

        private void ToExcelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new SaveFileDialog {FileName = profile.Report.ExcelFilePath, Title = TeamManagerLang.ChooseFileToSaveReport, DefaultExt = "*.xlsx"};
                if (dialog.ShowDialog(this) != true) return;
                var xls = new ExcelReport(dialog.FileName);
                xls.Print(BuildInfo(fullReport), fullReport.Time);
                using (Process.Start(new ProcessStartInfo
                    {
                        FileName = dialog.FileName,
                        CreateNoWindow = true,
                        UseShellExecute = true
                    }))
                {
                }
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't export data to excel");
            }
        }

        private void RefreshResults(object sender, RoutedEventArgs e)
        {
            PrintReport(fullReport);
        }
        
        private void ToEmailClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(TeamManagerLang.AreYouWantSendEmails, Title, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            DialogsCache.ShowProgress(DoSendEmails, TeamManagerLang.SendEmail, this);
        }

        private void DoSendEmails(IUpdater u)
        {
            try
            {
                var report = fullReport.Clone();
                var emailSender = new EmailsSender(profile,
                                                   report,
                                                   BuildInfo(report),
                                                   items => PrintReportToHtml(report, items));
                emailSender.Send(u);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't send emails");
            }
        }
    }
}