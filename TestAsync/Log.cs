using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Log
    {
        public static void Add(string s) {
            Console.WriteLine("{0} -  {1}", s, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
