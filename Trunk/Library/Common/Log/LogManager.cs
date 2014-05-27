using System;

namespace Mnk.Library.Common.Log
{
	public static class LogManager
	{
		private static AbstractLog logger = new NullLog();
		private static AbstractLog infoLogger = new NullLog();
		public static void Init(AbstractLog log, AbstractLog infoLog = null)
		{
			logger = log ?? new NullLog();
			infoLogger = infoLog??new NullLog();
		}

		public static AbstractLog GetLogger(Type type)
		{
			return new TypedLogger(()=>logger, type.FullName);
		}

		public static AbstractLog GetLogger<T>()
		{
			return GetLogger(typeof(T));
		}

		public static AbstractLog GetInfoLogger(Type type)
		{
			return new TypedLogger(() => infoLogger, type.FullName);
		}

		public static AbstractLog GetInfoLogger<T>()
		{
			return GetInfoLogger(typeof(T));
		}
	}
}