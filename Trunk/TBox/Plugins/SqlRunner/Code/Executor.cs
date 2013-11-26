﻿using System;
using System.Windows;
using System.Windows.Media;
using Localization.Plugins.SqlRunner;
using SqlRunner.Code.Settings;
using WPFControls.Code;
using WPFControls.Code.OS;
using WPFControls.Dialogs.StateSaver;
using WPFSyntaxHighlighter;

namespace SqlRunner.Code
{
	sealed class Executor : BaseExecutor, IDisposable
	{
		private readonly LazyDialog<MemoBox> message = new LazyDialog<MemoBox>(()=>new MemoBox(), "message");

		public void Execute(Window owner, Op operation, string connectionString, Config config, Action onEnd, ImageSource icon)
		{
			if(message.IsVisible)message.Hide();
		    message.Value.Icon = icon;
            base.Execute(owner, operation, connectionString, i => Finish(i, owner, operation.Key, config, onEnd), icon);
		}

		private void Finish(DatabaseInfo response, Window owner, string name, Config config, Action onEnd)
		{
			if (onEnd != null) onEnd();
			Mt.Do(owner,
			      () =>
				      {
					      message.LoadState(config.States);
					      message.Value.ShowDialog(
                              string.Format(SqlRunnerLang.RequestResultsTemplate, name, response.Time / 1000.0),
						      BuildMessage(response), string.Empty, owner);
				      }
					);
		}

		private static string BuildMessage(DatabaseInfo response)
		{
			return response.Status + Environment.NewLine + response.Body;
		}

		public void Save(Config config)
		{
			message.SaveState(config.States);
		}

		public void Dispose()
		{
			message.Dispose();
		}
	}
}
