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
				    return JsonSerializer.DeserializeFromString(File.ReadAllText(configPath), type);
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
				File.WriteAllText(configPath, JsonSerializer.SerializeToString(data, type));
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
