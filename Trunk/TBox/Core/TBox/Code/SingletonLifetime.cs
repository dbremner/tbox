using System;
using LightInject;

namespace Mnk.TBox.Core.Application.Code
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
