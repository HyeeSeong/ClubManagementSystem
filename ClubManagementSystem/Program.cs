using MySql.Data.MySqlClient;
using System;

class Program
{
    static void Main(string[] args)
    {
        string connStr = "Server=127.0.0.1;Port=3306;" +
                         "Database=club_management;" +
                         "Uid=hyeseong;" +
                         "Pwd=nhseo3482!;";

        using (var conn = new MySqlConnection(connStr))
        {
            try
            {
                conn.Open();
                Console.WriteLine("DB Connection Successful!");

                // 간단한 쿼리 테스트
                var cmd = new MySqlCommand("SELECT 1", conn);
                var result = cmd.ExecuteScalar();
                Console.WriteLine("Query result: " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB Connection Failed: " + ex.Message);
            }
        }
    }
}