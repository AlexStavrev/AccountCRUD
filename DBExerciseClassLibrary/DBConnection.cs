using System;
using System.Data.SqlClient;

namespace DBExerciseClassLibrary
{
    public class DBConnection
    {
        public string ConnectionString { get; private set; }

        public DBConnection(string connectionString = @"Data Source=DESKTOP-HU2QTEB\SQLEXPRESS; Initial Catalog=Company; Integrated Security=True; Trusted_Connection=True;")
        {
            ConnectionString = connectionString;
        }

        public void ReadDataBase()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Account", connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine
                            (
                                reader["ID"] + ") " +
                                reader["Name"] + Environment.NewLine +
                                reader["Balance"] + Environment.NewLine  + "----------------"
                             );
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: `{ ex.Message}`");
                }
            }
        }

        public decimal TotalBalance
        {
            get
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand("SELECT SUM(CAST(Balance AS decimal(16,2))) FROM Account", connection);
                    try
                    {
                        connection.Open();
                        return (decimal)command.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: `{ ex.Message}`");
                    }
                }
                return 0;
            }
        }
    }
}
