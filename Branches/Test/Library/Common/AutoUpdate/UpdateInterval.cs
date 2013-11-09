using System;

namespace Common.AutoUpdate
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
