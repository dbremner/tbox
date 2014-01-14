using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using NUnit.Core;
using Mnk.TBox.Plugins.NUnitRunner.Properties;
using Mnk.Library.WPFWinForms.Icons;

namespace Mnk.TBox.Plugins.NUnitRunner.Components
{
	[ValueConversion(typeof(ResultState), typeof(string))]
	class ResultStateToIconConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return GetIcon(value).ToImageSource();
		}

		private static Icon GetIcon(object value)
		{
			switch ((ResultState) value)
			{
				case ResultState.Error:
					return Resources.Failure;
				case ResultState.Cancelled:
				case ResultState.NotRunnable:
					return Resources.Skipped;
			}
			return (Icon)Resources.ResourceManager.GetObject(Enum.GetName(typeof(ResultState), value));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
