﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.Model;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Locales.Localization.Plugins.Templates;
using Mnk.TBox.Plugins.Templates.Code.Settings;
using Mnk.TBox.Plugins.Templates.Components;
using Mnk.Library.WpfControls.Code;
using Mnk.Library.WpfControls.Dialogs.StateSaver;
using Mnk.Library.WpfSyntaxHighlighter;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;

namespace Mnk.TBox.Plugins.Templates
{
	[PluginInfo(typeof(TemplatesLang), typeof(Properties.Resources), PluginGroup.Development)]
	public sealed class Templates : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private readonly LazyDialog<FileDialog> fileDialog;
		private readonly LazyDialog<StringDialog> stringDialog;

		public Templates()
		{
			fileDialog = new LazyDialog<FileDialog>(CreateDialog<FileDialog>, "templates");
			stringDialog = new LazyDialog<StringDialog>(CreateDialog<StringDialog>, "strings");
		}

		private T CreateDialog<T>()
			where T : Window, new()
		{
			return new T{Icon = Icon.ToImageSource()};
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
		}

		protected override void OnConfigUpdated()
		{
 			base.OnConfigUpdated();
			Menu = 
				new []{new UMenuItem{Header = TemplatesLang.FilesAndFolders, IsEnabled = false}}
				.Concat(EnumerateDirectories())
                .Concat(new[] { new USeparator(), new UMenuItem { Header = TemplatesLang.Strings, IsEnabled = false } })
				.Concat(EnumerateStrings())
				.ToArray();
		}

		private IEnumerable<UMenuItem> EnumerateStrings()
		{
			return Config.StringTemplates
				.Select(x => new UMenuItem{
						Header = x.Key,
						OnClick = o=>ProcessString(x)
					});
		}

		private IEnumerable<UMenuItem> EnumerateDirectories()
		{
			if (!Directory.Exists(Context.DataProvider.ReadOnlyDataPath))return new UMenuItem[0];
			return Directory.EnumerateDirectories(Context.DataProvider.ReadOnlyDataPath)
				.Select(path => new UMenuItem
				{
					Header = Path.GetFileName(path),
					OnClick = o => ProcessFolder(path)
				})
				.ToArray();
		}

		private void ProcessString(Template t)
		{
			PrepareValues(t.KnownValues);
			stringDialog.Do(Context.DoSync, d => d.ShowDialog(t, Config.ItemTemplate), Config.States);
		}

		private void ProcessFolder(string path)
		{
			var name = Path.GetFileName(path);
			var item = Config.FileTemplates.FirstOrDefault(x => x.Key.EqualsIgnoreCase(name));
			if (item == null)
			{
				item = new Template{Key = name};
				Config.FileTemplates.Add(item);
			}
			PrepareValues(item.KnownValues);
			fileDialog.Do(Context.DoSync, d => d.ShowDialog(item, path, Config.ItemTemplate), Config.States);
		}

		private void PrepareValues(IEnumerable<PairData> values)
		{
			foreach (var data in values)
			{
				var exist = Config.KnownValues.FirstOrDefault(x => x.Key.EqualsIgnoreCase(data.Key));
				if (exist != null)
				{
					data.Value = exist.Value;
				}
			}
		}

		public override void Save(bool autoSaveOnExit)
		{
			base.Save(autoSaveOnExit);
			if (!autoSaveOnExit) return;
			fileDialog.SaveState(Config.States);
			stringDialog.SaveState(Config.States);
		}

		public void Dispose()
		{
			fileDialog.Dispose();
			stringDialog.Dispose();
		}
	}
}
