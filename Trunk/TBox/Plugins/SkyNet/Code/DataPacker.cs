using System.IO;
using System.Linq;
using Ionic.Zip;
using Ionic.Zlib;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;

namespace Mnk.TBox.Plugins.SkyNet.Code
{
    class DataPacker : IDataPacker
    {
        private readonly ICopyDirGenerator copyDirGenerator;

        public DataPacker(ICopyDirGenerator copyDirGenerator)
        {
            this.copyDirGenerator = copyDirGenerator;
        }

        public string Pack(string path, string[] copyMasks, out string name)
        {
            var outputPath = Path.GetTempFileName();
            using (var zipFile = new ZipFile())
            {
                zipFile.CompressionLevel = CompressionLevel.BestCompression;
                string source;
                foreach (var dir in copyDirGenerator.GetFiles(path, copyMasks, out name, out source))
                {
                    var folder = dir.Key;
                    zipFile.AddFiles(
                        dir.Value.Select(x=>Path.Combine(source, folder, x)), folder);
                }
                zipFile.Save(outputPath);
            }
            return outputPath;
        }
    }
}
