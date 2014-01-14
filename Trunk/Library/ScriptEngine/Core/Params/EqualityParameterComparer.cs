using System.Collections.Generic;
using ScriptEngine.Core.Params;

namespace Mnk.Library.ScriptEngine.Core.Params
{
	public sealed class EqualityParameterComparer : IEqualityComparer<Parameter>
	{
		public bool Equals(Parameter x, Parameter y)
		{
			return string.Equals(x.Key, y.Key);
		}

		public int GetHashCode(Parameter obj)
		{
			return obj.Key.GetHashCode();
		}
	}
}
