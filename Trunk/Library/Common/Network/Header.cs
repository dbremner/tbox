namespace Mnk.Library.Common.Network
{
	public class Header : IHeader
	{
		public string Key { get; set; }
		public string Value { get; set; }

		public Header(string key, string value)
		{
			Key = key;
			Value = value;
		}
	}
}
