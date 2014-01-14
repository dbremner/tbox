using System;

namespace Mnk.Library.WPFControls.Components.EditButtons
{
	[Serializable]
	public sealed class PanelException : Exception
	{
		public PanelException(string message) : base(message) { }
	}
}
