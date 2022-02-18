#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BanqueBack.Models;

namespace BanqueBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly BanqueContext _context;

        public TransactionsController(BanqueContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions
                .OrderByDescending(s => s.Date)
                .ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.Transactionid)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.Transactionid }, transaction);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("bydates/{date1}/{date2}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByDates(DateTime date1, DateTime date2)
        {
            //var numAccount = 4;
            //var sql = string.Format("SELECT * From \"Transaction\" Where \"accountid\" = {0}", numAccount);
            //var accounts = await _context.Transactions.FromSqlRaw(sql).ToListAsync();

            var accounts = await _context.Transactions
                                      .Where(s => s.Date >= date1)
                                      .Where(s => s.Date <= date2)
                                      .OrderByDescending(s => s.Date)
                                      .ToListAsync();

            return accounts;
        }

        [HttpGet("byType/{type}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByOperation(string type)
        {
            var accounts = await _context.Transactions
                                      .Where(s => s.Operation == type)
                                      .OrderByDescending(s => s.Date)
                                      .ToListAsync();

            return accounts;
        }

        [HttpGet("byMonth")]
        public async Task<ActionResult<IEnumerable<Couple>>> GetTransactionsByMonth()
        {

            var grouped = await (from p in _context.Transactions
                                 .Where(p => p.Date < DateTime.Now)                   
                                 .Where(p => p.Date.Value.AddMonths(6) > DateTime.Now)
                                 group p by new { ope = p.Operation, month = p.Date.Value.Month }
                                 into grp
                                 select new Couple
                                 {
                                     Operation = grp.Key.ope,
                                     Month = grp.Key.month,
                                     Montant = grp.Sum(t => t.Montant)
                                 })
                                 .OrderByDescending(x => x.Month)
                                 .ToListAsync();

            return grouped;
        }

        [HttpGet("byCredit")]
        public async Task<ActionResult<IEnumerable<Result>>> GetTransactionsByCredit()
        {
            var accounts = await _context.Transactions
                                      .Where(s => s.Operation == "credit")
                                      .GroupBy(x => x.Description)
                                      .Select(y => new Result { Amount = y.Sum(b => b.Montant), Name = y.Key })
                                      .OrderByDescending(x => x.Amount)
                                      .ToListAsync();

            return accounts;
        }

        [HttpGet("byDebit")]
        public async Task<ActionResult<IEnumerable<Result>>> GetTransactionsByDebit()
        {
            var accounts = await _context.Transactions
                                      .Where(s => s.Operation == "debit")
                                      .GroupBy(x => x.Description)
                                      .Select(x => new Result { Amount = x.Sum(b => b.Montant), Name = x.Key })
                                      .OrderByDescending(x => x.Amount)
                                      .ToListAsync();

            return accounts;
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Transactionid == id);
        }
    }
}
