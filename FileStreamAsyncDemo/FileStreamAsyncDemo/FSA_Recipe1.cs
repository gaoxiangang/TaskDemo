using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static System.Text.Encoding;
namespace FileStreamAsyncDemo
{
    public class FSA_Recipe1
    {
        const int BUFF_SIZE = 4096;
        public static async Task RunMain()
        {
            using (var stream = new FileStream("text1.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None, BUFF_SIZE))
            {
                WriteLine($"1.Uses I/O Threads:{stream.IsAsync}");
                byte[] buffer = UTF8.GetBytes(CreateFileContent());

                var writeTask = Task.Factory.FromAsync(stream.BeginWrite, stream.EndWrite, buffer, 0, buffer.Length, null);
                await writeTask;
            }

            using (var stream = new FileStream("text2.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.None, BUFF_SIZE, FileOptions.Asynchronous))
            {
                WriteLine($"2.Uses I/O Threads:{stream.IsAsync}");
                byte[] buffer = UTF8.GetBytes(CreateFileContent());

                var writeTask = Task.Factory.FromAsync(stream.BeginWrite, stream.EndWrite, buffer, 0, buffer.Length, null);
                await writeTask;
            }


            using (var stream = File.Create("text3.txt", BUFF_SIZE, FileOptions.Asynchronous))
            using (var sw = new StreamWriter(stream))
            {
                WriteLine($"3.Uses I/O Threads:{stream.IsAsync}");
                await sw.WriteAsync(CreateFileContent());
            }


            using (var sw = new StreamWriter("text4.txt", true))
            {
                WriteLine($"4.Uses I/O Threads:{((FileStream)sw.BaseStream).IsAsync}");
                await sw.WriteAsync(CreateFileContent());
            }

            WriteLine("Starting parsing files in Parallel");

            var readTask = new Task<long>[4];
            for (int i = 0; i < 4; i++)
            {
                string fileName = $"text{i + 1}.txt";
                readTask[i] = SumFileContent(fileName);
            }
            long[] sums = await Task.WhenAll(readTask);
            WriteLine($"Sum in all Files {sums.Sum()}");
            WriteLine("Deleting files.....");

            Task[] deleteTask = new Task[4];

            for (int i = 0; i < 4; i++)
            {
                string fileName = $"text{i + 1}.txt";
                deleteTask[i] = asynchronousDelete(fileName);
            }

            await Task.WhenAll(deleteTask);

            WriteLine("Deleting Complete!");
        }


        public static Task asynchronousDelete(string fileName)
        {
            return Task.Run(() => File.Delete(fileName));
        }

        public static string CreateFileContent()
        {

            var sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                sb.Append($"{new Random(i).Next(0, 99999)}");
                sb.AppendLine();
            }
            return sb.ToString();
        }


        public static async Task<long> SumFileContent(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None, BUFF_SIZE, FileOptions.Asynchronous))
            using (var sr = new StreamReader(stream))
            {
                long sum = 0;
                while (sr.Peek() > -1)
                {
                    string line = await sr.ReadLineAsync();
                    sum += long.Parse(line);
                }
                return sum;
            }
        }
    }
}
