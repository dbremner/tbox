using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;

namespace ServicesCommander.Code
{
	public class Profile : Data
	{
		public CheckableDataCollection<ServiceInfo> Services { get; set; }

		public Profile()
		{
			Services = new CheckableDataCollection<ServiceInfo>();
		}

		public override object Clone()
		{
			return new Profile {Key = Key, Services = Services.Clone()};
		}
	}
}
