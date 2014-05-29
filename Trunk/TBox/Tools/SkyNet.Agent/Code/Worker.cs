using System;
using System.Collections.Generic;
using System.Threading;
using Mnk.Library.Common.Log;
using Mnk.Library.ScriptEngine.Core.Interfaces;
using Mnk.TBox.Tools.SkyNet.Agent.Code.Interfaces;
using Mnk.TBox.Tools.SkyNet.Common;
using ScriptEngine.Core.Params;
using ServiceStack.Text;

namespace Mnk.TBox.Tools.SkyNet.Agent.Code
{
    class Worker : IWorker, IDisposable
    {
        private readonly ILog log = LogManager.GetLogger<Worker>();
        private readonly ISkyContext context;
        private readonly IFilesDownloader filesDownloader;
        private readonly IScriptCompiler<ISkyScript> compiler;
        private Thread thread;
        private readonly object locker = new object();
        public Worker(ISkyContext context, IScriptCompiler<ISkyScript> compiler, IFilesDownloader filesDownloader)
        {
            this.context = context;
            this.compiler = compiler;
            this.filesDownloader = filesDownloader;
        }

        public void Start(AgentTask task)
        {
            lock (locker)
            {
                context.Reset();
                Abort();
                thread = new Thread(o => ProcessTask(task));
                thread.Start();
            }
        }

        public bool IsDone
        {
            get { return thread == null; }
        }

        public void Cancel()
        {
            lock (locker)
            {
                context.Cancel();
            }
        }

        public void Abort()
        {
            if (thread == null) return;
            thread.Abort();
            thread = null;
        }

        public void Terminate()
        {
            lock (locker)
            {
                Abort();
            }
        }

        private void ProcessTask(AgentTask task)
        {
            try
            {
                var path = filesDownloader.DownloadAndUnpackFiles(task.ZipPackageId);
                var script = compiler.Compile(task.Script, JsonSerializer.DeserializeFromString<IList<Parameter>>(task.ScriptParameters));
                task.Report = script.AgentExecute(path, task.Config, context);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't execute task: " + task.Id);
                task.Report = ex.ToString();
            }
            finally
            {
                lock (locker)
                {
                    thread = null;
                }
            }
        }

        public void Dispose()
        {
            Terminate();
        }
    }
}
