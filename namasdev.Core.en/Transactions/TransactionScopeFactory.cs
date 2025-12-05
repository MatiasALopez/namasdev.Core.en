using System.Transactions;

namespace namasdev.Core.Transactions
{
    public class TransactionScopeFactory
    {
        public static TransactionScope Create(
            TransactionScopeOption useTransaction = TransactionScopeOption.Required, 
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new TransactionScope(useTransaction, new TransactionOptions { IsolationLevel = isolationLevel });
        }
    }
}
