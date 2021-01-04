using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using static System.Console;
using static System.Threading.Thread;

namespace Reactive_ExtensionsDemo
{
    public class Rx_Recipe3
    {
        /*********
         * CustomObserver 自定义观察操作方法 /CustomSquence自定义集合
         * 声明CustomObserver 然后声明两个不同的CustomSquence集合，一个可以正常运行 一个则会报错
         * 我们在CustomSquence中并未检测数据是否正确以此来引发错误    PS记得Crtl+F5运行程序
         * 第一个则是正常的观察者模式。未执行任何监测更新操作
         * 第二个则是异步的观察者集合 执行中会和第一个操作打印相同的数据
         * 第三个也是异步的观察者集合，不同的是该集合会因为 传入的null而导致执行错误
         * **/

        public static void RunMain()
        {
            var observer = new CustomObserver();
            var goodObservable = new CustomSquence(new[] { 1, 2, 3, 4, 5 });
            var badObservable = new CustomSquence(null);
            using (IDisposable subseription =goodObservable.Subscribe(observer))
            {

            }

            using (IDisposable subseription = goodObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observer))
            {
                Sleep(TimeSpan.FromSeconds(10));
                WriteLine("按任意键继续");
                ReadLine();
            }

            using (IDisposable subseription=badObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(observer))
            {
                Sleep(TimeSpan.FromSeconds(10));
                WriteLine("按任意键继续");
                ReadLine();
            }

        }
    }

    public class CustomObserver : IObserver<int>
    {

        public void OnNext(int value)
        {

            WriteLine($"下一个值是：{value} 线程ID是：{CurrentThread.ManagedThreadId}");

        }
        public void OnError(Exception error)
        {
            WriteLine($"Error:{error.Message}");
        }
        public void OnCompleted()
        {

            WriteLine("Completed");
        }
    }

    public class CustomSquence : IObservable<int>
    {

        private readonly IEnumerable<int> _numbers;

        public CustomSquence(IEnumerable<int> numbers)
        {
            _numbers = numbers;
        }
        public IDisposable Subscribe(IObserver<int> observer)
        {
            foreach (int number in _numbers)
            {
                observer.OnNext(number);
            }
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
