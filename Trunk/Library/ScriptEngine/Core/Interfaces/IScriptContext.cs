namespace ScriptEngine.Core.Interfaces
{
	public interface IScriptContext : ISectionContext
	{
		string Resolve(string path);
		string GenerateNextUndoPath();
	}
}
