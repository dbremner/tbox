using System.Collections.Generic;
using System.Windows.Media;

namespace Mnk.TBox.Plugins.NUnitRunner.Code
{
    // fix for multithread issue
    static class CachedIcons
    {
        public  static IDictionary<string, ImageSource> Icons { get; private set; }

        static CachedIcons()
        {
            Icons = new Dictionary<string, ImageSource>();
        }
    }
}
