using System.Collections.Generic;

namespace DBExerciseClassLibrary
{
    public interface IDataAccessLayer
    {
        void Delete(Account account);
        void DeleteById(int id);
        IEnumerable<Account> FindAccountsFromPartOfName(string partOfName);
        Account FindById(int id);
        IEnumerable<Account> GetAccounts();
        void Insert(Account account);
        void Transfer(int sourceAccountId, int destimationAccountId, decimal amountToTransfer);
    }
}