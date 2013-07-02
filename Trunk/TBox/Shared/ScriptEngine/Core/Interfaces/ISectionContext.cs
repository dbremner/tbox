namespace ScriptEngine.Core.Interfaces
{
	public interface ISectionContext
	{
		void AddUndo(string function, params object[] args);
		bool UndoEmpty { get; }
	}
}
