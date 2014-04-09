using System.IO;
using System.Windows;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.ScriptEngine;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Files;

namespace Mnk.TBox.Plugins.SkyNet.Code
{
    class TaskExecutor
    {
        private readonly DataPacker dataPacker = new DataPacker();
        private readonly ServicesFacade servicesFacade;

        public TaskExecutor(ServicesFacade servicesFacade)
        {
            this.servicesFacade = servicesFacade;
        }

        public void Execute(SingleFileOperation op)
        {
            string name;
            var path = dataPacker.Pack(
                @"d:\tests.dll",
                new[] { "*.dll" }, out name);
            using (var cl = servicesFacade.CreateFileServerClient(servicesFacade.GetAgentConfig()))
            {
                using (var f = File.OpenRead(path))
                {
                    var id = cl.Instance.Upload(f);
                    MessageBox.Show(id);
                }
            }
            MessageBox.Show(path);
            File.Delete(path);
        }
    }
}
