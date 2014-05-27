using System;

namespace Mnk.Library.Common.Models
{
    public interface IRefreshable
    {
        event Action OnRefresh;
        void Refresh();
    }
}
