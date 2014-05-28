using System.Collections.Generic;

namespace Mnk.Library.Common.Tools
{
    public interface ICopyDirGenerator
    {
        IDictionary<string, IList<string>> GetFiles(string path, string[] copyMasks, out string name, out string sourceDir);
        string[] NormalizePaths(IEnumerable<string> paths);
        int CalcCopyDeep(IEnumerable<string> paths);
    }
}