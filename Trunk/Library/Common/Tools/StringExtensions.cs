using System;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Mnk.Library.Common.Tools
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string left, string right)
        {
            return String.Equals(left, right, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool EqualsIgnoreCaseAndTrim(this string left, string right)
        {
            return String.Equals((left ?? string.Empty).Trim(), (right ?? string.Empty).Trim(), StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsUniqueIgnoreCase<T>(this IEnumerable<T> collection, Func<T, string> getter, string value)
        {
            return collection.All(x => !getter(x).EqualsIgnoreCase(value));
        }

        public static T GetExistByKeyIgnoreCase<T>(this IEnumerable<T> collection, Func<T, string> getter, string key)
        {
            return collection.FirstOrDefault(item => getter(item).EqualsIgnoreCase(key));
        }

        public static void MergeIgnoreCase(this ICollection<string> collection, IList<string> existValues)
        {
            foreach (var value in existValues
                .Where(value => !string.IsNullOrWhiteSpace(value) && collection.GetExistByKeyIgnoreCase(x => x, value) == null))
            {
                collection.Add(value);
            }
            foreach (var value in collection
                .Where(value => collection.GetExistByKeyIgnoreCase(x => x, value) == null)
                .ToArray())
            {
                collection.Remove(value);
            }
        }

        public static StringBuilder AppendLineIfNeed(this StringBuilder sb, string text)
        {
            if (sb.Length != 0)
                sb.AppendLine();
            return sb.Append(text);
        }

        public static StringBuilder PrintTable(this StringBuilder sb, IList<IList<string>> table)
        {
            if (table.Count == 0 || table[0].Count == 0) return sb;
            var widthes = table[0].Select(x => x.Length).ToArray();
            foreach (var row in table)
            {
                for (var i = 0; i < widthes.Length && i < row.Count; ++i)
                {
                    if (widthes[i] < row[i].Length)
                    {
                        widthes[i] = row[i].Length;
                    }
                }
            }
            foreach (var row in table)
            {
                sb.AppendLine().Append("|");
                for (var i = 0; i < widthes.Length && i < row.Count; ++i)
                {
                    sb.Append(NormalizeLength(row[i], widthes[i])).Append("|");
                }
            }
            return sb;
        }

        public static StringBuilder PrintHtmlTable(this StringBuilder sb, IList<IList<string>> table, IList<string> styles)
        {
            if (table.Count == 0 || table[0].Count == 0) return sb;
            var cols = table[0].Max(x => x.Length);
            sb.Append("<table>");
            var id = 0;
            foreach (var row in table)
            {
                sb.Append("<tr");
                var style = styles[id++];
                if (!string.IsNullOrEmpty(style)) sb.AppendFormat(" class='{0}' ", style);
                sb.Append(">");
                for (var i = 0; i < cols && i < row.Count; ++i)
                {
                    sb.Append("<td>").Append(row[i] ?? "-").Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb;
        }

        private static string NormalizeLength(string s, int length)
        {
            return (s.Length < length) ? s.PadRight(length) : s;
        }

        public static bool IsUniqueIgnoreCase<T>(this ICollection<T> collection, string text)
            where T : UI.Model.Data
        {
            return collection.IsUniqueIgnoreCase(x => x.Key, text);
        }

        public static T GetExistByKeyIgnoreCase<T>(this ICollection<T> collection, string key)
            where T : UI.Model.Data
        {
            return collection.GetExistByKeyIgnoreCase(x => x.Key, key);
        }

        public static int GetExistIndexByKeyIgnoreCase<T>(this Collection<T> collection, string key)
            where T : UI.Model.Data
        {
            for (var i = 0; i < collection.Count; ++i)
            {
                var item = collection[i];
                if (key.EqualsIgnoreCase(item.Key))
                {
                    return i;
                }
            }
            return -1;
        }

        public static string FormatTimeInSec(this int time)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:00}:{1:00}:{2:00}", time / 3600, (time / 60) % 60, time % 60);
        }
    }
}
