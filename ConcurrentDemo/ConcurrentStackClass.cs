using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentDemo
{
    public static class ConcurrentStackClass
    {
         //后进先出模式。优先处理最近进入生产者队列的数据，先进入生产队列的数据具有较低的优先级。直到生产者不再生产数据
         //最早的数据才会被处理
        public static async Task RunProgram()
        {
            var  taskStack = new ConcurrentStack<CustomTask>();
            var cts = new CancellationTokenSource();
            var taks = Task.Run(() => TaskProducter(taskStack));
            Task[] processors = new Task[4];

            for (int i = 1; i <= 4; i++)
            {
                string processId = i.ToString();
                processors[i - 1] = Task.Run(() => TaskProcessor(taskStack, $"processors:{processId}", cts.Token));
            }
            await taks;
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            await Task.WhenAll(processors);
        }
        /// <summary>
        ///  生产者队列
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static async Task TaskProducter(ConcurrentStack<CustomTask> taskStack)
        {

            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(50);
                var workItem = new CustomTask() { ID = i };
                taskStack.Push(workItem);
                Console.WriteLine($"{workItem.ID} 已经被生产。。。");
            }
        }
        /// <summary>
        /// 消费者队列
        /// </summary>
        /// <param name="customTasks"></param>
        /// <param name="name"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task TaskProcessor(ConcurrentStack<CustomTask> taskStack, string name, CancellationToken token)
        {
            CustomTask customItem;
            bool dequeueSuccesful = false;
            await GetRandomDleay();
            do
            {
                dequeueSuccesful = taskStack.TryPop(out customItem);
                if (dequeueSuccesful)
                {
                    Console.WriteLine($"{customItem.ID} 已经被消费。。。");
                }
            } while (!token.IsCancellationRequested);

        }

        public static Task GetRandomDleay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);
        }

    }
}
