using System;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace DeadLock
{
    class Program
    {
        static void Main(string[] args)
        {
            object lock1 = new object();
            object lock2 = new object();
            //new Thread(() => LockTooMuch(lock1, lock2)).Start();

            //lock (lock2)
            //{

            //    Thread.Sleep(1000);
            //    WriteLine("---------Montior TryEnter----------");
            //    if (Monitor.TryEnter(lock1, TimeSpan.FromSeconds(5)))
            //    {
            //        WriteLine("Acquired a protected resouce successfuly");
            //    }
            //    else
            //    {
            //        WriteLine("TimeOut acquiring a resouce!");

            //    }
            //}


            new Thread(() => LockTooMuch(lock1, lock2)).Start();

            WriteLine("---------------------------------");
            lock (lock2)
            {
                WriteLine("This  will be deadLock");
                Sleep(1000);
                lock (lock1)
                {
                    WriteLine("Acquired a protected resouce successfuly");
                }

            }
            Console.ReadKey();
        }

        static void LockTooMuch(object lock1, object lock2)
        {
            lock (lock1)
            {
                Sleep(1000);
                lock (lock2);
            }

        }
    }
}
