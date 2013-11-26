using System.Drawing;
using System.Windows.Media;
using WPFWinForms;

namespace Interface 
{
	public interface IPlugin
	{
		void Init(IPluginContext context);
		UMenuItem[] Menu { get; }
		Icon Icon { get; set; }
		PluginGroup PluginGroup { get; set; }
	}
}
