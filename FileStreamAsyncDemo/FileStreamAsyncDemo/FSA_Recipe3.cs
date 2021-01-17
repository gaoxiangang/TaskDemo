using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FileStreamAsyncDemo
{
    public class FSA_Recipe3
    {

        public static void RunMain()
        {
            var t = ProcessAsynchronousIO();
            t.GetAwaiter().GetResult();


        }

        public static async Task ProcessAsynchronousIO()
        {
            try
            {
                const string connectionString = @"Server=47.116.75.154;Database=LeoGXGDB;UID=sa;PWD=Pass1234; ";

                using (var connection = new SqlConnection(connectionString))
                {

                    await connection.OpenAsync();
                    var cmd = new SqlCommand("SELECT * FROM Staff ", connection);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var id = reader.GetFieldValue<string>(0);
                            var staffNo = reader.GetFieldValue<string>(1);

                            Console.WriteLine($"Table Rows ID:{id},StaffNo:{staffNo}");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
