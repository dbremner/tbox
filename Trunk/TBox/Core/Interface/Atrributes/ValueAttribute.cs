using System;

namespace Interface.Atrributes
{
	public abstract class ValueAttribute : Attribute
	{
		public string Value { get; private set; }
		protected ValueAttribute(string value)
		{
			Value = value;
		}
	}
}
