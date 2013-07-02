using System.Collections.ObjectModel;
using System.Windows;
using Common.Base;
using Common.Base.Log;
using Common.UI.Model;
using PluginsShared.Templates;
using Templates.Code.Settings;
using WPFControls.Controls;

namespace Templates.Components
{
	/// <summary>
	/// Interaction logic for FileDialog.xaml
	/// </summary>
	public partial class FileDialog
	{
		private static readonly ILog Log = LogManager.GetLogger<FileDialog>();
		private TemplatesWorker worker = null;
		private Template template;
		private string sourcePath;
		public FileDialog()
		{
			InitializeComponent();
			TargetPath.PathGetter = new FolderPathGetter();
		}

		public void ShowDialog(Template t, string source, string itemTemplate)
		{
			sourcePath = source;
			if(ExceptionsHelper.HandleException(
				() => Prepare(t, itemTemplate),
				()=>"Can't prepare templates", Log))
			{
				Hide();
			}
		}

		private void Prepare(Template t, string itemTemplate)
		{
			template = t;
			Title = "Templates - [" + t.Key + "]";
			worker = new TemplatesWorker(itemTemplate);
			Options.ItemsSource = 
				template.KnownValues = 
					new ObservableCollection<PairData>(worker.GetValues(sourcePath, t.KnownValues));
			TargetPath.Value = template.Value;
			TargetPathValueChanged(null, null);
			ShowAndActivate();
		}

		private void FillTemplateClick(object sender, RoutedEventArgs e)
		{
			ExceptionsHelper.HandleException(
				FillTemplates, 
				()=>"Can't fill templates",
				Log
				);
		}

		private void FillTemplates()
		{
			worker.Copy(sourcePath, template.Value = TargetPath.Value, template.KnownValues);
			Hide();
		}

		private void TargetPathValueChanged(object sender, RoutedEventArgs e)
		{
			FillButton.IsEnabled = !string.IsNullOrEmpty(TargetPath.Value);
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
