using ScriptEngine.Core.Interfaces;

namespace ScriptEngine.Core
{
	class ScriptRunner : IScriptRunner
	{
		public void Run(IScript script)
		{
			script.Run();
		}
	}
}
