﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Common.Base;
using Common.Base.Log;
using Common.Console;
using Common.Tools;
using Searcher.Code.Finders.Parsers;
using Searcher.Code.Finders.Search;
using WPFControls.Tools;
using Searcher.Code;
using Searcher.Code.Checkers;
using Searcher.Code.Settings;

namespace Searcher.Components
{
	/// <summary>
	/// Interaction logic for SearchDialog.xaml
	/// </summary>
	sealed partial class SearchDialog : IDisposable
	{
		private static readonly ILog Log = LogManager.GetLogger<SearchDialog>();
		private Config config;
		private readonly DataTable filesTable = new DataTable();
		private readonly SearchAdder searchAdder = new SearchAdder();
		private SearchEngine searchEngine;
		private bool disableSearch = false;
		private readonly Stopwatch sw = new Stopwatch();

		public SearchDialog()
		{
			InitializeComponent();
			filesTable.Columns.Add("Name", typeof(String));
			filesTable.Columns.Add("Ext", typeof(String));
			filesTable.Columns.Add("Path", typeof(String));
			filesTable.DefaultView.Sort = "Path ASC";
			lvResult.ItemsSource = filesTable.DefaultView;
			cbCount.Items.AddRange("16", "64", "128", "256", "512", "1024");
			FileTypesPanel.View = FileTypes;
		}

		public override void Dispose()
		{
			filesTable.Dispose();
			base.Dispose();
		}

		public void Load(Config main)
		{
			disableSearch = true;
			DataContext = main;
			config = main;
			FileTypes.ItemsSource = config.Index.FileTypes;
			disableSearch = false;
		}

		internal void ShowDialog(SearchEngine engine)
		{
			disableSearch = true;
			if (!IsVisible)
			{
				ExceptionsHelper.HandleException(
					FillFolderFilter,
					() => "Can't enumerate directories",
					Log
					);
			}
			searchEngine = engine;
			edText.Clear();
			SearchText.SetFocus();
			ShowAndActivate();
			btnSearch.IsEnabled = SearchText.Text.Length > 0;
			disableSearch = false;
			if (btnSearch.IsEnabled)
			{
				DoSearch();
			}
		}

		private void FillFolderFilter()
		{
			FoldersFilter.Items.Clear();
			FoldersFilter.Items.Add(string.Empty);
			foreach (var folder in config.Index.FileNames.CheckedItems.Where(folder => Directory.Exists(folder.Key)))
			{
				AddPath(folder.Key);
				foreach (var subFolder in new DirectoryInfo(folder.Key).SafeEnumerateDirectories(Log))
				{
					AddPath(subFolder.FullName);
				}
			}
		}

		private void AddPath(string path)
		{
			if (!string.IsNullOrEmpty(path) && !path.EndsWith("\\"))
			{
				path += "\\";
			}
			FoldersFilter.Items.Add(path);
		}

		private DataRow GetSelectedCollection()
		{
			return ((DataRowView)lvResult.SelectedItem).Row;
		}

		private string GetSelectedFilePath()
		{
			return GetSelectedCollection().ItemArray[2].ToString();
		}

		private string GetSelectedFileExt()
		{
			return GetSelectedCollection().ItemArray[1].ToString();
		}

		private void LvResultMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (lvResult.SelectedIndex < 0 ||
				lvResult.SelectedIndex >= filesTable.Rows.Count) return;
		    Cmd.Start(config.Result.OpenWith, Log, string.Format("\"{0}\"", GetSelectedFilePath()), false, true);
		}

		private void LvResultSelectedIndexChanged(object sender, EventArgs e)
		{
			if (lvResult.SelectedIndex < 0 ||
				lvResult.SelectedIndex >= filesTable.Rows.Count ||
				config.Result.AutoLoadFile != true) return;
			try
			{
				sw.Restart();
				var path = GetSelectedFilePath();
				Title = string.Format("Searcher - [ {0} ]", path);
				edText.Clear();
				edText.Format = GetSelectedFileExt();
			    edText.Read(path);
				sbiWords.Content = edText.MarkAll(SearchText.Text, 
					config.Search.MatchCase);
				sbiLoadFileTime.Content = sw.ElapsedMilliseconds / 1000.0;
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Internal error.");
				edText.Value = Properties.Resources.ErrorLoadingFile;
				edText.Format = string.Empty;
				sbiLoadFileTime.Content = string.Empty;
			}
		}

