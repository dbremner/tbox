using System;
using NUnitAgent.Code;

namespace NUnitAgent
{
	class Program
	{
		[STAThread]
		static int Main(string[] args)
		{
            Console.WriteLine("Execute NUnitAgent with arguments: " + string.Join("; ", args));
			var worker = new Worker();
			return worker.Run(args);
		}
	}
}
