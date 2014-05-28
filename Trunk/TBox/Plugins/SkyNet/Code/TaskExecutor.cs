using System.IO;
using System.Windows;
using Mnk.Library.ScriptEngine;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;

namespace Mnk.TBox.Plugins.SkyNet.Code
{
    class TaskExecutor : ITaskExecutor
    {
        private readonly IDataPacker dataPacker;
        private readonly IServicesBuilder servicesBuilder;
        private readonly IConfigsFacade configsFacade;

        public TaskExecutor(IServicesBuilder servicesBuilder, IConfigsFacade configsFacade, IDataPacker dataPacker)
        {
            this.servicesBuilder = servicesBuilder;
            this.configsFacade = configsFacade;
            this.dataPacker = dataPacker;
        }

        public void Execute(SingleFileOperation op)
        {
            string name;
            var path = dataPacker.Pack(
                @"d:\sample.dll",
                new[] { "*.dll" }, out name);
            using (var cl = servicesBuilder.CreateFileServerClient(configsFacade.GetAgentConfig()))
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
