using AccountAPI.Controllers;
using DBExerciseClassLibrary;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            TryWebAPI();
            TryDatabase();
        }

        private static void TryWebAPI()
        {
            Console.WriteLine("Using rest client");
            AccountAPIConsumercs accountApi = new AccountAPIConsumercs();

            accountApi.DeleteById(16);

            foreach (Account account in accountApi.GetAllAccounts())
            {
                Console.WriteLine(account);
            }
        }

        private static void TryDatabase()
        {
            DBConnection dataBase = new DBConnection();
            Console.WriteLine($"Connecting to {dataBase.ConnectionString}...");
            dataBase.ReadDataBase();
            Console.WriteLine($"Total Balance: {dataBase.TotalBalance}");
            Console.WriteLine();
            AccountDataAccess dataAccess = new AccountDataAccess();

            //Transfer Money from Candice to Bob
            dataAccess.Transfer(3, 1, 200.3m);
            foreach (Account account in dataAccess.GetAccounts())
            {
                Console.WriteLine(account);
            }

            Console.WriteLine(dataAccess.FindById(1));

            // Find all containing something in their name
            Console.WriteLine("Accounts containing 'O'");
            foreach (Account account in dataAccess.FindAccountsFromPartOfName("o"))
            {
                Console.WriteLine(account);
            }

            // Add > Print > Delete > Print
            Console.WriteLine(Environment.NewLine + "Add > Print > Delete > Print");
            Account lastAccount = new Account(0, "TEST ACCOUNT", 1000);
            dataAccess.Insert(lastAccount);
            foreach (Account account in dataAccess.GetAccounts())
            {
                Console.WriteLine(account);
            }
            dataAccess.DeleteById(lastAccount.Id);
            Console.WriteLine("After Deletion...");
            foreach (Account account in dataAccess.GetAccounts())
            {
                Console.WriteLine(account);
            }

            // Add a bunch
            //dataAccess.Insert(new Account(0, "Allice Gal", 523.23m));
            //dataAccess.Insert(new Account(0, "Candice Willon", 7324.83m));
            //dataAccess.Insert(new Account(0, "Grub Anderson", 234.74m));
            //dataAccess.Insert(new Account(0, "Malloc Vin", 6745.25m));
            //dataAccess.Insert(new Account(0, "Nancy An", 7845.34m));
            //dataAccess.Insert(new Account(0, "Krover Uurn", 324.94m));
            //dataAccess.Insert(new Account(0, "Al Newl", 0));
        }
    }
}
