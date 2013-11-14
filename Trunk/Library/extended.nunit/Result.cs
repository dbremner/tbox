using System;
using NUnit.Core;

namespace extended.nunit
{
	[Serializable]
	public class Result
	{
		public int Id { get; set; }
		public string Key { get; set; }
		public string Message { get; set; }
		public string StackTrace { get; set; }
		public ResultState State { get; set; }
        public string[] Categories { get; set; }
	}
}
