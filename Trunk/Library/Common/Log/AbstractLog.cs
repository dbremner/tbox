using System;
using System.Globalization;
using System.Linq;

namespace Mnk.Library.Common.Log
{
    public abstract class AbstractLog : ILog
    {
        public abstract void Write(string value);
        public virtual void Write(Exception ex, string value)
        {
            var aex = ex as AggregateException;
            if (aex != null && aex.InnerExceptions != null && aex.InnerExceptions.Any())
            {
                ex = aex.InnerExceptions.First();
            }
            Write(string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", value, Environment.NewLine, ex));
        }

        public void Write(string format, params object[] args)
        {
            Write(string.Format(CultureInfo.InvariantCulture, format, args));
        }

        public void Write(Exception ex, string format, params object[] args)
        {
            Write(ex, string.Format(CultureInfo.InvariantCulture, format, args));
        }
    }
}
