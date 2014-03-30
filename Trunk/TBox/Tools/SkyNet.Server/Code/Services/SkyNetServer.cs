using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using Mnk.Library.Common.Base.Log;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Tools.SkyNet.Common.Configurations;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Server;

namespace Mnk.TBox.Tools.SkyNet.Server.Code.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class SkyNetServer : ISkyNetCommon
    {
        private readonly string rootFolder;
        private readonly ServerConfig config;
        private readonly ILog log = LogManager.GetLogger<SkyNetServer>();
        private readonly Storage storage = new Storage();

        public SkyNetServer(ServerConfig config)
        {
            this.config = config;
            rootFolder = Path.GetTempPath();
        }

        public IList<ServerAgent> GetAgents()
        {
            try
            {
                lock (storage)
                {
                    return storage.Config.Agents;
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex);
                return new List<ServerAgent>();
            }
        }

        public void ConnectAgent(ServerAgent agent)
        {
            try 
            { 
                lock (storage)
                {
                    if (string.IsNullOrEmpty(agent.Endpoint) || agent.TotalCores <= 0 )
                    {
                        SetStatusCode(HttpStatusCode.ExpectationFailed);
                        return;
                    }
                    var exist = storage.Config.Agents.FirstOrDefault(x => x.Endpoint.EqualsIgnoreCase(agent.Endpoint));
                    if (exist != null)
                    {
                        exist.TotalCores = agent.TotalCores;
                    }
                    else
                    {
                        storage.Config.Agents.Add(new ServerAgent
                        {
                            Endpoint = agent.Endpoint,
                            TotalCores = agent.TotalCores,
                            State = ServerAgentState.Idle
                        });
                    }
                    storage.Save();
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex);
            }
        }

        public void DisconnectAgent(string endpoint)
        {
            try
            {
                lock (storage)
                {
                    var agent = storage.Config.Agents.FirstOrDefault(x => x.Endpoint.EqualsIgnoreCase(endpoint));
                    if (agent == null)
                    {
                        SetStatusCode(HttpStatusCode.NotFound);
                        return;
                    }
                    storage.Config.Agents.Remove(agent);
                    storage.Save();
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex);
            }
        }

        public string AddTask(ServerTask task)
        {
            try
            {
                lock (storage)
                {
                    task.Id = Guid.NewGuid().ToString();
                    if (string.IsNullOrEmpty(task.Config) || string.IsNullOrEmpty(task.Script))
                    {
                        SetStatusCode(HttpStatusCode.ExpectationFailed);
                        return string.Empty;
                    }
                    storage.Config.Tasks.Add(new ServerTask
                    {
                        Id = task.Id,
                        Config = task.Config,
                        Script = task.Script,
                        Owner = task.Owner,
                        CreatedTime = DateTime.UtcNow,
                        IsDone = false,
                        Progress = 0,
                        Report = string.Empty
                    });
                    storage.Save();
                    return task.Id;
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex);
                return string.Empty;
            }
        }

        public string DeleteTask(string id)
        {
            try
            {
                lock (storage)
                {
                    var task = storage.Config.Tasks.FirstOrDefault(x => x.Id.EqualsIgnoreCase(id));
                    if (task == null)
                    {
                        SetStatusCode(HttpStatusCode.NotFound);
                        return string.Empty;
                    }
                    storage.Config.Tasks.Remove(task);
                    storage.Save();
                    return task.Report;
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex);
                return string.Empty;
            }
        }

        public void PingIsAlive()
        {
        }

        public IList<ServerTask> GetTasks()
        {
            try
            {
                lock (storage)
                {
                    return storage.Config.Tasks
                        .Select(x=>new ServerTask
                            {
                                Progress = x.Progress,
                                IsDone = x.IsDone,
                                Owner = x.Owner,
                                CreatedTime = x.CreatedTime
                            })
                        .ToArray();
                }
            }
            catch (Exception ex)
            {
                ProcessError(ex);
                return new List<ServerTask>();
            }
        }


        private static void SetStatusCode(HttpStatusCode status)
        {
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = status;
            }
        }

        private void ProcessError(Exception ex)
        {
            log.Write(ex, "Unexpected exception");
            SetStatusCode(HttpStatusCode.InternalServerError);
        }

        public string Upload(Stream s)
        {
            var id = Guid.NewGuid().ToString();
            try
            {
                var path = GetFilePath(id);
                if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);
                var fileInfo = new FileInfo(path);
                using (var f = fileInfo.OpenWrite())
                {
                    using (s)
                    {
                        s.CopyTo(f);
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                try
                {
                    var path = GetFilePath(id);
                    if (File.Exists(path)) File.Delete(path);
                }
                catch(Exception fex)
                {
                    log.Write(fex, "Can't delete broken file: " + id);
                }
                ProcessError(ex);
                return string.Empty;
            }
        }

        public Stream Download(string id)
        {
            try
            {
                var info = GetFileInfo(id);
                return info.OpenRead();
            }
            catch (Exception ex)
            {
                ProcessError(ex);
                return null;
            }
        }

        public void Delete(string id)
        {
            try
            {
                GetFileInfo(id).Delete();
            }
            catch (Exception ex)
            {
                ProcessError(ex);
            }
        }

        private FileInfo GetFileInfo(string id)
        {
            var fileInfo = new FileInfo(GetFilePath(id));
            if (fileInfo.Exists == false)
            {
                throw new FileNotFoundException("File not found", id);
            }
            return fileInfo;
        }

        private string GetFilePath(string id)
        {
            return Path.Combine(rootFolder, id);
        }
    }
}
