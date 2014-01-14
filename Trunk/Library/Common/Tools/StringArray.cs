using System;
using System.Linq;

namespace Mnk.Library.Common.Tools
{
	public class StringArray<T>
	{

		private readonly string divider;
		private readonly Func<string, T> convert;
		public StringArray(string divider, Func<string, T> convert)
		{
			this.divider = divider;
			this.convert = convert;
		}

		public T[] FromLine(string text)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				var lines = text.Split(new[] { divider }, StringSplitOptions.RemoveEmptyEntries);
				if (lines.Length != 0)
				{
					return lines.Select(x => convert(x)).ToArray();
				}
			}
			return new T[0];
		}

		public string ToLine(T[] items)
		{
			return items == null ? string.Empty : string.Join(divider, items);
		}
	}
}
