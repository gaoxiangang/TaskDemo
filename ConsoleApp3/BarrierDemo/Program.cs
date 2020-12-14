using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using static System.Threading.Thread;

namespace BarrierDemo
{
    class Program
    {
        static Barrier barrier = new Barrier(2,b=> {
            WriteLine("到这里就结束了 弟弟");
        });


        static void Play() {
            WriteLine("1");
            WriteLine("2");
            WriteLine("3");
            WriteLine("4");
            barrier.SignalAndWait();
            
        }
        static void Main(string[] args)
        {

            var t1 = new Thread(() => Play());
            var t2 = new Thread(() => Play());
            t1.Start();
            t2.Start();
            ReadLine();
        }
    }
}
