using System;
using System.Threading.Tasks;
using static System.Threading.Thread;
namespace Recipe7
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<int> task;

            try
            {
                task = Task.Run(() => TaskMethod("Task1", 2));
                int result = task.GetAwaiter().GetResult();
                Console.WriteLine($"Result :{ result}");
                Console.ReadKey();
            }
            catch (Exception)
            {

                throw;
            }

        }

        static int TaskMethod(string name, int seconds)
        {

            Console.WriteLine($"线程{name}是在线程{CurrentThread.ManagedThreadId},是否是在线程池:{CurrentThread.IsThreadPoolThread}");

            Sleep(TimeSpan.FromSeconds(seconds));
            throw new Exception("Boom");
            return 42 * seconds;

        }
    }

}
