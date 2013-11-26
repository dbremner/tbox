using System.IO;
using System.Linq;
using Common.Base.Log;
using Common.MT;
using Common.SaveLoad;
using Common.Tools;
using ConsoleScriptRunner.Settings;
using PluginsShared.Automator;
using ScriptEngine;
using ScriptEngine.Core;

namespace ConsoleScriptRunner
{
	class Worker
	{
		private static readonly ILog Log = LogManager.GetLogger<Worker>();
		private readonly ParamSerializer<Config> serializer;
		private readonly string rootPath;

		public Worker(string cfgPath, string rootPath)
		{
			this.rootPath = rootPath;
			serializer = new ParamSerializer<Config>(Path.Combine(cfgPath, "Config/Automater.config"));
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
				foreach (var path in op.Pathes.CheckedItems)
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
