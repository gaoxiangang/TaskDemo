using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Timers;
using static System.Console;
using static System.Threading.Thread;

namespace Reactive_ExtensionsDemo
{
    class Program
    {

        delegate string AsyncDelegate(string name);
        /// <summary>i
        /// https://www.cnblogs.com/shanyou/p/3233894.html
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //foreach (int i in Power(2, 8))
            //{
            //    Console.WriteLine("{0} ", i);
            //}

            //Rx_Recipe2.RunMain();

            //Rx_Recipe3.RunMain();

            //Rx_Recipe4.RuMain();
            //Rx_Recipe5.RunMain();
            Rx_Recipe6.RunMain();

            IObservable<string> o = Rx_Recipe7.LongRunningOperationAsync("Task1");

            using (var sub = Rx_Recipe7.OutputToConsole(o))
            {
                Sleep(TimeSpan.FromSeconds(2));
            }

            WriteLine("----------------");

            Task<string> t = Rx_Recipe7.LongRunningOperationTaskAsync("Task2");
            using (var sub = Rx_Recipe7.OutputToConsole(t.ToObservable()))
            {
                Sleep(TimeSpan.FromSeconds(2));
            }

            WriteLine("----------------");
            AsyncDelegate asyncMethod = Rx_Recipe7.LongRunningOperation;

            Func<string, IObservable<string>> observableFactoyr = Observable.FromAsyncPattern<string, string>(asyncMethod.BeginInvoke, asyncMethod.EndInvoke);

            o = observableFactoyr("Task3");

            using (var sub = Rx_Recipe7.OutputToConsole(o))
            {
                Sleep(TimeSpan.FromSeconds(2));
            }
            WriteLine("----------------");



            using (var timer = new Timer(1000))
            {
                var ot = Observable.FromAsyncPattern<ElapsedEventHandler, 
                    ElapsedEventArgs>(
                    h => timer.Elapsed += h,
                             h =>timer.Elapsed  -= h );////这傻逼玩意 

                timer.Start();
                using (var sub = Rx_Recipe7.OutputToConsole(ot))
                {
                    Sleep(TimeSpan.FromSeconds(5));
                }
                WriteLine("----------------");
                timer.Stop();
            }
            Console.ReadKey();
        }

        //public static System.Collections.Generic.IEnumerable<int> Power(int number, int exponent)
        //{
        //    int result = 1;

        //    for (int i = 0; i < exponent; i++)
        //    {
        //        result = result * number;
        //        yield return result;
        //    }
        //}
    }
}
