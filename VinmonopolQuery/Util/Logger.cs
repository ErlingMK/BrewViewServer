using System;
using System.IO;

namespace VinmonopolQuery.Util
{
    internal class Logger
    {
        private static void Log(string logMessage, TextWriter w)
        {
            w.Write("\nLog Entry : ");
            w.Write($"{DateTime.Now:dd/MM/yyyy HH:mm}: ");
            w.Write($"{logMessage}");
        }

        private static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        public static void Log(string message)
        {
#if DEBUG
            Console.WriteLine(message);
#endif

            using var wr = File.AppendText(@"C:\Users\erlin\source\repos\BrewViewServer\VinmonopolQuery\log.log");
            Log(message, wr);
        }
    }
}
