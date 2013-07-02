using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Common.Base.Log;
using Common.MT;
using Interface;
using ScriptEngine;
using ScriptEngine.Core;
using ScriptEngine.Core.Params;
using WPFControls.Code.OS;
using WPFControls.Dialogs;

namespace Automator.Components
{
	/// <summary>
	/// Interaction logic for ScriptsRunner.xaml
	/// </summary>
	public partial class ScriptsRunner
	{
		private static readonly ILog Log = LogManager.GetLogger<ScriptsRunner>();
		private Operation config;
		private IList<ExecutionContext> executionContexts;
		private readonly ParametersMerger parametersMerger = new ParametersMerger(); 
		public static Window Instance { get; set; }
		public IPluginContext Context { get; set; }
		public ScriptsRunner()
		{
			Instance = this;
			InitializeComponent();
		}

		public void ShowDialog(Operation o)
		{
			Owner = null;
			DataContext = config = o;
			ShowAndActivate();
			ReloadClick(null, null);
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void RunClick(object sender, RoutedEventArgs e)
		{
			var invalidKeys = string.Join(Environment.NewLine, ParametersValidator.Validate(config.Parameters));
			if (!string.IsNullOrEmpty(invalidKeys))
			{
				MessageBox.Show("Can't run, because next parameters are invalid:" + Environment.NewLine + invalidKeys, 
					Title, MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
			Hide();
			DialogsCache.ShowProgress(u => Work(u, GetParameters() ), 
					"Execute: " + Title, topmost:false, showInTaskBar:true);
		}

		private IList<Parameter> GetParameters()
		{
			return config.Parameters;
		}

		private string[] GetPathes()
		{
			return config.Pathes
				.CheckedItems
				.Select(x => Path.Combine(Context.DataProvider.DataPath, x.Key))
				.ToArray();
		}

		private void Work(IUpdater u, IList<Parameter> parameters)
		{
			try
			{
				DoWork(u, parameters);
			}
			catch (CompilerExceptions cex)
			{
				Log.Write(cex.ToString());
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Error running script");
			}
		}

		private void DoWork(IUpdater u, IList<Parameter> parameters)
		{
			var i = 0;
			foreach (var context in executionContexts)
			{
				if (u.UserPressClose) return;
				u.Update(i++ / (float)executionContexts.Count);
				context.Execute(parameters);
			}
		}

		private void Invoke(Action a)
		{
			Exception lastEx = null;
			Dispatcher.Invoke(new Action(() =>
				{
					try
					{
						a();
					}
					catch (Exception ex)
					{
						lastEx = ex;
					}
				}));
			if (lastEx != null) throw lastEx;
		}

		private void ReloadClick(object sender, RoutedEventArgs e)
		{
			Parameters.ItemsSource = null;
			DialogsCache.ShowProgress(DoReload, "Reloading...", this);
		}

		private void DoReload(IUpdater obj)
		{
			var compiler = new ScriptCompiler();
			try
			{
				executionContexts = GetPathes().Select(x => compiler.GetExecutionContext(File.ReadAllText(x), Invoke)).ToArray();
				IList<Parameter> parameters = new List<Parameter>();
				parameters = executionContexts.Aggregate(parameters, (current, x) => parametersMerger.Merge(current, x.Parameters));
				config.Parameters = new ObservableCollection<Parameter>(
					parametersMerger.Clarify(config.Parameters, parameters));
				Mt.Do(this, ()=> Parameters.ItemsSource = config.Parameters);
				return;
			}
			catch (CompilerExceptions cex)
			{
				MessageBox.Show(cex.ToString(), config.Key, MessageBoxButton.OK, MessageBoxImage.Stop);
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't initialize script: " + config.Key);
			}
			Mt.Do(this, Hide);
		}
	}
}
