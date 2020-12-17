using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConcurrentDemo
{
    public static class ConcurrentBagClass
    {

        static Dictionary<string, string[]> dict = new Dictionary<string, string[]>();

        public static async Task RunProgram()
        {
            var cbg = new ConcurrentBag<CrawlingTask>();
            string[] urls = { "baidu", "taobao", "google", "faceboook" };

            Task[] tasks = new Task[4];

            for (int i = 1; i <= 4; i++)
            {
                string crawlName = $"Crowl{i}";

                cbg.Add(new CrawlingTask
                {
                    UrlToCrowl = urls[i - 1],
                    ProducterName = "root"
                });
                tasks[i - 1] = Task.Run(() => crowl(cbg, crawlName));
            }
            await Task.WhenAll(tasks);
        }

        public static async Task crowl(ConcurrentBag<CrawlingTask> crawlingTasks, string crawlName)
        {

            CrawlingTask task;
            while (crawlingTasks.TryTake(out task))
            {
                IEnumerable<string> urls = await GetLinkFormatContent(task);
                if (urls != null)
                {
                    foreach (var item in urls)
                    {
                        var t = new CrawlingTask()
                        {
                            UrlToCrowl = item,
                            ProducterName = crawlName
                        };
                        crawlingTasks.Add(t);
                    }
                }

                Console.WriteLine($"网页{task.UrlToCrowl}URL被放入队列{task.ProducterName},在{crawlName}消费者被完成");
            }

        }

        public static async Task<IEnumerable<string>> GetLinkFormatContent(CrawlingTask crawlingTask)
        {
            await GetRandomDleay();

            if (dict.ContainsKey(crawlingTask.UrlToCrowl))
                return dict[crawlingTask.UrlToCrowl];

            return null;
        }


        public static void CreateLinks()
        {
            dict["baidu"] = new[] { "httpbaidu1", "httptaobao1", "httpgoogle1", "httpfacebooo1" };
            dict["taobao"] = new[] { "httpbaidu2", "httptaobao2", "httpgoogle2", "httpfaceboook2" };
            dict["google"] = new[] { "httpbaidu3", "httptaobao3", "httpgoogle3", "httpfaceboook3" };
            dict["faceboook"] = new[] { "httpbaidu4", "httptaobao4", "httpgoogle4", "httpfaceboook4" };
        }
        public static Task GetRandomDleay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(150, 200);
            return Task.Delay(delay);
        }
    }

    public class CrawlingTask
    {
        public string UrlToCrowl { get; set; }

        public string ProducterName { get; set; }
    }
}
