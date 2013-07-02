using System.Drawing;
using WPFWinForms;

namespace Interface 
{
	public interface IPlugin
	{
		void Init(IPluginContext context);
		UMenuItem[] Menu { get; }
		Icon Icon { get; }
	}
}
