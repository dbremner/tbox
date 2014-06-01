using System.Collections.Generic;
using System.Linq;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit
{
    public static class Extensions
    {
        public static IEnumerable<Result> Collect(this Result result)
        {
            foreach (var r in result.Children.Cast<Result>().SelectMany(Collect))
            {
                yield return r;
            }
            yield return result;
        }

    }
}
