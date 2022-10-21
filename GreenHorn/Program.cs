using System;
using Microsoft.Data.SqlClient;

public class Program
{
    public static void Main(string[] args)
    {
        // Use your own server, database, and user ID.
        // Connetion string - user ID is not provided and is asked interactively.
        string ConnectionString = @"Server=tcp:greenhorn.database.windows.net,1433;Initial Catalog=GreenHornDB;Persist Security Info=False;User ID=2195776@johnabbottcollege.net;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication='Active Directory Integrated';";


        using (SqlConnection conn = new SqlConnection(ConnectionString))

        {
            conn.Open();
            Console.WriteLine("ConnectionString2 succeeded.");
            using (var cmd = new SqlCommand("SELECT @@Version", conn))
            {
                Console.WriteLine("select @@version");
                var result = cmd.ExecuteScalar();
                Console.WriteLine(result.ToString());
            }

        }
        Console.ReadKey();

    }
}