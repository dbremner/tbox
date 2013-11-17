using System;
using System.IO;
using Common.SaveLoad;
using Interface;
using SkyNet.Server.Code.Settings;

namespace SkyNet.Server.Code
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
