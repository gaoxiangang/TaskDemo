using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static System.Threading.Thread;

namespace Reactive_ExtensionsDemo
{

    /// <summary>
    /// c创建可观察的对象
    /// </summary>
   public  class Rx_Recipe5
    {

        /************
         * 这里大部分功能是使用Observable类型的静态工厂方法实现的
         * 前两个例子展示如何使用有值和无值创建一个Observable 类型
         * 接下来用Throw来构造一个Observable类，来调用OnError对象
         * Repate代表一个无穷尽的序列。该方法有多个重载我们通过重复值42构造一无尽的序列
         * 然后使用linq的Task方法取出5个值
         * range表示一组值，与IEnumerable的Range很像
         * Create支持自定义场景，有相当多的重载允许我们使用取消标志和任务，首先最简单的，接受一个函数，该函数接受一个观察者实例。
         * 并且返回IDisposable对象代表订阅者。如需清理任何资源，可以放置清除逻辑，但本实例返回一个空的IDsposable 因为不需要
         * ***/
        public static void RunMain() {

            IObservable<int> o =  Observable.Return(0);
            using (var sub = OutputToConsole(o))
            WriteLine("---------------");

            o = Observable.Empty<int>();
            using (var sub = OutputToConsole(o))
            WriteLine("---------------");

            o = Observable.Throw<int>( new Exception());
            using (var sub = OutputToConsole(o))
                WriteLine("---------------");

            o = Observable.Repeat(42);
            using (var sub = OutputToConsole(o.Take(5)))
                WriteLine("---------------");

            o = Observable.Range(0,10);
            using (var sub = OutputToConsole(o))
                WriteLine("---------------");

            o = Observable.Create<int>(ob =>
            {
                for (int i = 0; i < 10; i++)
                {
                    ob.OnNext(i);
                }
                return Disposable.Empty;
            });

            using (var sub=OutputToConsole(o));

            WriteLine("----------------");
            
        }

        public static IDisposable OutputToConsole<T>(IObservable<T> sequence)
        {
            return sequence.Subscribe(
                obj => WriteLine($"{obj}"),
                ex=> WriteLine($"Error: {0}",ex.Message),
                ()=>WriteLine("Complete")                
            );
        }
    }
}
