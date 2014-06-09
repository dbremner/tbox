using System;
using System.IO;
using NUnit.Core;
using NUnit.Util;

namespace Mnk.Library.ParallelNUnit.Core
{
    public static class NUnitBase
    {
        static NUnitBase()
        {
            CoreExtensions.Host.InitializeService();
            var settingsService = new SettingsService(true);
            ServiceManager.Services.AddService(settingsService);
            ServiceManager.Services.AddService(new DomainManager());
            ServiceManager.Services.AddService(new RecentFilesService());
            ServiceManager.Services.AddService(new ProjectService());
            ServiceManager.Services.AddService(new TestLoader());
            ServiceManager.Services.AddService(new AddinRegistry());
            ServiceManager.Services.AddService(new AddinManager());
            ServiceManager.Services.AddService(new TestAgency());
            ServiceManager.Services.InitializeServices();
            CoreExtensions.Host.AddinRegistry = Services.AddinRegistry;
        }

        public static TestPackage CreatePackage(string path, string runtimeFramework)
        {
            var p = new TestPackage(Path.GetFullPath(path));
            p.Settings["ProcessModel"] = ProcessModel.Default;
            p.Settings["DomainUsage"] = DomainUsage.Default;
            p.Settings["UseThreadedRunner"] = true;
            p.Settings["DefaultTimeout"] = 120000;
            p.Settings["WorkDirectory"] = AppDomain.CurrentDomain.BaseDirectory;
            p.Settings["StopOnError"] = false;
            p.Settings["ShadowCopyFiles"] = false;
            p.Settings["RuntimeFramework"] = string.IsNullOrEmpty(runtimeFramework) ? 
                RuntimeFramework.CurrentFramework :
                RuntimeFramework.Parse(runtimeFramework);
            
            return p;
        }

        public static void Dispose()
        {
            CoreExtensions.Host.UnloadService();
        }

    }
}
