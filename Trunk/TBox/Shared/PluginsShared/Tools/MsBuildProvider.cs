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

		private string CmdPath
		{
			get
			{
				var cmd = Path.Combine(rootDir, "msbuild.cmd");
				if (!File.Exists(cmd)) Log.Write("Can't find: {0}", cmd);
				return cmd;
			}
		}

		public void Build(string path)
		{
			Cmd.Start(CmdPath, Log, string.Format("\"{0}\" \"{1}\"", PathToMsBuild, path), false);
		}

		public void Build(string mode, string path, bool waitEnd = false)
		{
			Cmd.Start(CmdPath, Log, string.Format("\"{0}\" \"{1}\" \"{2}\"", PathToMsBuild, path, mode), waitEnd);
		}
	}
}
