using System;
using System.IO;
using Common.Base.Log;
using ServiceStack.Text;

namespace Common.SaveLoad
{
	public sealed class AnySerializer
	{
		private static readonly ILog Log = LogManager.GetLogger<AnySerializer>();
		private readonly string configPath;
		private readonly Type type;

		public AnySerializer(string configPath, Type type)
		{
			this.configPath = configPath;
			this.type = type;
		}

		public object Load(object defValue = null)
		{
			if (File.Exists(configPath))
			{
				try
				{
                    using (var s = File.Open(configPath, FileMode.Open))
                    {
                        return JsonSerializer.DeserializeFromStream(type, s)??defValue;
                    }
				}
				catch (Exception ex)
				{
					Log.Write(ex, "Can't load configuration: " + configPath);
				}
			}
			else
			{
				if (defValue != null) Save(defValue);
			}
			return defValue;
		}

		public bool Save(object data)
		{
			try
			{
                using (var s = File.Open(configPath, FileMode.Create))
                {
                    JsonSerializer.SerializeToStream(data, type, s);
                }
				return true;
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't save configuration: " + configPath);
				return false;
			}
		}
	}
}
