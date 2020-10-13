using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VinmonopolQuery.Util
{
    internal class Logger
    {
        private static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.Write($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}: ");
            w.WriteLine($"{logMessage}");
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
            Console.WriteLine(message);

            using var wr = File.AppendText("log.txt");
            Logger.Log(message, wr);
        }
    }
}
