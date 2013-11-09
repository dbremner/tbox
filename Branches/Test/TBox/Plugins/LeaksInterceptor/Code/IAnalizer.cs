using System.Collections.Generic;
using WPFControls.Drawings.Graphics;

namespace LeaksInterceptor.Code
{
	interface IAnalizer
	{
		bool Analize();
		string CopyResults();
		IGraphic GetGraphic(string name);
		IEnumerable<string> GetNames();
	}
}
