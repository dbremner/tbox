using System;

namespace Mnk.Library.Common.Log
{
	sealed class TypedLogger : AbstractLog
	{
		private readonly Func<AbstractLog> logger;
		private readonly string fullName;
		public TypedLogger(Func<AbstractLog> logger, string fullName)
		{
			this.logger = logger;
			this.fullName = fullName;
		}

		public override void Write(string value)
		{
			logger().Write(fullName + ": " + value);
		}

		public override void Write(Exception ex, string value)
		{
			logger().Write(ex, fullName + ": " + value);
		}
	}

}
