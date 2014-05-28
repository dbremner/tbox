﻿using System.Collections.Generic;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Tools.SkyNet.Server.Code.Settings
{
    public class Config
    {
        public IList<ServerAgent> Agents { get; private set; }
        public IList<ServerTask> Tasks { get; private set; }

        public Config()
        {
            Agents = new List<ServerAgent>();
            Tasks = new List<ServerTask>();
        }
    }
}
