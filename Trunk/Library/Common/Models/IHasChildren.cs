using System.Collections.Generic;

namespace Mnk.Library.Common.Models
{
    public interface IHasChildren
    {
        IList<IHasChildren> Children { get; }
    }
}
