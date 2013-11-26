using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Searcher.Code
{
	class Filter
	{
		private readonly Regex[] filter;
		public Filter(IEnumerable<string> fileMasksToExclude)
		{
			filter = fileMasksToExclude
                .Select(x => new Regex(x.Replace(".", "[.]").Replace("*", ".*").Replace("?", ".").Replace("\\", "\\\\"), RegexOptions.Compiled))
				.ToArray();
		}

		public bool CheckAttribute(FileAttributes a)
		{
			return !a.HasFlag(FileAttributes.Hidden) &&
				!a.HasFlag(FileAttributes.ReadOnly) &&
				!a.HasFlag(FileAttributes.System);
		}

		public bool CanInclude(FileInfo info)
		{
			return CheckAttribute(info.Attributes) && !filter.Any(x => x.IsMatch(info.FullName));
		}
	}
}