		private void PrintResults(ICollection<int> files)
		{
			sbiSearchTime.Content = sw.ElapsedMilliseconds / 1000.0;
			sw.Restart();
			lvResult.Visibility = Visibility.Hidden;
			filesTable.Rows.Clear();
			if (files.Count != 0)
			{
				var results = files.Select(i => searchEngine.FileInformer.GetFilePath(i));
				if (FoldersFilter.SelectedValue != null)
				{
					var folder = FoldersFilter.SelectedValue.ToString();
					if (!string.IsNullOrEmpty(folder))
					{
						results = results.Where(x => x.StartsWith(folder));
					}
				}
				foreach (var path in results)
				{
					filesTable.Rows.Add(
						Path.GetFileNameWithoutExtension(path),
						GetFileExtension(path),
						path
						);
				}
			}
			lvResult.Visibility = Visibility.Visible;
			CollectionViewSource.GetDefaultView(lvResult.ItemsSource).Refresh();

			sbiFiles.Content = filesTable.Rows.Count;
			sbiFillResultsTime.Content = sw.ElapsedMilliseconds / 1000.0;
		}

		private static string GetFileExtension(string path)
		{
			var ext = Path.GetExtension(path);
			return !string.IsNullOrEmpty(ext) ? ext.Remove(0, 1) : ext;
		}

		private void SearchClick(object sender, RoutedEventArgs e)
		{
			if (disableSearch) return;
			ExceptionsHelper.HandleException(
				SafeSearch, 
				() => "Search internal error", 
				Log);
		}

		private void SafeSearch()
		{
			sw.Restart();
			var searchText = SearchText.Text.Trim();
			Title = "Searcher";
			edText.Value = string.Empty;
			sbiFiles.Content = string.Empty;
			sbiSearchTime.Content = string.Empty;
			sbiFillResultsTime.Content = string.Empty;
			sbiWords.Content = string.Empty;
			sbiLoadFileTime.Content = string.Empty;
			var files = new HashSet<int>();
			if (!string.IsNullOrWhiteSpace(searchText))
			{
				searchAdder.Words.Clear();
				if (config.Search.SearchMode == SearchMode.FileNames)
				{
					searchAdder.Words.Add(searchText);
				}
				else
				{
					var parser = new Parser(searchAdder, config.Index);
					parser.ParseFileData(searchText, 0);
				}
				if (!searchAdder.Words.Any())
				{
					MessageBox.Show("Text don't contain searchable symbols", "Searcher", MessageBoxButton.OK, MessageBoxImage.Hand);
					return;
				}
				if (searchAdder.Words.Count < 2 && (!config.Search.FullTextSearch || config.Search.SearchMode == SearchMode.FileNames))
				{
					Search(searchAdder.Words.FirstOrDefault(), files, config.Search.FileCount);
				}
				else
				{
					var firstWord = true;
					foreach (var word in searchAdder.Words)
					{
						var tmp = new HashSet<int>();
						Search(word, tmp, int.MaxValue);
						if (firstWord)
						{
							files = tmp;
							firstWord = false;
						}
						else
						{
							files.IntersectWith(tmp);
						}
					}
					var items = (IEnumerable<int>)files;
					if (config.Search.FullTextSearch && config.Search.SearchMode == SearchMode.FileData)
					{
						items = files.AsParallel().Where(x => FileContains(x, searchText)); 
					}
					PrintResults(items.Take(config.Search.FileCount).ToArray());
					return;
				}
			}
			PrintResults(files);
		}

		private bool FileContains(int i, string searchText)
		{
			try
			{
				var path = searchEngine.FileInformer.GetFilePath(i);
				return File.Exists(path) && NativeBoost.FileSystem.FileContains(path, searchText, config.Search.MatchCase);
			}
			catch (Exception)
			{
				return false;
			}
		}

		private void Search(string searchText, HashSet<int> files, int fileCount)
		{
			var checker = CreateChecker(searchText);
			switch (config.Search.SearchMode)
			{
				case SearchMode.FileNames:
					searchEngine.FileInformer.Find(
						GetTypes(), checker, fileCount, files);
					break;
				case SearchMode.FileData:
					searchEngine.WordsFinder.Find(
						GetTypes(), checker, fileCount, files);
					break;
				default:
					throw new ArgumentException("Unknown search mode: " + config.Search.SearchMode);
			}
		}

		private void DoSearch()
		{
			if(!IsVisible)return;
			SearchClick(null, null);
		}

		private void CbSearchTextKeyUp(object sender, KeyEventArgs e)
		{
			btnSearch.IsEnabled = SearchText.Text.Length > 0;
			if (!btnSearch.IsEnabled) return;
			if (e.Key == Key.Return)
			{
				SearchClick(sender, e);
			}
		}

		private IFileChecker CreateChecker(string searchText)
		{
			return FileCheckerFactory.Create(
				config.Search.CompareType, config.Search.MatchCase, searchText);
		}

		private ISet<string> GetTypes()
		{
			return new HashSet<string>(config.Index.FileTypes.CheckedItems.Select(x => x.Key)); 
		}

		public void Clear()
		{
			filesTable.Rows.Clear();
		}

		private void NeedSearch(object sender, RoutedEventArgs e)
		{
			DoSearch();
		}

		private void SelectedTextChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				btnSearch.IsEnabled = e.AddedItems[0].ToString().Length > 0;
			}
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
