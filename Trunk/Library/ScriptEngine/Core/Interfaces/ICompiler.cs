namespace ScriptEngine.Core.Interfaces
{
	public interface ICompiler
	{
		void Execute(string sourceText);
		void Build(string sourceText);
	}
}
