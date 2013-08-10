using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using Interface;
using Interface.Atrributes;
using WPFControls.Dialogs;
using WPFWinForms;

namespace TextGenerator
{
	[PluginName("Text generator")]
	[PluginDescription("Small tool to generate text. Also it help when you export tables to excel or onenote.")]
	public sealed class TextGenerator : ConfigurablePlugin<Settings, Config>
	{
		public TextGenerator()
		{
			Menu = new []{
				new UMenuItem{Header = "Replace chars sequenses by tabs", OnClick = o=>PrepareTable()},
				new UMenuItem{Header = "Generate Text form clipboard", OnClick = o=>GenerateText(Clipboard.GetText())},
				new UMenuItem{Header = "Generate Text", OnClick = o=>GenerateText(string.Empty)},
				new UMenuItem{Header = "Generate Guid", OnClick = o=>GenerateGuid()},
				new UMenuItem{Header = "Calc Clipboard text length", OnClick = o=>CalcClipboardTextLength()},
			};
		}

		private static void CalcClipboardTextLength()
		{
			MessageBox.Show("Length of the clipboard text = " + Clipboard.GetText().Length.ToString(), "Text generator",
			                MessageBoxButton.OK, MessageBoxImage.Information);
		}

		private static void GenerateGuid()
		{
			Clipboard.SetText(Guid.NewGuid().ToString());
		}

		private void GenerateText(string text)
		{
			var tmp = 0;
			var value = DialogsCache.ShowInputBox("Please specify text length", "Generate text", Config.TextLength.ToString(CultureInfo.InvariantCulture), (x)=>int.TryParse(x, out tmp), Application.Current.MainWindow);
			if(!value.Key)return;

			Config.TextLength = int.Parse(value.Value);
			var sb = new StringBuilder(Config.TextLength);
			if (string.IsNullOrEmpty(text)) 
				text = "Sample default text. ";
			while (sb.Length < Config.TextLength) sb.Append(text);
			Clipboard.SetText(sb.ToString().Substring(0, Config.TextLength));
		}

		private void PrepareTable()
		{
			var text = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(text)) return;
			var lines = text.Split(
				new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var result = new StringBuilder();
			foreach (var words in 
				lines.Select(line => line.Split(new[] {Config.Fill}, StringSplitOptions.RemoveEmptyEntries)))
			{
				foreach (var word in words)
				{
					result.Append(word).Append('\t');
				}
				result.Append(Environment.NewLine);
			}
			Clipboard.SetText(result.ToString());
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = context.GetSystemIcon(96);
		}
	}
}
