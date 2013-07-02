using System;

namespace Interface.Atrributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class PluginDescriptionAttribute : ValueAttribute
	{
		public PluginDescriptionAttribute(string value) : base(value) { }
	}
}
