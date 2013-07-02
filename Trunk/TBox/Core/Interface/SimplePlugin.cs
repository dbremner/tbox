using System.Drawing;
using WPFWinForms;

namespace Interface
{
	public abstract class SimplePlugin : IPlugin
	{
		public IPluginContext Context { get; private set; } 
		public UMenuItem[] Menu { get; protected set; }
		public Icon Icon { get; protected set; }

		public virtual void Init(IPluginContext context)
		{
			Context = context;
		}
	}
}
