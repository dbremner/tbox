using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Common.OS
{
    public class ProcessesRunner : IDisposable
    {
        class ProcessesStartInfo
        {
            public string Path { get; set; }
            public string Args { get; set; }
            public bool WithAdminRights { get; set; }
        }

        private readonly object locker = new object();
        private readonly List<ProcessesStartInfo> items = new List<ProcessesStartInfo>();
        private readonly List<Process> processes = new List<Process>();

        public void Add(string path, string args = null, bool runAsAdmin = false)
        {
            items.Add(new ProcessesStartInfo
            {
                Path = path,
                Args = args,
                WithAdminRights = runAsAdmin,
            });
        }

        public void Start()
        {
            lock (locker)
            {
                var adminsCommand = 
                    string.Join("&", items
                                        .Where(x => x.WithAdminRights)
                                        .Select(x => string.Format("\"{0}\" \"{1}\"", x.Path, x.Args))
                    );
                if (!string.IsNullOrEmpty(adminsCommand))
                {
                    processes.Add(Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c " + adminsCommand,
                        CreateNoWindow = true,
                        UseShellExecute = true,
                        Verb = "runas"
                    }));
                }
                foreach (var info in items.Where(x => !x.WithAdminRights))
                {
                    processes.Add(
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = info.Path,
                            Arguments = info.Args,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                        }));
                }
            }
        }

        public void Stop()
        {
            lock (locker)
            {
                foreach (var p in processes)
                {
                    p.Kill();
                }
            }
        }

        public void WaitForExit()
        {
            lock (locker)
            {
                foreach (var p in processes)
                {
                    p.WaitForExit();
                    p.Dispose();
                }
                processes.Clear();
            }
        }


        public void Dispose()
        {
            lock (locker)
            {
                foreach (var p in processes)
                {
                    p.Dispose();
                }
                processes.Clear();
            }
            items.Clear();
        }
    }
}
