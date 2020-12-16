using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentDemo
{
    public static class ConcurrentQueueClass
    {
        //首先使用ConcurrentQueue创建一个任务队列，然后创建一个取消标志，然后再启用一个工作者线程去完成生产
        //之后又去创建了四个工作者线程去处理任务。四个工作者会每隔不同的时间从生产队列中取出数据去完成，直到得到取消状态
        // 之后我们等待生产者工作线程完成，然后等待一段时间后去执行任务取消状态。最后再等待所有的消费者线程工作完成
        //由于每个消费者线程等待的时间不同，所有结果会有先后顺序，但是却没有重复的，所以该生产者队列是安全的
        public static async Task RunProgram()
        {
            var queue = new ConcurrentQueue<CustomTask>();
            var cts = new CancellationTokenSource();
            var taks = Task.Run(() => TaskProducter(queue));
            Task[] processors = new Task[4];

            for (int i = 1; i <= 4; i++)
            {
                string processId = i.ToString();
                processors[i - 1] = Task.Run(() => TaskProcessor(queue, $"processors:{processId}", cts.Token));
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
        public static async Task TaskProducter(ConcurrentQueue<CustomTask> queue)
        {

            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(50);
                var workItem = new CustomTask() { ID = i };
                queue.Enqueue(workItem);
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
        public static async Task TaskProcessor(ConcurrentQueue<CustomTask> customTasks, string name, CancellationToken token)
        {
            CustomTask customItem;
            bool dequeueSuccesful = false;
            await GetRandomDleay();
            do
            {
                dequeueSuccesful = customTasks.TryDequeue(out customItem);
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
    public class CustomTask
    {
        public int ID { get; set; }
    }
}
