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
            var settingsService = new SettingsService(false);
            ServiceManager.Services.AddService(settingsService);
            ServiceManager.Services.AddService(new ProjectService());
            ServiceManager.Services.AddService(new DomainManager());
            ServiceManager.Services.AddService(new TestLoader());
            ServiceManager.Services.AddService(new AddinRegistry());
            ServiceManager.Services.AddService(new AddinManager());
            ServiceManager.Services.InitializeServices();
            CoreExtensions.Host.AddinRegistry = Services.AddinRegistry;
        }

        public static TestPackage CreatePackage(string path, string runtimeFramework)
        {
            var p = new TestPackage(Path.GetFullPath(path));
            p.Settings["ProcessModel"] = ProcessModel.Single;
            p.Settings["DomainUsage"] = DomainUsage.None;
            p.Settings["ShadowCopyFiles"] = false;
            if (!string.IsNullOrEmpty(runtimeFramework))
                p.Settings["RuntimeFramework"] = RuntimeFramework.Parse(runtimeFramework);
            return p;
        }

        public static void Dispose()
        {
            CoreExtensions.Host.UnloadService();
        }

    }
}
