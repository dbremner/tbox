﻿using System;
using System.IO;
using Mnk.Library.Common;
using Mnk.Library.Common.Log;

namespace Mnk.TBox.Core.Application.Code.ErrorsSender
{
	class LogsSender
	{
		private static readonly ILog Log = LogManager.GetLogger<LogsSender>();
		private readonly string sourceFile;

		public LogsSender(string sourceFile)
		{
			this.sourceFile = sourceFile;
		}

		public void SendIfNeed(string destinationFolder)
		{
			ExceptionsHelper.HandleException(
				()=>DoSendIfNeed(destinationFolder),
				()=>"Can't copy log to: " + destinationFolder,
				Log
				);
		}

		private void DoSendIfNeed(string destinationFolder)
		{
			if (File.Exists(sourceFile) && !string.IsNullOrEmpty(destinationFolder))
			{
				File.Copy(sourceFile, Path.Combine(destinationFolder, Guid.NewGuid() + ".log"), true);
			}
		}
	}
}
