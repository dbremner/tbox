using PluginsShared.Tools;

namespace ProjectMan.Code
{
	class ProjectContext
	{
		public ProjectContext(ISvnProvider svnProvider, IMsBuildProvider msBuildProvider)
		{
			MsBuildProvider = msBuildProvider;
			SvnProvider = svnProvider;
		}

		public ISvnProvider SvnProvider { get; private set; }
		public IMsBuildProvider MsBuildProvider { get; private set; }
	}
}
