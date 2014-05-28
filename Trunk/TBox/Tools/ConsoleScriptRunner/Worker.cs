﻿using System.IO;
using System.Linq;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.MT;
using Mnk.Library.Common.SaveLoad;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Core.PluginsShared.Automator;
using Mnk.Library.ScriptEngine;
using Mnk.Library.ScriptEngine.Core;
using Mnk.TBox.Tools.ConsoleScriptRunner.Settings;

namespace Mnk.TBox.Tools.ConsoleScriptRunner
{
	class Worker
	{
		private static readonly ILog Log = LogManager.GetLogger<Worker>();
		private readonly ConfigurationSerializer<Config> serializer;
		private readonly string rootPath;

		public Worker(string cfgPath, string rootPath)
		{
			this.rootPath = rootPath;
			serializer = new ConfigurationSerializer<Config>(Path.Combine(cfgPath, "Config/Automater.config"));
		}

		public void Run(string profileToRun)
		{
			var config = serializer.Load();
			if (config == null)
			{
				Log.Write("Can't load configuration");
				return;
			}
			var profile = config.Profiles.FirstOrDefault(x => x.Key.EqualsIgnoreCase(profileToRun));
			if (profile == null)
			{
				Log.Write("Can't load profile: " + profileToRun);
				return;
			}
			var compiler = new ScriptCompiler<IScript>();
		    var context = new ScriptContext {Updater = new ConsoleUpdater()};

			foreach (var op in profile.Operations)
			{
				foreach (var path in op.Paths.CheckedItems)
				{
					var s = compiler.Compile(
						File.ReadAllText(Path.Combine(rootPath, "Data/Automater/", path.Key)), 
						op.Parameters);
                    s.Run(context);
				}
			}
		}
	}
}
