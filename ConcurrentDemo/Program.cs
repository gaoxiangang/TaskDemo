using System;
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


            task = ConcurrentStackClass.RunProgram();

            task.Wait();

            Console.ReadKey();

        }
    }
}
