using System;

namespace PluginsShared.ReportsGenerator
{
    public interface IDayTypeProvider
    {
        bool IsHoliday(DateTime date);
    }
}
