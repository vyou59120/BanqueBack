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
    public class CommercialController : ControllerBase
    {
        private readonly BanqueContext _context;

        public CommercialController(BanqueContext context)
        {
            _context = context;
        }

        // GET: api/Commercials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Commercial>>> GetCommercial()
        {
            return await _context.Commercials.ToListAsync();
        }

        // GET: api/Commercials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Commercial>> GetCommercial(int id)
        {
            var Commercial = await _context.Commercials.FindAsync(id);

            if (Commercial == null)
            {
                return NotFound();
            }

            return Commercial;
        }

        // PUT: api/Commercials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommercial(int id, Commercial Commercial)
        {
            if (id != Commercial.commercialid)
            {
                return BadRequest();
            }

            _context.Entry(Commercial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommercialExists(id))
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

        // POST: api/Commercials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Commercial>> PostCommercial(Commercial Commercial)
        {
            _context.Commercials.Add(Commercial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommercial", new { id = Commercial.commercialid }, Commercial);
        }

        // DELETE: api/Commercials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommercial(int id)
        {
            var Commercial = await _context.Commercials.FindAsync(id);
            if (Commercial == null) 
            {
                return NotFound();
            }

            _context.Commercials.Remove(Commercial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*
        // GET: api/CommercialsAccount
        //public async Task<ActionResult<IEnumerable<Commercial>>> GetCommercialAccount()
        [HttpGet("Commercial_Account/{id}")]
        public async Task<ActionResult<Commercial>> GetCommercialAccount()
        {
            List<Commercial> Commercials = await _context.Commercials.ToListAsync();
            List<Account> accounts = await _context.Accounts.ToListAsync();
            var query = from account in accounts
                        join Commercial in Commercials
                        on account.commercialid
                        equals Commercial.commercialid
                        select new { AccountOwner = account.Commercial };
            //Exclude duplicates.
            var Q = query.Distinct().ToList();
            return Ok(Q);
        }
         */

        private bool CommercialExists(int id)
        {
            return _context.Commercials.Any(e => e.commercialid == id);
        }
    }
}
