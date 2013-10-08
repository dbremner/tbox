using System.IO;
using Common.Base.Log;
using Common.Console;

namespace PluginsShared.Tools
{
	public class MsBuildProvider : IMsBuildProvider
	{
		private readonly string rootDir;
		private static readonly ILog Log = LogManager.GetLogger<MsBuildProvider>();

		public string PathToMsBuild { get; private set; }

		public MsBuildProvider(string pathToMsBuild, string rootDir)
		{
			this.rootDir = rootDir;
			PathToMsBuild = pathToMsBuild;
		}

		private string GetCmdPath(string name)
		{
			var cmd = Path.Combine(rootDir, name + ".cmd");
			if (!File.Exists(cmd)) Log.Write("Can't find: {0}", cmd);
			return cmd;
		}

		public void BuildBuildFile(string path, string args)
		{
			Cmd.Start(GetCmdPath("build_build"), Log, string.Format("\"{0}\" \"{1}\" \"{2}\"", PathToMsBuild, path, args), false);
		}

		public void Build(string mode, string path, bool waitEnd = false)
		{
			Cmd.Start(GetCmdPath("build_project"), Log, string.Format("\"{0}\" \"{1}\" \"{2}\"", PathToMsBuild, path, mode), waitEnd);
		}
	}
}
