using Common.UI.Model;
using Interface;

namespace TBox.Code.Objects
{
	public class EnginePluginInfo : CheckableData
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public IPlugin Plugin { get; set; }

		public EnginePluginInfo(string key, string name, string description, bool isChecked)
		{
			Key = key;
			Name = name;
			Description = description;
			IsChecked = isChecked;
		}
	}
}
