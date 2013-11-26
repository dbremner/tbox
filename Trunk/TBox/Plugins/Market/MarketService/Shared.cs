using System;
using System.IO;
using Common.Base;
using Common.Base.Log;

namespace MarketService
{
	static class Shared
	{
		public const string Plugins = "Plugins";
		public static readonly string FolderName = Path.Combine(Environment.CurrentDirectory, "Data");
		private static int lineNo = 0;
		public static void Do(string description, Action action, ILog log)
		{
			try
			{
				var time = Environment.TickCount;
				action();
				Console.WriteLine("{0} => {1} : {2}", ++lineNo, description, Environment.TickCount - time);
			}
			catch (Exception ex)
			{
				log.Write(ex, description);
			}
		}
	}
}
