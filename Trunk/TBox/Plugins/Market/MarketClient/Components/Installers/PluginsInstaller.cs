using System.Linq;
using MarketClient.ServiceReference;

namespace MarketClient.Components.Installers
{
	public class PluginsInstaller
	{
		public Plugin[] Items { get; set; }

		public string[] Names
		{
			get { return Items.Select(x => x.Name).ToArray(); }
		}
	}
}
