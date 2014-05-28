using System;

namespace Mnk.Library.WpfControls.Components.Drawings.DirectionsTable
{
	public interface IDirectionable : ICloneable
	{
		string From { get; set; }
		string To { get; set; }
		string Title { get; set; }
	}
}
