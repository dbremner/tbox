using System.Collections.Generic;
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Server;

namespace Mnk.TBox.Tools.SkyNet.Server.Code.Settings
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
