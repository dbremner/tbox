using Mnk.Library.ScriptEngine;

namespace Mnk.TBox.Plugins.SkyNet.Code.Interfaces
{
    internal interface ITaskExecutor
    {
        string Execute(SingleFileOperation operation);
    }
}