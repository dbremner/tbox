using System;
using LightInject;

namespace TBox.Code
{
    class SingletonLifetime : ILifetime
    {
        private readonly object locker = new object();
        private object instance = null;

        public object GetInstance(Func<object> createInstance, Scope scope)
        {
            lock (locker)
            {
                if (instance == null)
                    return instance = createInstance();
                return instance;
            }
        }
    }
}
