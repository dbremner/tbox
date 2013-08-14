using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows;
using Common.Encoders;
using Encoder.Code;
using Encoder.Components;
using Interface;
using Interface.Atrributes;
using PluginsShared.Encoders;
using WPFControls.Dialogs.StateSaver;
using WPFSyntaxHighlighter;
using WPFWinForms;

namespace Encoder
{
	[PluginName("Encoder")]
	[PluginDescription("You can easy encode/decode strings and change the formatting\nfor easier reading.")]
	public sealed class Encoder : SingleDialogPlugin<Config, Dialog>
	{
		private readonly Operation[] operations;
		public Encoder() : base("Show")
		{
			operations = GetOperations().ToArray();
			Menu = CreateMenu(operations).ToArray();
		}

		protected  override Dialog CreateDialog()
		{
			var dialog = base.CreateDialog();
			dialog.Init(operations);
			return dialog;
		}

		private static IEnumerable<Operation> GetOperations()
		{
			yield return new Operation { Header = "Encode c string", Work = x => CommonOps.EncodeString(x) };
			yield return new Operation { Header = "Decode c string", Work = x => CommonOps.DecodeString(x) };
			yield return new Operation { Header = "Encode Uri", Work = x => HttpUtility.UrlEncode(x) };
			yield return new Operation { Header = "Decode Uri", Work = x => HttpUtility.UrlDecode(x) };
			yield return new Operation { Header = "Encode Html", Work = x => HttpUtility.HtmlEncode(x)};
			yield return new Operation { Header = "Decode Html", Work = x => HttpUtility.HtmlDecode(x) };
			yield return new Operation { Header = "Encode to base64", Work = x => Base64Encode.EncodeTo64(x) };
			yield return new Operation { Header = "Decode from base64", Work = x => Base64Encode.DecodeFrom64(x) };
			yield return new Operation { Header = "Format Xml", Work = x => XmlHelper.Format(x), Format = "xml" };
			yield return new Operation { Header = "Format Fql Simple", Work = x => FqlParser.ParseSimple(x), Format = "mssql" };
			yield return new Operation { Header = "Format Fql Advanced", Work = x => FqlParser.ParseWithSubItems(x), Format = "mssql" };
			yield return new Operation { Header = "Format SQL", Work = x => new SqlParser().Parse(x), Format = "mssql" };
			yield return new Operation { Header = "Format Html", Work = x => new HtmlParser().Parse(x), Format = "html" };
			yield return new Operation { Header = "Format JSON", Work = x => new JsonParser().Format(x), Format = "js" };
			yield return new Operation { Header = "Format c-like code (JS,CSS)", Work = x => new CCodeFormatter().Format(x), Format = "js" };
			yield return new Operation { Header = "Minimize to line", Work = x => CommonOps.Minimize(x) };
		}

		private IEnumerable<UMenuItem> CreateMenu(IEnumerable<Operation> ops)
		{
			yield return new UMenuItem 
			{ 
				Header = "Show...", 
				OnClick = o => Dialog.Do(Context.DoSync, d=>d.ShowAndActivate(), Config.States)
			};
			foreach (var o in ops)
			{
				var tmp = o;
				var header = o.Header;
				yield return new UMenuItem { Header = o.Header + "...", OnClick = x => Work(tmp, header) };
			}
		}

		private void Work(Operation o, string title)
		{
			Dialog.Do(Context.DoSync, 
				d => d.ShowDialog("Encoder", 
					Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty, 
					title, o.Format),
					Config.States
					);
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = Context.GetSystemIcon(144);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
		}
	}
}
