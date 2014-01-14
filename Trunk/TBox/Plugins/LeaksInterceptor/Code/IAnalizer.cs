using System.Collections.Generic;
using Mnk.Library.WPFControls.Components.Drawings.Graphics;

namespace Mnk.TBox.Plugins.LeaksInterceptor.Code
{
	interface IAnalizer
	{
		bool Analize();
		string CopyResults();
		IGraphic GetGraphic(string name);
		IEnumerable<string> GetNames();
	}
}
