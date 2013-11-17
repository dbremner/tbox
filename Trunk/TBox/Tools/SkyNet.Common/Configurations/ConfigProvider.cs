﻿using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using Common.Base.Log;
using Common.SaveLoad;

namespace SkyNet.Common.Configurations
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = false)]
    public class ConfigProvider<T> : IConfigProvider<T>
        where T: new()
    {
        private readonly ParamSerializer<T> serializer;
        private readonly ILog log = LogManager.GetLogger<ConfigProvider<T>>();
        public T Config { get; private set; }
        
        public ConfigProvider(string path)
        {
            serializer = new ParamSerializer<T>(path);
            Config = serializer.Load(new T());
        }

        public T Get()
        {
            return Config;
        }

        public void Set(T config)
        {
            try
            {
                serializer.Save(config);
                Config = config;
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't save config");
                if (WebOperationContext.Current != null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                }
            }
        }
    }
}