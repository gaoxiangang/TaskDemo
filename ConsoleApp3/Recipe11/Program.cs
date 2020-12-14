using System;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace Recipe11
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Thread(FaultyThread);
            t.Start();
            t.Join();

            try
            {
                var y = new Thread(BadFaultyThread);
                y.Start();

            }
            catch (Exception ex)
            {

                WriteLine(" 我们没获取到任何东西！");
            }
        }

        static void BadFaultyThread()
        {

            WriteLine("启动一个错误的线程");
            Sleep(TimeSpan.FromSeconds(2));
            throw new Exception("Boom!");
        }

        static void FaultyThread()
        {

            try
            {
                WriteLine("开启一个错误线程");
                Sleep(TimeSpan.FromSeconds(1));
                throw new Exception("Boom!");

            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);

            }
        }

    }
}
