using System.Collections.Generic;

namespace Mnk.Library.Common.Log
{
	public sealed class MemoryLog : AbstractLog
	{
		private readonly List<string> data = new List<string>();
		public IList<string> Data { get { return data; } }

		public override void Write(string value)
		{
			if (data.IndexOf(value) == -1)
			{
				data.Add(value);
			}
		}

		public void Copy(AbstractLog log)
		{
			foreach (var x in data)
			{
				log.Write(x);
			}
		}

		public void Clear()
		{
			data.Clear();
		}
	}
}
