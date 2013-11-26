using System;
using System.Collections.Generic;
using System.Linq;
using Common.Tools;
using PluginsShared.ReportsGenerator;
using TeamManager.Code.Settings;

namespace TeamManager.Code.Reports
{
    class DayTypeProvider : IDayTypeProvider
    {
        private readonly IList<SpecialDay> specialDays;

        public DayTypeProvider(IList<SpecialDay> specialDays)
        {
            this.specialDays = specialDays;
        }

        public bool IsHoliday(DateTime date)
        {
            var str = date.ToShortDateString();
            var special = specialDays.FirstOrDefault(x => x.Key.EqualsIgnoreCase(str));
            if (special != null) return special.IsHolyday;
            return date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
        }
    }
}
