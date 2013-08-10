using System;
using System.Windows;
using Interface;
using Interface.Atrributes;
using Searcher.Code;
using Searcher.Code.Settings;
using WPFSyntaxHighlighter;
using WPFWinForms;

namespace Searcher
{
	[PluginName("Searcher")]
	[PluginDescription("Ability to search in the big set of files for words and file names.")]
	public sealed class Searcher : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private AvailabilityChecker availabilityChecker;
		private readonly Lazy<Worker> worker;

		public Searcher()
		{
			worker = new Lazy<Worker>(() =>
			{
				var w = new Worker(availabilityChecker, Context.DoSync);
				w.Fill(Config);
				w.InitFolders(Context.DataProvider.WritebleDataPath);
				return w;
			});
			Icon = Properties.Resources.Icon;
		}

		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			Menu = new[]
				       {
					       new UMenuItem
						       {
								   IsEnabled = availabilityChecker.CanSearch,
							       Header = "Search...", 
								   OnClick = DoSearch
						       },
 						   new USeparator(), 
						   new UMenuItem
							   {
								   IsEnabled = availabilityChecker.CanUnload,
								   Header = "Unload indexes..", 
								   OnClick = DoUnload
							   },
						   new UMenuItem
							   {
								   IsEnabled = availabilityChecker.CanRebuild,
								   Header = "Rebuild indexes...", 
								   OnClick = DoRebuild
							   }, 
				       };
		}

		public override void Init(IPluginContext context)
		{
			availabilityChecker = new AvailabilityChecker(context.RebuildMenu);
			base.Init(context);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
		}

		public override void Load()
		{
			base.Load();
			if(worker.IsValueCreated)worker.Value.Fill(Config);
		}

		public override void Save(bool autoSaveOnExit)
		{
			base.Save(autoSaveOnExit);
			if (worker.IsValueCreated) worker.Value.Save(Config, autoSaveOnExit);
		}

		private void DoUnload(object o)
		{
			worker.Value.UnloadIndexes();
		}

		private void DoRebuild(object o)
		{
			if (o is NonUserRunContext || 
				MessageBox.Show("Are you realy want rebuild all indexes?", "Searcher", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
			{
				worker.Value.RebuildIndexes();
			}
		}

		private void DoSearch(object o)
		{
			worker.Value.ShowSearchDialog();
		}

		public void Dispose()
		{
			if (worker.IsValueCreated)
				worker.Value.Dispose();
		}
	}

}