using TransactionApi.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionsApi.Data;
using TransactionsApi.Models;

namespace TransactionApi.Services
{
    public class TransactionService : ITransactionService
    {
        ApplicationDbContext dbContext;

        public TransactionService(ApplicationDbContext _db)
        {
            dbContext = _db;
        }

        public async Task<string> AddTransaction(Transaction Transaction)
        {
            dbContext.Transaction.Add(Transaction);
            await dbContext.SaveChangesAsync();

            return await Task.FromResult("");

            ///<summary>
            /// metoda dodająca nową transakcję
            ///</summary>
        }

        public async Task<string> DeleteTransaction(int id)
        {
            var Transaction = dbContext.Transaction.FirstOrDefault(x => x.idTransactions == id);
            dbContext.Entry(Transaction).State = EntityState.Deleted;
            dbContext.SaveChanges();
            return await Task.FromResult("");
        }

        public async Task<Transaction> GetTransactionByIdAsync(int idTransactions)
        {
            var Transaction = await dbContext.Transaction.FindAsync(idTransactions);

            if (Transaction == null)
            {

            }

            return Transaction;
        }


        public async Task<IList<Transaction>> GetTransactions()
        {
            return await dbContext.Transaction.ToListAsync();
        }

        public Transaction UpdateTransaction(Transaction transaction)
        {
            dbContext.Entry(transaction).State = EntityState.Modified;
            dbContext.SaveChanges();
            return transaction;
        }

    }
}
