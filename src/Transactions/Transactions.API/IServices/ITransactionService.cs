using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionsApi.Models;

namespace TransactionApi.IServices
{
    public interface ITransactionService
    {
        Task<IList<Transaction>> GetTransactions();
        Task<Transaction> GetTransactionByIdAsync(int idTransactions);
        Task<string> AddTransaction(Transaction Transaction);
        Transaction UpdateTransaction(Transaction Transaction);
        Task<string> DeleteTransaction(int idTransactions);

    }
}
