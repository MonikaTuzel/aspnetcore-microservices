using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransactionApi.IServices;
using TransactionsApi.Data;
using TransactionsApi.Models;

namespace TransactionsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private ITransactionService transactionService { get;}
        
        public TransactionsController(ITransactionService _transactionService)
        {
            transactionService = _transactionService;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<IEnumerable<Transaction>> GetTransaction()
        {
            return await transactionService.GetTransactions();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
          return await transactionService.GetTransactionByIdAsync(id);
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public Transaction PutTransaction([FromBody]Transaction transaction)
        {
            return transactionService.UpdateTransaction(transaction);
        }

        // POST: api/Transactions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<String> PostTransaction([FromBody] Transaction transaction)
        {
            return await transactionService.AddTransaction(transaction);

        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteTransaction(int id)
        {
            return await transactionService.DeleteTransaction(id);
            
            ///<summary>
            /// metoda usuwająca nową transakcję
            ///</summary>

        }


        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.idTransactions == id);
        }
    }
}
