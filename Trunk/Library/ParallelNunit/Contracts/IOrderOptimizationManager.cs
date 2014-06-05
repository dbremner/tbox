﻿using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IOrderOptimizationManager
    {
        IList<Result> Optimize(string path, IList<Result> tests);
        void SaveStatistic(string path, IList<Result> tests);
    }
}