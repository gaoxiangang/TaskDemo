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
