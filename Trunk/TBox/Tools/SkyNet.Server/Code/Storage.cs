using System;
using System.IO;
using Mnk.Library.Common.SaveLoad;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Tools.SkyNet.Server.Code.Settings;

namespace Mnk.TBox.Tools.SkyNet.Server.Code
{
    class Storage 
    {
        private readonly ParamSerializer<Config> serializer = new ParamSerializer<Config>(
                Path.Combine(Folders.UserToolsFolder, "SkyNet.Server.Data.config"));
        public Storage()
        {
            Config = serializer.Load(new Config());
        }

        public void Save()
        {
            serializer.Save(Config);
        }

        public Config Config { get; private set; }
    }
}
