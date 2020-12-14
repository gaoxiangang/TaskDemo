using System;
using System.Threading;
using static System.Console;

namespace MutexDemo
{
    class Program
    {
        //设置一个互斥量
        const string MutexName = "CSharpThradCookBook";

        /// <summary>
        /// 同时只对一个线程授予访问权限
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            using (var m = new Mutex(false, MutexName))
            {
                //在设定的时间内等待互斥量是否被释放         
                if (!m.WaitOne(TimeSpan.FromSeconds(5), false))
                {
                    //未等代到互斥量说明已被释放 可以进行下一步同步
                    WriteLine("Second instrance Is  Running ");
                }
                else {
                    //等待到互斥量被释放  手动操作
                    WriteLine("The First  Is Running ");

                    ReadLine();
                    m.ReleaseMutex();
                }
            }
        }



    }
}
