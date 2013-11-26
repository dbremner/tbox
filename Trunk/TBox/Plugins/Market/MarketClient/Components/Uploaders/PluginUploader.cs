using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Common.MT;
using Common.SaveLoad;
using MarketClient.Code;
using MarketClient.ServiceReference;
using WPFControls.Dialogs;

namespace MarketClient.Components.Uploaders
{
	public class PluginUploader
	{
		private readonly Func<Plugin> creator;
		public PluginUploader(Func<Plugin> creator)
		{
			this.creator = creator;
		}

		private PluginUploadContract CreateContract(Plugin item, IList<string> pathes, bool allowNoExistFile)
		{
			var ret = new PluginUploadContract { Item = item };
			foreach (var path in pathes.Where(path => string.IsNullOrWhiteSpace(path) || !File.Exists(path)))
			{
				if (!allowNoExistFile)
				{
					throw new Exception(string.Format("File '{0}' not exist!", path));
				}
				return ret;
			}
			Stream stream;
			ret.Length = ExtFile.LoadDirectoryFiles(pathes, out ret.Descriptions, out stream);
			var streamWithProgress = new StreamWithProgress(stream);
			streamWithProgress.ProgressChanged += ProgressChanged;
			ret.FileByteStream = streamWithProgress;

			return ret;
		}

		private void ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			updater.Update(string.Empty, e.Length == 0 ? 0 : e.BytesRead / (float)e.Length);
		}

		private static void ShowError(string text)
		{
			MessageBox.Show(text, "Plugins error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		private IUpdater updater;
		private void DoWithProgress(string caption, Action action)
		{
			DialogsCache.ShowProgress(u =>
			{
				this.updater = u;
				try
				{
					action();
				}
				finally
				{
					Synchronizer.RefreshTables(u);
				}
			}, caption, null);
		}

		public void Upload(string name, string[] pathes)
		{
			var ret = new UploadContract();
			DoWithProgress(
				string.Format("Uploading plugin: '{0}'", name),
				() => Synchronizer.Do(service =>
									 ret = service.Plugin_Upload(CreateContract(creator(), pathes, false))));
			if (!ret.Success)
			{
				ShowError(string.Format("Can't upload: '{0}'. {1}",
					name,
					(ret.Exist ? "Already exist!" : "Internal error!")));
			}
		}

		public void Upgrade(string name, string[] pathes)
		{
			var ret = new UploadContract();
			DoWithProgress(
							string.Format("Upgrading plugin: '{0}'", name),
							() => Synchronizer.Do(service =>
												 ret = service.Plugin_Upgrade(CreateContract(creator(), pathes, true))));
			if (!ret.Success)
			{
				ShowError(string.Format("Can't upgrade: '{0}'. {1}",
					name,
					(!ret.Exist ? "Not exist!" : "Internal error!")));
			}
		}

		public void Delete(string name)
		{
			if (MessageBox.Show(string.Format("Are you want delete '{0}'?", name),
								name, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
			var ret = false;
			DoWithProgress(
				string.Format("Delete file: '{0}'", name),
				() => Synchronizer.Do(service =>
									  ret = service.Plugin_Delete(creator())));
			if (!ret)
			{
				ShowError(string.Format("Can't delete: '{0}'.", name));
			}
		}

		public void Refresh()
		{
			DoWithProgress("Refreshing..", () => { });
		}

		public Plugin[] Items { get; private set; }

		public void GetList(IMarketService service, string author)
		{
			Items = service.Plugin_GetList(new Plugin { Author = author }, 0, int.MaxValue, null);
		}
	}
}
