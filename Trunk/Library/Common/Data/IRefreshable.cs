using System;

namespace Mnk.Library.Common.Data
{
    public interface IRefreshable
    {
        event Action OnRefresh;
        void Refresh();
    }
}
