using System;

namespace Common.Data
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
}
