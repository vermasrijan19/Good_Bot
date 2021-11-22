using System;
using System.IO;
using System.Linq;

namespace Good_Bot
{
    public static class Currency
    {
        public static void Accounting(ulong id, int bal = 0)
        {
            // This will create a file named sample.txt 
            // at the specified location  
            Console.WriteLine(id);
            var sw = new StreamWriter(id + ".txt");
            // To write on the console screen 
            sw.WriteLine(bal);
            // To write in output stream 
            sw.Flush();
            // To close the stream 
            sw.Close();
        }

        public static int get_Balance(in ulong id)
        {
            // var streamReader = new StreamReader(id + ".txt");
            var line1 = File.ReadLines(id + ".txt").First();
            Console.WriteLine(line1);
            return int.Parse(line1);
        }
    }
}