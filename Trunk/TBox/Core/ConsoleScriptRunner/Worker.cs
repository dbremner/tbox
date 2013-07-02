using System.IO;
using System.Linq;
using Common.Base.Log;
using Common.SaveLoad;
using Common.Tools;
using ConsoleScriptRunner.Settings;
using ScriptEngine.Core;

namespace ConsoleScriptRunner
{
	class Worker
	{
		private static readonly ILog Log = LogManager.GetLogger<Worker>();
		private readonly ParamSerializer<Config> serializer;
		private readonly string rootPath;

		public Worker(string rootPath)
		{
			this.rootPath = rootPath;
			serializer = new ParamSerializer<Config>(Path.Combine(rootPath, "Config/Automater.config"));
		}

		public void Run(string profileToRun)
		{
			var config = serializer.Load();
			if (config == null)
			{
				Log.Write("Can't load config");
				return;
			}
			var profile = config.Profiles.FirstOrDefault(x => x.Key.EqualsIgnoreCase(profileToRun));
			if (profile == null)
			{
				Log.Write("Can't load profile: " + profileToRun);
				return;
			}
			var compiler = new ScriptCompiler();

			foreach (var op in profile.Operations)
			{
				foreach (var path in op.Pathes.CheckedItems)
				{
					compiler.Execute(
						File.ReadAllText(Path.Combine(rootPath, "Data/Automater/", path.Key)), 
						op.Parameters,
						a=>a());
				}
			}
		}
	}
}
