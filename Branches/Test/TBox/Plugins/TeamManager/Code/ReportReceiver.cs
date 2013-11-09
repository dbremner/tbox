﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.MT;
using Common.Tools;
using PluginsShared.ReportsGenerator;
using ScriptEngine;
using TeamManager.Code.Scripts;

namespace TeamManager.Code
{
    public class ReportReceiver
    {
        private readonly string readOnlyDataPath;
        public ReportReceiver(string readOnlyDataPath)
        {
            this.readOnlyDataPath = readOnlyDataPath;
        }

        internal FullReport GetTimeReport(DateTime dateFrom, DateTime dateTo, IList<string> persons, IList<Operation> operations, IUpdater u)
        {
            var sw = new Stopwatch();
            sw.Start();
            dateFrom = Normalize(dateFrom);
            dateTo = Normalize(dateTo);
            var i = 0;
            var results = new List<LoggedInfo>();
            var runner = new ReportScriptRunner();
            var columns = new Dictionary<string, ColumnInfo>();
            var folder = readOnlyDataPath;
            Parallel.ForEach(operations, o =>
            {
                var context = new ReportScriptContext(dateFrom, dateTo, persons, columns)
                {
                    Name = o.Key
                };
                foreach (var path in o.Pathes.CheckedItems)
                {
                    if (u.UserPressClose) break;
                    runner.Run(Path.Combine(folder, path.Key), context, o.Parameters);
                }
                lock (results)
                {
                    results.AddRange(context.Report);
                }
                u.Update(o.Key, ++i / (float)operations.Count);
            });
            return new FullReport
                {
                    From = dateFrom,
                    To = dateTo,
                    Emails = persons,
                    Time = (int)sw.ElapsedMilliseconds/1000,
                    Columns = operations.Select(x=>new KeyValuePair<string, ColumnInfo>(x.Key, columns[x.Key])).ToArray(),
                    Items = results
                        .Where(x => Normalize(x.Date) >= dateFrom && Normalize(x.Date) <= dateTo)
                        .OrderBy(x => x.Column, new OrderComparer<string>(operations.Select(x => x.Key).ToArray()))
                        .ToArray()
                };
        }

        private static DateTime Normalize(DateTime date)
        {
            return date.AddHours(-date.Hour).AddMinutes(-date.Minute)
                .AddSeconds(-date.Second).AddMilliseconds(-date.Millisecond);
        }

    }
}