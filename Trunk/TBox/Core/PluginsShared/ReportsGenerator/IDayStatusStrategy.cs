using System;
using System.Collections.Generic;

namespace PluginsShared.ReportsGenerator
{
    public interface IDayStatusStrategy
    {
        string GetState(DateTime day, Dictionary<string, Dictionary<string, double>> values, int workHours, IDayTypeProvider dayTypeProvider);
    }
}
