using Mnk.Library.ScriptEngine;

namespace Mnk.TBox.Plugins.SkyNet.Code.Interfaces
{
    internal interface ITaskExecutor
    {
        void Execute(SingleFileOperation op);
    }
}