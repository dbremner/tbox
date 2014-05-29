using Mnk.Library.ScriptEngine;

namespace Mnk.TBox.Plugins.SkyNet.Code.Interfaces
{
    public interface ITaskExecutor
    {
        string Execute(SingleFileOperation operation);
    }
}