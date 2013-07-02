using System;
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
			};
		}

		private static void GenerateGuid()
		{
			Clipboard.SetText(Guid.NewGuid().ToString());
		}

		private static void GenerateText(string text)
		{
			int tmp;
			var value = DialogsCache.ShowInputBox("Please specify text length", "Generate text", "64", (x)=>int.TryParse(x, out tmp), Application.Current.MainWindow);
			if(!value.Key)return;
			var count = int.Parse(value.Value);
			var sb = new StringBuilder(count);
			if (string.IsNullOrEmpty(text)) 
				text = "Sample default text. ";
			while (sb.Length < count) sb.Append(text);
			Clipboard.SetText(sb.ToString().Substring(0, count));
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
