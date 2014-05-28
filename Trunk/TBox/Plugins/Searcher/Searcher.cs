using System;
using System.Windows;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Locales.Localization.Plugins.Searcher;
using Mnk.TBox.Plugins.Searcher.Code;
using Mnk.TBox.Plugins.Searcher.Code.Settings;
using Mnk.Library.WpfSyntaxHighlighter;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;

namespace Mnk.TBox.Plugins.Searcher
{
	[PluginInfo(typeof(SearcherLang), typeof(Properties.Resources), PluginGroup.Desktop)]
	public sealed class Searcher : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private AvailabilityChecker availabilityChecker;
		private readonly Lazy<Worker> worker;

		public Searcher()
		{
			worker = new Lazy<Worker>(() =>
			{
				var w = new Worker(availabilityChecker, Context.DoSync, Icon.ToImageSource());
				w.Fill(Config);
				w.InitFolders(Context.DataProvider.WritebleDataPath);
				return w;
			});
		}

		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			Menu = new[]
				       {
					       new UMenuItem
						       {
								   IsEnabled = availabilityChecker.CanSearch,
							       Header = SearcherLang.Search, 
								   OnClick = DoSearch
						       },
 						   new USeparator(), 
						   new UMenuItem
							   {
								   IsEnabled = availabilityChecker.CanUnload,
								   Header = SearcherLang.UnloadIndexes, 
								   OnClick = DoUnload
							   },
						   new UMenuItem
							   {
								   IsEnabled = availabilityChecker.CanRebuild,
								   Header = SearcherLang.RebuildIndexes, 
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
                MessageBox.Show(SearcherLang.AreYouWantRebuild, SearcherLang.PluginName, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
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