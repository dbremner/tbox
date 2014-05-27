using System;

namespace Mnk.Library.Common.Models
{
	[Serializable]
	public sealed class Pair<TKey, TValue>
	{
		public TKey Key { get; set; }
		public TValue Value { get; set; }

		public Pair(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		public Pair()
			: this(default(TKey), default(TValue))
		{

		}
	}

	public static class Pair
	{
		public static Pair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
		{
			return new Pair<TKey, TValue>(key, value);
		}
	}
}
