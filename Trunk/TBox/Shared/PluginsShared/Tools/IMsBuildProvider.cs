namespace PluginsShared.Tools
{
	public interface IMsBuildProvider
	{
		void Build(string mode, string path, bool waitEnd = false);
		void Build(string path);
		string PathToMsBuild { get; }
	}
}
