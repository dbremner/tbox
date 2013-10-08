using System;
using System.IO;
using System.Windows;
using Common.MT;
using Localization.Plugins.Searcher;
using Searcher.Code.Settings;
using Searcher.Components;
using WPFControls.Code;
using WPFControls.Code.OS;
using WPFControls.Dialogs;
using WPFControls.Dialogs.StateSaver;

namespace Searcher.Code
{
	sealed class Worker : IDisposable
	{
		private readonly AvailabilityChecker availabilityChecker;
		private readonly Action<Action> synchronizer;
		private string folderPath;
		private readonly LazyDialog<SearchDialog> searchDialog;
		private Config mainConfig;
		private SearchEngine engine;

		public Worker(AvailabilityChecker availabilityChecker, Action<Action> synchronizer)
		{
			this.availabilityChecker = availabilityChecker;
			this.synchronizer = synchronizer;
			searchDialog = new LazyDialog<SearchDialog>(() =>
			{
				var dialog = new SearchDialog();
				dialog.Load(mainConfig);
				return dialog;
			}, "search");
		}

		public void InitFolders(string path)
		{
			folderPath = Path.Combine(path, "Indexes");
		}

		private void Init(IUpdater updater)
		{
			var success = false;
			try
			{
				success =  engine.LoadSearchInfo(folderPath, updater);
				GC.Collect();
				GC.WaitForFullGCComplete();
				synchronizer(() => ShowDialog(success));
			}
			finally
			{
				availabilityChecker.EndInitSearch(success);
			}
		}

		public void ShowSearchDialog()
		{
			if (availabilityChecker.IndexesLoaded)
			{
				ShowDialog(true);
			}
			else
			{
				BeginOperation();
                DialogsCache.ShowProgress(Init, SearcherLang.LoadIndexes);
			}
		}

		private bool DoMakeIndexes(IUpdater updater)
		{
			var success = false;
			try
			{
				success = engine.MakeIndex(folderPath, updater);
			}
			finally
			{
				availabilityChecker.EndMakeIndexes(success);
			}
			GC.Collect();
			GC.WaitForFullGCComplete();
			return true;
		}

		public void RebuildIndexes()
		{
			BeginOperation();
			DialogsCache.ShowProgress(u => DoMakeIndexes(u), SearcherLang.CreateIndexes);
		}

		private void ShowDialog(bool success)
		{
			if (success)
			{
				searchDialog.LoadState(mainConfig.States);
				searchDialog.Value.ShowDialog(engine);
			}
			else searchDialog.Hide();
		}

		public void UnloadIndexes()
		{
			try
			{
				BeginOperation();
				engine.Unload();
				GC.Collect();
				GC.WaitForFullGCComplete();
			}
			finally
			{
				availabilityChecker.EndUnload();
			}
		}

		public void Fill(Config config)
		{
			mainConfig = config;
			if (searchDialog.IsValueCreated)
			{
				searchDialog.Value.Load(config);
			}
			engine = new SearchEngine(mainConfig.Index);
			UnloadIndexes();
		}

		public void Save(Config config, bool autosaveOnExit)
		{
			if (!autosaveOnExit) return;
			searchDialog.SaveState(config.States);
		}

		private void BeginOperation()
		{
			availabilityChecker.BeginOperation();
			if (!searchDialog.IsVisible) return;
			searchDialog.Hide();
			searchDialog.Value.Clear();
		}

		public void Dispose()
		{
			searchDialog.Dispose();
		}
	}
}
