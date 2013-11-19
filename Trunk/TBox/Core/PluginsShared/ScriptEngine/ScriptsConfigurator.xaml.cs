﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Common.Base.Log;
using Common.MT;
using Interface;
using Localization.PluginsShared;
using ScriptEngine;
using ScriptEngine.Core;
using ScriptEngine.Core.Params;
using WPFControls.Code.OS;
using WPFControls.Dialogs;

namespace PluginsShared.ScriptEngine
{
	/// <summary>
	/// Interaction logic for ScriptsConfigurator.xaml
	/// </summary>
	public abstract partial class ScriptsConfigurator
	{
        protected static readonly ILog Log = LogManager.GetLogger<ScriptsConfigurator>();
		protected Operation Config;
	    private IScriptConfigurator configurator;
		protected IList<ScriptPackage> ScriptsPackages;
		private readonly ParametersMerger parametersMerger = new ParametersMerger(); 
		public IPluginContext Context { get; set; }
		protected ScriptsConfigurator()
		{
			InitializeComponent();
		    btnAction.Content = PluginsSharedLang.Configure;
		}

        public void ShowDialog(Operation o, IScriptConfigurator c, Window owner)
		{
			Owner = null;
			DataContext = Config = o;
		    configurator = c;
			ShowAndActivate();
			ReloadClick(null, null);
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ActionClick(object sender, RoutedEventArgs e)
		{
			var invalidKeys = string.Join(Environment.NewLine, ParametersValidator.Validate(Config.Parameters));
			if (!string.IsNullOrEmpty(invalidKeys))
			{
				MessageBox.Show(PluginsSharedLang.CantRunInvalidParams + Environment.NewLine + invalidKeys, 
					Title, MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
		    DoAction();
		}

        protected virtual void DoAction()
        {
            Hide();
        }

	    protected abstract IEnumerable<string> GetPathes();

		private void ReloadClick(object sender, RoutedEventArgs e)
		{
			Parameters.ItemsSource = null;
			DialogsCache.ShowProgress(DoReload, PluginsSharedLang.Reloading, this);
		}

		private void DoReload(IUpdater obj)
		{
			try
			{
                ScriptsPackages = GetPathes().Select(x=>configurator.GetParameters(File.ReadAllText(x))).ToArray();
				IList<Parameter> parameters = new List<Parameter>();
				parameters = ScriptsPackages.Aggregate(parameters, (current, x) => parametersMerger.Merge(current, x.Parameters));
				Mt.Do(this, () =>
				{
					Config.Parameters = new ObservableCollection<Parameter>(
						parametersMerger.Clarify(Config.Parameters, parameters));
					Parameters.ItemsSource = Config.Parameters;
				});
				return;
			}
			catch (CompilerExceptions cex)
			{
				MessageBox.Show(cex.ToString(), Config.Key, MessageBoxButton.OK, MessageBoxImage.Stop);
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't initialize script: " + Config.Key);
			}
			Mt.Do(this, Hide);
		}
	}
}
