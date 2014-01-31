using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ZetaLongPaths;
using ZetaLongPaths.Native;

namespace Mnk.TBox.Plugins.Searcher.Code
{
    class Filter
    {
        private readonly Regex[] filter;
        public Filter(IEnumerable<string> fileMasksToExclude)
        {
            filter = fileMasksToExclude
                .Select(x => new Regex(x.Replace(".", "[.]").Replace("*", ".*").Replace("?", ".").Replace("\\", "\\\\"), RegexOptions.Compiled))
                .ToArray();
        }

        public bool CheckAttribute(FileAttributes a)
        {
            return !a.HasFlag(FileAttributes.Hidden) &&
                !a.HasFlag(FileAttributes.Readonly) &&
                !a.HasFlag(FileAttributes.System);
        }

        public bool CanInclude(ZlpFileInfo info)
        {
            return CheckAttribute(info.Attributes) && !filter.Any(x => x.IsMatch(info.FullName));
        }
    }
}
