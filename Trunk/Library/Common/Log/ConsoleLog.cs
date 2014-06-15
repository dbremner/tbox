using System;
using System.Globalization;
using System.Linq;

namespace Mnk.Library.Common.Log
{
    public class ConsoleLog : AbstractLog
    {
        public override void Write(string value)
        {
            var oldClr = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ForegroundColor = oldClr;
        }

        public override void Write(Exception ex, string value)
        {
            var aex = ex as AggregateException;
            if (aex != null && aex.InnerExceptions != null && aex.InnerExceptions.Any())
            {
                ex = aex.InnerExceptions.First();
            }
            Write(string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", value, Environment.NewLine, ex.Message));
        }
    }
}
