using System.Collections.Generic;

namespace Mnk.Library.Common.Data
{
    public interface IHasChildren
    {
        IList<IHasChildren> Children { get; }
    }
}
