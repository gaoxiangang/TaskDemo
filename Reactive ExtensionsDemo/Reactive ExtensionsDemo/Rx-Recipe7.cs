using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Timers;
using static System.Console;
using static System.Threading.Thread;
namespace Reactive_ExtensionsDemo
{
    public class Rx_Recipe7
    {

        public static void RunMain()
        {
            
           

          
        }

        public static async Task<T> AwaitOnObservable<T>(IObservable<T> observable)
        {
            T obj = await observable;
            WriteLine($"{obj}");
            return obj;
        }

        public static Task<string> LongRunningOperationTaskAsync(string name)
        {
            return Task.Run(() => LongRunningOperation(name));
        }

        public static IObservable<string> LongRunningOperationAsync(string name)
        {
            return Observable.Start(() => LongRunningOperation(name));
        }
        public static string LongRunningOperation(string name)
        {
            Sleep(TimeSpan.FromSeconds(2));
            return $"Task {name} is Complete .Thread id {CurrentThread.ManagedThreadId} ";
        }

        public static IDisposable OutputToConsole(IObservable<EventPattern<ElapsedEventArgs>> sequence)
        {
            return sequence.Subscribe(
                obj => WriteLine($"{obj}"),
                ex => WriteLine($"Error: {ex.Message}"),
                () => WriteLine($"Complete")
            );
        }
        public static IDisposable OutputToConsole<T>(IObservable<T> sequence)
        {
            return sequence.Subscribe(
                obj => WriteLine($"{obj}"),
                ex => WriteLine($"Error: {ex.Message}"),
                () => WriteLine($"Complete")
            );
        }

    }
}
