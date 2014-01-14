using System;
using System.Collections.Generic;
using System.Linq;
using ScriptEngine.Core.Params;

namespace Mnk.Library.ScriptEngine.Core.Params
{
	sealed class Factory
	{
		public Parameter Create(ParameterAttribute attribute, string name, Type propertyType)
		{
			if (attribute == null)
			{
				attribute = GetAttributeForType(propertyType);
			}
			else
			{
				if (!CheckInterface(propertyType, attribute.GetTypeForAttribute()))
				{
					throw new ArgumentException("Attribute is invalid: " + name);
				}
			}
			var p = attribute.CreateParameter();
			p.Key = name;
			return p;
		}

		private static ParameterAttribute GetAttributeForType(Type type)
		{
			if (type == typeof(bool)) return new BoolAttribute();
			if (type == typeof(string)) return new StringAttribute();
			if (type == typeof(Guid))return new GuidAttribute();
			if (type == typeof(int))return new IntegerAttribute();
			if (type == typeof (double)) return new DoubleAttribute();
			if (type == typeof(IList<string>)) return new StringListAttribute();
			if (type == typeof(IList<Guid>)) return new GuidListAttribute();
			if (type == typeof(IList<int>)) return new IntegerListAttribute();
			if (type == typeof(IList<double>)) return new DoubleListAttribute();
			if (type == typeof(IDictionary<string, string>)) return new StringDictionaryAttribute();
			if (type == typeof(IDictionary<string, bool>)) return new BoolDictionaryAttribute();
			if (type == typeof(IDictionary<string, Guid>)) return new GuidDictionaryAttribute();
			if (type == typeof(IDictionary<string, int>)) return new IntegerDictionaryAttribute();
			if (type == typeof(IDictionary<string, double>)) return new DoubleDictionaryAttribute();
			throw new ArgumentException("Unknown type: " + type);
		}

		private static bool CheckInterface(Type a, Type b)
		{
			return a == b || a.GetInterfaces().Contains(b);
		}
	}
}
