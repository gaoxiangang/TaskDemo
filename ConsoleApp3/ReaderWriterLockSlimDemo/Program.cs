using System;
using System.Collections.Generic;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;


namespace ReaderWriterLockSlimDemo
{
    class Program
    {
        static ReaderWriterLockSlim _rm = new ReaderWriterLockSlim();

        static Dictionary<int, int> _items = new Dictionary<int, int>();


        static void Read()
        {


            WriteLine("Reading  content  of  a Dictionary");
            while (true)
            {
                try
                {
                    _rm.EnterReadLock();
                    foreach (var key in _items.Keys)
                    {
                        Sleep(TimeSpan.FromSeconds(0.1));
                    }
                }
                finally
                {
                    _rm.ExitReadLock();
                }
            }
        }

        static void Write(string threadName)
        {
            while (true)
            {
                try
                {
                    int newKey = new Random().Next(250);
                    _rm.EnterUpgradeableReadLock();//获取读锁 如果需要写锁操作 就可以更新为写锁  然后释放
                    if (!_items.ContainsKey(newKey))
                    {
                        try
                        {
                            _rm.EnterWriteLock();
                            _items[newKey] = 1;
                            WriteLine($"new  key{newKey} is added to a dictionary by a {threadName}");
                        }
                        finally
                        {
                            _rm.ExitWriteLock();
                        }
                        Sleep(TimeSpan.FromSeconds(0.1));
                    }
                }
                finally
                {
                    _rm.ExitUpgradeableReadLock();
                }
            }
        }

        //ReaderWriterLockSlim  代表一个管理资源访问的锁，允许多个线程同时读取 以及独占访问
        static void Main(string[] args)
        {

            new Thread(Read) { IsBackground = true }.Start();
            new Thread(Read) { IsBackground = true }.Start();
            new Thread(Read) { IsBackground = true }.Start();

            new Thread(() => Write("Thread 1")) { IsBackground = true }.Start();
            new Thread(() => Write("Thread 2")) { IsBackground = true }.Start();

            Sleep(TimeSpan.FromSeconds(30));

            ReadLine();
        }
    }
}
