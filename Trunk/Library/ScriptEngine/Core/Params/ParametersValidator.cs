using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScriptEngine.Core.Params;

namespace Mnk.Library.ScriptEngine.Core.Params
{
	public static class ParametersValidator
	{
		public static bool ValidateValue(IBoolParameter p, bool value)
		{
			return true;
		}

		public static bool ValidateValue(IStringParameter p, string value)
		{
			return p.CanBeEmpty || !string.IsNullOrEmpty(value);
		}

		public static bool ValidateValue(IGuidParameter p, Guid value)
		{
			return true;
		}

		public static bool ValidateValue(IFileParameter p, string value)
		{
			return !p.ShouldExist || File.Exists(value);
		}

		public static bool ValidateValue(IDirectoryParameter p, string value)
		{
			return !p.ShouldExist || Directory.Exists(value);
		}

		public static bool ValidateValue(IIntegerParameter p, int value)
		{
			return value <= p.Max && value >= p.Min;
		}

		public static bool ValidateValue(IDoubleParameter p, double value)
		{
			return value <= p.Max && value >= p.Min;
		}

		public static IEnumerable<string> Validate(IEnumerable<Parameter> parameters)
		{
			return parameters.Where(p => !Validate(p)).Select(x => x.Key);
		}

		public static bool Validate(Parameter parameter)
		{
			return DoValidation((dynamic)parameter);
		}

		private static bool DoValidation<T>(Parameter<T> p)
		{
			return ValidateValue((dynamic)p, (dynamic)p.Value);
		}

		private static bool DoValidation<T>(ListParameter<T> p)
		{
			return p.Checked.All(x => ValidateValue((dynamic)p, (dynamic)x.Value));
		}

		private static bool DoValidation<T>(DictionaryParameter<T> p)
		{
			return p.Checked.All(x => ValidateValue((dynamic)p, (dynamic)x.Value));
		}
	}
}
