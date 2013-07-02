using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Common.Data;

namespace WFControls.Components.DataManagers
{
    public interface IDataMan
    {
        void Add(string name);
        void Del(int id);
        void Change(int id, string name);
        void Clear();
        string[] GetValues();
    }

    [Serializable]
    public class SimpleDataMan : IDataMan, IEnumerable<string>, ICloneable
    {
        protected List<string> MValues = new List<string>();
        public List<string> Values { get { return MValues; } }
        public string this[int id]
        {
            get { return MValues[id]; }
            set { MValues[id] = value; }
        }
        public int Length { get { return MValues.Count; } }

        public void Add(string name)
        {
            MValues.Add(name);
        }
        public void Del(int id)
        {
            MValues.RemoveAt(id);
        }
        public void Change(int id, string name)
        {
            MValues[id] = name;
        }
        public void Clear()
        {
            MValues.Clear();
        }
        public string[] GetValues()
        {
            if (MValues.Count > 0)
            {
                var ret = new string[MValues.Count];
                for (int i = 0; i < MValues.Count; i++)
                {
                    ret[i] = MValues[i];
                }
                return ret;
            }
            return null;
        }
        public void Add(object o)
        {
            MValues.Add((string)o);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return MValues.GetEnumerator();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)MValues).GetEnumerator();
        }

        public object Clone()
        {
            var dm = new SimpleDataMan();
            foreach (var value in MValues)
            {
                dm.MValues.Add(value);
            }
            return dm;
        }
    }

    [Serializable]
    public class DataMan<T> : IDataMan, IEnumerable<Pair<string, T>>, ICloneable
        where T : new()
    {
        protected List<Pair<string, T>> MValues = new List<Pair<string, T>>();
        public List<Pair<string, T>> Values { get { return MValues; } }

        public void Add(string name)
        {
            MValues.Add(new Pair<string, T>(name, new T()));
        }
        public void Del(int id)
        {
            MValues.RemoveAt(id);
        }
        public void Change(int id, string name)
        {
            MValues[id].Key = name;
        }
        public void Clear()
        {
            MValues.Clear();
        }
        public string[] GetValues()
        {
            if (MValues.Count > 0)
            {
                var ret = new string[MValues.Count];
                for (int i = 0; i < MValues.Count; i++)
                {
                    ret[i] = MValues[i].Key;
                }
                return ret;
            }
            return null;
        }
        public void Add(object o)
        {
            MValues.Add((Pair<string, T>)o);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return MValues.Select(v => v.Value).GetEnumerator();
        }

        public IEnumerator<Pair<string, T>> GetEnumerator()
        {
            return ((IEnumerable<Pair<string, T>>)MValues).GetEnumerator();
        }

        public object Clone()
        {
            var dm = new DataMan<T>();
            foreach (var value in MValues)
            {
                dm.MValues.Add(new Pair<string, T>(value.Key, (T)((ICloneable)value.Value).Clone()));
            }
            return dm;
        }
    }
}
