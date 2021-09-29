using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Dapper.Transaction;

namespace DBExerciseClassLibrary
{
    public class AccountDataAccess : IDataAccessLayer
    {
        private const string ConnectionString = @"Data Source=DESKTOP-HU2QTEB\SQLEXPRESS; Initial Catalog=Company; Integrated Security=True; Trusted_Connection=True;";

        //public void Insert(Account account)
        //{
        //    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-HU2QTEB\SQLEXPRESS; Initial Catalog=Company; Integrated Security=True; Trusted_Connection=True;"))
        //    {
        //        SqlCommand command = new SqlCommand("INSERT INTO Account VALUES (@Name, @Balance); SELECT CAST(scope_identity() AS int)", connection);
        //        command.Parameters.AddWithValue("@Name", account.Name);
        //        command.Parameters.AddWithValue("@Balance", account.Balance);
        //        try
        //        {
        //            connection.Open();
        //            account.Id = (int)command.ExecuteScalar();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error: `{ ex.Message}`");
        //        }
        //    }
        //}

        public void Insert(Account account)
        {
            string sqlCommand = "INSERT INTO Account VALUES (@Name, @Balance); SELECT CAST(scope_identity() AS int)";
            using SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                account.Id = connection.ExecuteScalar<int>(sqlCommand, new { account.Name, account.Balance });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting account with id {account.Id}: `{ ex.Message}`");
            }
        }

        public IEnumerable<Account> GetAccounts()
        {
            string sqlCommand = "SELECT * FROM Account";
            List<Account> accounts = new List<Account>();
            using SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                accounts = connection.Query<Account>(sqlCommand).AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when getting all account: `{ex.Message}`");
            }
            return accounts;
        }

        public void Delete(Account account)
        {
            DeleteById(account.Id);
        }

        //public void DeleteById(int id)
        //{
        //    using (SqlConnection connection = new SqlConnection(ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand("DELETE FROM Account WHERE Id = @Id", connection);
        //        command.Parameters.AddWithValue("@Id", id);
        //        try
        //        {
        //            connection.Open();
        //            command.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error: `{ ex.Message}`");
        //        }
        //    }
        //}
        public void DeleteById(int id)
        {
            string sqlCommand = "DELETE FROM Account WHERE Id = @Id";
            using SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                connection.Execute(sqlCommand, new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accured when deleting account with id {id}: `{ex.Message}`");
            }
        }

        //public void Transfer(int sourceAccountId, int destimationAccountId, decimal amountToTransfer)
        //{
        //    using (SqlConnection connection = new SqlConnection(ConnectionString))
        //    {
        //        SqlTransaction transaction = null;
        //        try
        //        {
        //            connection.Open();
        //            transaction = connection.BeginTransaction();

        //            // Command 1
        //            SqlCommand commandUpdateSourceBalance = new SqlCommand("UPDATE Account SET Balance=Balance - @Amount WHERE Id = @SourceAccountId", connection, transaction);
        //            commandUpdateSourceBalance.Parameters.AddWithValue("@Amount", amountToTransfer);
        //            commandUpdateSourceBalance.Parameters.AddWithValue("@SourceAccountId", sourceAccountId);

        //            // Command 2
        //            SqlCommand commandUpdateDestinationBalance = new SqlCommand("UPDATE Account SET Balance=Balance + @Amount WHERE Id = @DestimationAccountId", connection, transaction);
        //            commandUpdateDestinationBalance.Parameters.AddWithValue("@Amount", amountToTransfer);
        //            commandUpdateDestinationBalance.Parameters.AddWithValue("@DestimationAccountId", destimationAccountId);

        //            commandUpdateSourceBalance.ExecuteNonQuery();
        //            commandUpdateDestinationBalance.ExecuteNonQuery();
        //            transaction.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error: `{ex.Message}`");
        //            Console.WriteLine("Rolling back...");
        //            try
        //            {
        //                transaction.Rollback();
        //            }
        //            catch (Exception exRollBack)
        //            {
        //                Console.WriteLine($"[CRITICAL] Error during roll back: `{exRollBack.Message}`");
        //            }
        //        }
        //    }
        //}

        public void Transfer(int sourceAccountId, int destimationAccountId, decimal amount)
        {
            string sqlCommandSource = "UPDATE Account SET Balance=Balance - @Amount WHERE Id = @SourceAccountId";
            string sqlCommandTarget = "UPDATE Account SET Balance=Balance + @Amount WHERE Id = @DestimationAccountId";

            using SqlConnection connection = new SqlConnection(ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                using (transaction = connection.BeginTransaction())
                {
                    transaction.Execute(sqlCommandSource, new { amount, sourceAccountId });
                    transaction.Execute(sqlCommandTarget, new { amount, destimationAccountId });
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: `{ex.Message}`");
                Console.WriteLine("Rolling back...");
                try
                {
                    transaction.Rollback();
                }
                catch (Exception exRollBack)
                {
                    Console.WriteLine($"[CRITICAL] /!\\ Error during roll back: `{exRollBack.Message}`");
                }
            }
        }

        //public Account FindById(int id)
        //{
        //    Account foundAccount = null;
        //    using (SqlConnection connection = new SqlConnection(ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand("SELECT * FROM Account WHERE Id = @Id", connection);
        //        command.Parameters.AddWithValue("@Id", id);
        //        try
        //        {
        //            connection.Open();
        //            SqlDataReader reader = command.ExecuteReader();
        //            if (reader.Read())
        //            {
        //                foundAccount = new Account((int)reader["Id"], (string)reader["Name"], (decimal)reader["Balance"]);
        //            }
        //            reader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error: `{ ex.Message}`");
        //        }
        //    }
        //    return foundAccount;
        //}

        public Account FindById(int id)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            string sqlCommand = "SELECT * FROM Account WHERE Id = @Id";
            try
            {
                return (Account)connection.Query<Account>(sqlCommand, new { id }).AsList()[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when finding account with id {id}: `{ex.Message}`");
            }
            return null;
        }

        //public IEnumerable<Account> FindAccountsFromPartOfName(string partOfName)
        //{
        //    List<Account> accounts = new List<Account>();
        //    using (SqlConnection connection = new SqlConnection(ConnectionString))
        //    {
        //        SqlCommand command = new SqlCommand("SELECT * FROM Account WHERE Name LIKE @PartOfName", connection);
        //        command.Parameters.AddWithValue("@PartOfName", $"%{partOfName}%");
        //        try
        //        {
        //            connection.Open();
        //            SqlDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                accounts.Add(new Account((int)reader["Id"], (string)reader["Name"], (decimal)reader["Balance"]));
        //            }
        //            reader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error: `{ ex.Message}`");
        //        }
        //    }
        //    return accounts;
        //}

        public IEnumerable<Account> FindAccountsFromPartOfName(string name)
        {
            string sqlCommand = "SELECT * FROM Account WHERE Name LIKE @PartOfName";
            string partOfName = $"%{name}%";
            List<Account> accounts = new List<Account>();
            using SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                accounts = connection.Query<Account>(sqlCommand, new { partOfName }).AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving names containing \"{name}\": `{ex.Message}`");
            }
            return accounts;
        }
    }
}
