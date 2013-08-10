using System;

namespace TBox.Code.AutoUpdate
{
	class CodePlexUpdater : IAutoUpdater
	{
		private const string CheckUrl = "https://tbox.svn.codeplex.com/svn/last.txt";
		private const string DownloadUrlTemplate = "https://tbox.codeplex.com/downloads/get/{0}";

		public bool TryUpdate(bool manual = false)
		{
			throw new NotImplementedException();
		}
	}
}
