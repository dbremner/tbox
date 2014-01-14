using System;
using System.Collections.Generic;
using System.Linq;
using Mnk.TBox.Core.PluginsShared.ReportsGenerator;

namespace TeamManager.Code.Reports
{
    public class PmcDayStatusStrategy : IDayStatusStrategy
    {
        private const string Type = "PMC";
        private readonly string[] notWork = new[] { "EXV", "ILL", "OVT", "POV", "VAC", "EDU" };

        public string GetState(DateTime day, Dictionary<string, Dictionary<string, double>> values, int workHours, IDayTypeProvider dayTypeProvider)
        {
            var workTime = ProcessNotWorkActivities(values, workHours);
            var sums = values.Select(item => item.Value.Sum(x => x.Value)).ToArray();
            if (sums.Any(sum => NotEquals(sum, workTime)))
            {
                var first = sums.First();
                if (sums.Any(x => NotEquals(first, x))) return DayTypes.Error;
                return GetState(day, dayTypeProvider);
            }
            return values.Count == 0 ? GetState(day, dayTypeProvider) : DayTypes.Working;
        }

        private double ProcessNotWorkActivities(Dictionary<string, Dictionary<string, double>> values, double workHours)
        {
            var time = workHours;
            Dictionary<string, double> value;
            if (!values.TryGetValue(Type, out value)) return workHours;
            foreach (var info in value.ToDictionary(x=>x.Key, x=>x.Value))
            {
                if (!notWork.Contains(info.Key)) continue;
                value.Remove(info.Key);
                time -= info.Value;
            }
            return time;
        }

        private static bool NotEquals(double a, double b)
        {
            return Math.Abs(a-b) > 0.000001;
        }

        private static string GetState(DateTime day, IDayTypeProvider dayTypeProvider)
        {
            return dayTypeProvider.IsHoliday(day) ? DayTypes.Holyday : DayTypes.Error;
        }
    }
}
