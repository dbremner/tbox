using System;
using System.Linq;
using System.Reflection;

namespace RunAsx86
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var assembly = Assembly.LoadFile(args[0]);

                var type = assembly.GetTypes().FirstOrDefault(t => GetMethod(t) != null);
                if (type == null)
                {
                    Console.WriteLine("Can't find method Main");
                    return;
                }
                var mi = GetMethod(type);

                var arguments = args.Skip(1).Where(a => a.Trim() != "").ToArray();
                var r = mi.Invoke(type, new object[] {arguments});
                Console.WriteLine(r);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private static MethodInfo GetMethod(Type t)
        {
            return t.GetMethod("Main", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        }
    }
}
