using System;

namespace Common.Base.Log
{
    public class ConsoleLog : AbstractLog
    {
        public override void Write(string value)
        {
            var oldClr = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(value);
            System.Console.ForegroundColor = oldClr;
        }
    }
}
