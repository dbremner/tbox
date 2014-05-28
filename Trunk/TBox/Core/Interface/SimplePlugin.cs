using System.Drawing;
using System.Windows.Media;
using Mnk.Library.WpfWinForms;

namespace Mnk.TBox.Core.Interface
{
	public abstract class SimplePlugin : IPlugin
	{
		public IPluginContext Context { get; private set; } 
		public UMenuItem[] Menu { get; protected set; }
		public Icon Icon { get; set; }
		public PluginGroup PluginGroup { get; set; }

		public virtual void Init(IPluginContext context)
		{
			Context = context;
		}
	}
}
