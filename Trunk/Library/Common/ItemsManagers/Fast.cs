using System;
using System.Collections.Generic;
using System.Linq;
using Common.Base;
using Common.Base.Log;

namespace Common.ItemsManagers
{
	public class Fast<T>
	{
		private readonly ILog log = LogManager.GetLogger<Fast<T>>();
		private readonly Dictionary<string, T> objects = new Dictionary<string, T>();

		public void Add(string name, T obj)
		{
			if (IsExist(name))
			{
				log.Write("Key '{0}' already exist!", name);
				return;
			}
			objects.Add(name.ToLower(), obj);
		}

		public bool IsExist(string name)
		{
			return objects.ContainsKey(name.ToLower());
		}

		public int Count
		{
			get { return objects.Count; }
		}

		public string[] GetNames()
		{
			return objects.Keys.ToArray();
		}

		public T Get(string name)
		{
			if (!IsExist(name))
			{
				return default(T);
			}
			return objects[name.ToLower()];
		}

		public void Del(string name)
		{
			if (!IsExist(name))
			{
				log.Write("Key '{0}' is not finded, can't delete!", name);
				return;
			}
			objects.Remove(name.ToLower());
		}

		public void Clear()
		{
			objects.Clear();
		}
	}
}
