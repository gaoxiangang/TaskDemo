using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ConcurrentDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConcurrentDictionaryClass concurrent = new ConcurrentDictionaryClass();
            //concurrent.Begion();
            Task task = null;
            //Task task = ConcurrentQueueClass.RunProgram();


            //task = ConcurrentStackClass.RunProgram();


            //ConcurrentBagClass.CreateLinks();
            //task = ConcurrentBagClass.RunProgram();

            task = BLockingCollectionClass.RunProgram();
            Console.WriteLine();
            task.Wait();

            task = BLockingCollectionClass.RunProgram(new ConcurrentStack<CustomTask>());
            Console.WriteLine();

            task.Wait();

            Console.ReadKey();

        }
    }
}
