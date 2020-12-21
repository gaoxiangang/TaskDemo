using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ConcurrentDemo
{

    /// <summary>
    /// 使用BlockingCollection来简化异步处理的工作负载
    /// </summary>
    public static class BLockingCollectionClass
    {

        /******************
         * BlockingCollection 可以改变任务存储在队列中数据的方式
         * 默认情况下 它使用的是 ConcurrentQueue容器。但是我们传入的IProducerConsumerCollection 是所有队列都实现的一个泛型接口
         * 然后第二次执行时传入的是ConcurrentStack的容器。这时候就改变了数据模式，由默认的先进先出改为了后进先出
         * GetConsumingEnumerable:消费者线程 通过使用GetConsumingEnumerable 来获取对应的工作项，当生产者队列中没有数据时，
         * GetConsumingEnumerable会阻塞线程直到生产者队列有数据。
         * CompleteAdding：当生产着使用CompleteAdding时表示该迭代周期结束，工作已经完成
         * **/

        //初始执行程序     
        public static async Task RunProgram(IProducerConsumerCollection<CustomTask> collection = null)
        {
            var taskCollection = new BlockingCollection<CustomTask>();
            if (collection != null)
                taskCollection = new BlockingCollection<CustomTask>(collection);
            //开始执行生产者操作去生产数据
            var taskSource = Task.Run(() => TaskProducer(taskCollection));
            //建造四个消费者工作线程
            Task[] tasks = new Task[4];

            for (int i = 1; i <= 4; i++)
            {
                string processorId = $"process{i}";
                //每一个线程都执行一个完成消费者工作的操作
                tasks[i - 1] = Task.Run(() => TaskProcessor(taskCollection, processorId));
            }
            //等待生产者工作线程完成
            await taskSource;
            //等待所有消费者工作完成
            await Task.WhenAll(tasks);
        }

        //执行生产者操作
        public static async Task TaskProducer(BlockingCollection<CustomTask> collection)
        {
            await Task.Delay(20);

            for (int i = 0; i <= 20; i++)
            {
                var customerTask = new CustomTask { ID = i };
                collection.Add(customerTask);
                Console.WriteLine($"线程{customerTask.ID}已经被生产！");
            }

            collection.CompleteAdding();
        }

        //执行消费者操作
        public static async Task TaskProcessor(BlockingCollection<CustomTask> collection, string name)
        {
            await GetRandomDleay();
            foreach (CustomTask item in collection.GetConsumingEnumerable())
            {
                Console.WriteLine($"线程{item.ID}已被生产者{name}处理");
                await GetRandomDleay();
            }

        }
        public static Task GetRandomDleay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);
        }
    }
}
