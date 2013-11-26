using System;
using Common.UI.Model;
using NUnit.Core;

namespace PluginsShared.UnitTests.Settings
{
	[Serializable]
	public class Result : CheckableData
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public string StackTrace { get; set; }
		public ResultState State { get; set; }
        public string[] Categories { get; set; }
	}
}
