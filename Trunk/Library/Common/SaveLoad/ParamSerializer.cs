namespace Mnk.Library.Common.SaveLoad
{
	public sealed class ParamSerializer<T>
	{
		private readonly AnySerializer serializer;

		public ParamSerializer(string configFilePath)
		{
			serializer = new AnySerializer(configFilePath, typeof(T));
		}

		public T Load(T defValue=default(T))
		{
			return (T)serializer.Load(defValue);
		}

		public bool Save(T data)
		{
			return serializer.Save(data);
		}
	}
}
