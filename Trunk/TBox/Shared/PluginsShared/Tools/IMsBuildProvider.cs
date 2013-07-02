namespace PluginsShared.Tools
{
	public interface IMsBuildProvider
	{
		void Build(string mode, string path, bool waitEnd = false);
		string PathToMsBuild { get; }
	}
}
