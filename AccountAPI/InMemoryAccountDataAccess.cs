using AccountAPI.Controllers;
using DBExerciseClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAPI
{
    public class InMemoryAccountDataAccess : IDataAccessLayer
    {
        public AccountController AccountController { get; set; }

        public InMemoryAccountDataAccess()
        {
            AccountController = new AccountController(new AccountDataAccess());
        }

        public void Delete(Account account)
        {
            throw new NotImplementedException("Deleteing accounts is not added yet");
        }

        public void DeleteById(int id)
        {
            AccountController.DeleteById(id);
        }

        public IEnumerable<Account> FindAccountsFromPartOfName(string partOfName)
        {
            throw new NotImplementedException("Finding by id is not added yet");
        }

        public Account FindById(int id)
        {
            return AccountController.GetById(id).Value;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return AccountController.Get();
        }

        public void Insert(Account account)
        {
            AccountController.AddAccount(account);
        }

        public void Transfer(int sourceAccountId, int destimationAccountId, decimal amountToTransfer)
        {
            throw new NotImplementedException("Transfering is not implemented yet");
        }
    }
}
