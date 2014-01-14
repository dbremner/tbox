using System;
using System.Globalization;

namespace Mnk.Library.Common.Tools
{
	public class Converter
	{
		private readonly long groupDivideNumber;
		private readonly string groupDivider;

		public Converter(long groupDivideNumber, string groupDivider)
		{
			this.groupDivideNumber = groupDivideNumber;
			this.groupDivider = groupDivider;
		}

		public string FormatDigitsGroups(long value)
		{
			if (value < groupDivideNumber)
			{
				return value.ToString(CultureInfo.InvariantCulture);
			}
			var ret = string.Empty;
			do
			{
				var digits = value%groupDivideNumber;
				ret = digits + groupDivider + ret;
				value /= groupDivideNumber;
			} while (value > 0);
			return ret;
		}

		public int GetGroupsCount(int value)
		{
			return (int)Math.Ceiling( Math.Pow(value, 1.0/groupDivideNumber) );
		}

	}
}
