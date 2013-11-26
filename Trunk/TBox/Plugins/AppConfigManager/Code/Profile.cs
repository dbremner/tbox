using System;
using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;

namespace AppConfigManager.Code
{
	[Serializable]
	public class Profile : Data
	{
		public CheckableDataCollection<CheckableData> Files { get; set; }
		public CheckableDataCollection<Option> Options { get; set; }

		public Profile()
		{
			Files = new CheckableDataCollection<CheckableData>();
			Options = new CheckableDataCollection<Option>();
		}

		public override object Clone()
		{
			return new Profile
			{
				Key = Key,
				Files = Files.Clone(),
				Options = Options.Clone()
			};
		}
	}
}
