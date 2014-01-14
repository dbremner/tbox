namespace Mnk.Library.ScriptEngine.Core.Interfaces
{
	public interface ICompiler<out T>
	{
		T Compile(string sourceText);
	}
}
