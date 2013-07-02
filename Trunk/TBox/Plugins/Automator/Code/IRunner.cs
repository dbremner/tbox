using System;

namespace Automator.Code
{
	interface IRunner
	{
		void Run(string path, Action<Action> dispatcher);
	}
}
