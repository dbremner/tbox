﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Tools.SkyNet.Common;
using Mnk.TBox.Tools.SkyNet.Server.Code.Interfaces;

namespace Mnk.TBox.Tools.SkyNet.Server.Code
{
    class SkyNetServerTasksLogic : ISkyNetServerTasksLogic
    {
        private readonly IHttpContextHelper contextHelper;
        private readonly IStorage storage;
        private readonly ISkyContext context;

        public SkyNetServerTasksLogic(IHttpContextHelper contextHelper, IStorage storage, ISkyContext context)
        {
            this.contextHelper = contextHelper;
            this.storage = storage;
            this.context = context;
        }

        public string AddTask(ServerTask task)
        {
            task.Id = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(task.Config) || string.IsNullOrEmpty(task.Script))
            {
                contextHelper.SetStatusCode(HttpStatusCode.ExpectationFailed);
                return string.Empty;
            }
            storage.Config.Tasks.Add(new ServerTask
            {
                Id = task.Id,
                State = TaskState.Idle,
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

        public IList<ServerTask> GetTasks()
        {
            return storage.Config.Tasks
                        .Select(x => new ServerTask
                        {
                            State = x.State,
                            Progress = x.Progress,
                            IsDone = x.IsDone,
                            Owner = x.Owner,
                            CreatedTime = x.CreatedTime
                        })
                        .ToArray();
        }

        public void CancelTask(string id)
        {

        }

        public void TerminateTask(string id)
        {
            
        }

        public string DeleteTask(string id)
        {
            var task = storage.Config.Tasks.FirstOrDefault(x => x.Id.EqualsIgnoreCase(id));
            if (task == null)
            {
                contextHelper.SetStatusCode(HttpStatusCode.NotFound);
                return string.Empty;
            }
            if (task.State == TaskState.InProgress)
            {
                contextHelper.SetStatusCode(HttpStatusCode.Conflict);
                return string.Empty;
            }
            storage.Config.Tasks.Remove(task);
            storage.Save();
            return task.Report;
        }
    }
}
