using System;

namespace Mnk.Library.Common.AutoUpdate
{
	[Serializable]
	public enum UpdateInterval
	{
		Startup,
		Dayly,
		Weekly,
		Monthly,
		Never
	}
}
