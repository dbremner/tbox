using Common.UI.Model;

namespace ProjectMan.Code.Settings
{
	public class ProjectInfo : CheckableData
	{
		public string MsBuildParams { get; set; }

		public ProjectInfo()
		{
			MsBuildParams = string.Empty;
		}

		public override object Clone()
		{
			return new ProjectInfo
				{
					IsChecked = IsChecked,
					Key = Key,
					MsBuildParams = MsBuildParams
				};
		}
	}
}
