﻿using System.Collections.Generic;
using System.Linq;
using Mnk.Library.Common.Base;
using Mnk.Library.Common.Base.Log;
using Mnk.Library.Common.Data;

namespace Mnk.Library.Common.ItemsManagers
{
	public sealed class Ordered<T>
	{
		private readonly ILog log = LogManager.GetLogger<Ordered<T>>();
		private readonly IList<Pair<string, T>> objects = new List<Pair<string, T>>();

		public void Add(string name, T obj)
		{
			if (IsExist(name))
			{
				log.Write("Key '{0}' already exist!", name);
				return;
			}
			objects.Add(new Pair<string, T>(name.ToLower(), obj));
		}

		public bool IsExist(string name)
		{
			name = name.ToLower();
			return objects.Any(obj => string.Equals(name, obj.Key));
		}

		public int Count
		{
			get { return objects.Count; }
		}

		public T Get(string name)
		{
			name = name.ToLower();
			foreach (var obj in objects.Where(obj => string.Equals(name, obj.Key)))
			{
				return obj.Value;
			}
			return default(T);
		}

		public void Del(string name)
		{
			name = name.ToLower();
			foreach (var obj in objects.Where(obj => string.Equals(name, obj.Key)))
			{
				objects.Remove(obj);
				return;
			}
			log.Write("Key '{0}' is not finded, can't delete!", name);
		}

		public void Clear()
		{
			objects.Clear();
		}
	}
}
