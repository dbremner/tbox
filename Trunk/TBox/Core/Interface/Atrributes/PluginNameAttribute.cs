using System;

namespace Interface.Atrributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class PluginNameAttribute : ValueAttribute
	{
		public PluginNameAttribute(string value) : base(value) { }
	}
}
