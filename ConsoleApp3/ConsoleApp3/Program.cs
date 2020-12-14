using System;
using System.Threading;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("--------CountBegin--------");
            var count = new Counter();

            var thread1 = new Thread(() => TestCounter(count));
            var thread2 = new Thread(() => TestCounter(count));
            var thread3 = new Thread(() => TestCounter(count));


            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread1.Join();
            thread2.Join();
            thread3.Join();

            Console.WriteLine($"Total Count:{count.Count}");
            Console.WriteLine("--------CountEnd--------");


            Console.WriteLine("--------WithLockCountBegin--------");

            var withCount = new CountWithNoLock();

            var withCount1 = new Thread(() => TestCounter(withCount));
            var withCount2 = new Thread(() => TestCounter(withCount));
            var withCount3 = new Thread(() => TestCounter(withCount));


            withCount1.Start();
            withCount2.Start();
            withCount3.Start();
            withCount1.Join();
            withCount2.Join();
            withCount3.Join();


            Console.WriteLine($"Total Count:{withCount.Count}");
            Console.WriteLine("--------WithLockCountEnd--------");

            Console.ReadKey();
        }

        static void TestCounter(CounterBase c)
        {

            for (int i = 0; i < 10000; i++)
            {
                c.Increment();
                c.Decrement();
            }

        }

      
    }

    class Counter : CounterBase
    {

        private int _count;

        public int Count => _count;


        public override void Decrement()
        {
            _count++;
        }

        public override void Increment()
        {
            _count--;
        }
    }



    class CountWithNoLock : CounterBase
    {
        private int _count;
        public int Count => _count;       

        public override void Decrement()
        {
            Interlocked.Decrement(ref _count);
        }

        public override void Increment()
        {
            Interlocked.Increment(ref _count);
        }
    }
    abstract class CounterBase
    {
        public abstract void Increment();
        public abstract void Decrement();

    }
}
