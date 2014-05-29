namespace Mnk.Library.WpfControls.Code.Content
{
	public sealed class StringDataManager : IDataManager
	{
		public object Create(string key)
		{
			return key;
		}

		public object Clone(object sample, string key)
		{
			return key;
		}

		public object ChangeKey(object target, string newKey)
		{
			return newKey;
		}

		public string GetKey(object target)
		{
			return target.ToString();
		}
	}
}
