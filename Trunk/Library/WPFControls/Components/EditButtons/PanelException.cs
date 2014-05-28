using System;

namespace Mnk.Library.WpfControls.Components.EditButtons
{
	[Serializable]
	public sealed class PanelException : Exception
	{
		public PanelException(string message) : base(message) { }
	}
}
