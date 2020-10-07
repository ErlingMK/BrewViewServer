using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VinmonopolQuery.Util
{
    internal class Logger
    {
        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.Write($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}: ");
            w.WriteLine($"{logMessage}");
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}
