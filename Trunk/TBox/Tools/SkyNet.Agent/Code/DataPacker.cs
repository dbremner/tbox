using System;
using System.IO;
using Ionic.Zip;
using Mnk.Library.Common.Log;
using Mnk.TBox.Tools.SkyNet.Agent.Code.Interfaces;

namespace Mnk.TBox.Tools.SkyNet.Agent.Code
{
    class DataPacker : IDataPacker
    {
        private readonly ILog log = LogManager.GetLogger<DataPacker>();
        public string Unpack(Stream s)
        {
            var zipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var folderPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            try
            {
                using (var fs = File.Create(zipPath))
                {
                    s.CopyTo(fs);
                }
                using (var zs = new ZipFile(zipPath))
                {
                    zs.ExtractAll(folderPath);
                }
            }
            catch (Exception ex)
            {
                log.Write(ex, "Can't unpack data");
                if(Directory.Exists(folderPath))Directory.Delete(folderPath, true);
                throw;
            }
            finally
            {
                if (File.Exists(zipPath)) File.Delete(zipPath);
            }
            return folderPath;
        }
    }
}
