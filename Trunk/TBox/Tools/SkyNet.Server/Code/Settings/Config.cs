using System.Collections.Generic;
using SkyNet.Common.Contracts.Server;

namespace SkyNet.Server.Code.Settings
{
    class Config
    {
        public IList<ServerAgent> Agents { get; set; }
        public IList<ServerTask> Tasks { get; set; }

        public Config()
        {
            Agents = new List<ServerAgent>();
            Tasks = new List<ServerTask>();
        }
    }
}
