using System;

namespace Mnk.Library.Common.UI.Model
{
    [Serializable]
    public class Data : IData, ICloneable
    {
        public string Key { get; set; }
        public override string ToString()
        {
            return Key;
        }
        public virtual object Clone()
        {
            return new Data { Key = Key };
        }
    }

    [Serializable]
    public class Data<T> : Data
    {
        public T Value { get; set; }
        public override object Clone()
        {
            return new Data<T> { Key = Key, Value = Value };
        }
    }
}
