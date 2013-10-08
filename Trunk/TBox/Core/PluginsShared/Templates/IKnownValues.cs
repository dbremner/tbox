using System.Collections.Generic;

namespace PluginsShared.Templates
{
	interface IKnownValues
	{
		IDictionary<string, string> Collection { get; }
		void Prepare(IStringFiller filler);
	}
}
