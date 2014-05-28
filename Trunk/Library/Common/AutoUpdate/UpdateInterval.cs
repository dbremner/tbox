using System;

namespace Mnk.Library.Common.AutoUpdate
{
    [Serializable]
    public enum UpdateInterval
    {
        Startup,
        Daily,
        Weekly,
        Monthly,
        Never
    }
}
