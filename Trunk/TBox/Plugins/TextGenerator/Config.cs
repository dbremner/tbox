using System;

namespace TextGenerator
{
	[Serializable]
	public class Config
	{
		public string Fill { get; set; }

		public Config()
		{
			Fill = " ";
		}
	}
}
