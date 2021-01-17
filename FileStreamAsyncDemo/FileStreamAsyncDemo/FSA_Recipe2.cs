using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Console;

namespace FileStreamAsyncDemo
{

    /// <summary>
    /// 编写一个异步的服务端和客户端
    /// </summary>
    public class FSA_Recipe2
    {

        public static void RunMain()
        {
            var server = new AsyncHttpServer(1234);
            var t = Task.Run(()=>server.Satrt());
            WriteLine("Listing on  port 1234 Open Http://localhost:1234/ in your Browsser");

            WriteLine("Trying To Connect>...");
            WriteLine();

            GetResponseAsync("http://localhost:1234").GetAwaiter().GetResult();
            WriteLine();
            WriteLine("please enter  to stop the server");
            ReadLine();
            server.Stop().GetAwaiter().GetResult();

        }

        public static async Task GetResponseAsync(string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                string responseHeaders = responseMessage.Headers.ToString();
                string responseContent = await responseMessage.Content.ReadAsStringAsync();

                WriteLine("response Headers:");
                WriteLine(responseHeaders);
                WriteLine("RespnseContent :");
                WriteLine(responseContent);
            }
        }
    }

    public class AsyncHttpServer
    {
        readonly HttpListener _httpListener;
        const string RESPONSE_TEMPLATE = "<html><head><title>Test</title></head><body><h2>TestPage</h2>" + "<h4>Today is :{0}</h4></body></html>";

        public AsyncHttpServer(int portNumber)
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add($"http://localhost:{portNumber}/");
        }

        public async Task Satrt()
        {
            _httpListener.Start();
            while (true)
            {
                var ctx = await _httpListener.GetContextAsync();
                WriteLine("Client Connection。。。。");
                var response = string.Format(RESPONSE_TEMPLATE, DateTime.Now);
                using (var sw = new StreamWriter(ctx.Response.OutputStream))
                {
                    await sw.WriteAsync(response);
                    await sw.FlushAsync();
                }
            }
        }

        public async Task Stop()
        {
            _httpListener.Abort();
        }
    }
}
