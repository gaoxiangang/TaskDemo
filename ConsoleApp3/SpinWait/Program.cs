using System;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace SpinWaitDemo
{
    class Program
    {

        static volatile bool _isCommpleted = false;


        static void UserModelWait()
        {
            while (!_isCommpleted)
            {
                WriteLine("UserModel");
            }
            WriteLine();
            WriteLine("UserModel  Wait  is  Complete");
        }
        static void HybridSpinWait()
        {
            var s = new SpinWait();
            while (!_isCommpleted)
            {
                s.SpinOnce();
                WriteLine(s.NextSpinWillYield);
            }
            WriteLine("HybridSpinWait is  complete");

        }

        static void Main(string[] args)
        {

            var t1 = new Thread(UserModelWait);
            var t2 = new Thread(HybridSpinWait);

            WriteLine("Runing usermodel  waiting");
            t1.Start();
            Sleep(20);
            _isCommpleted = true;
            Sleep(TimeSpan.FromSeconds(1));
            _isCommpleted = false;
            WriteLine("Runing HybridSpinWait waiting");

            t2.Start();
            Sleep(5);

            _isCommpleted = true;

            ReadKey();

        }
    }
}
