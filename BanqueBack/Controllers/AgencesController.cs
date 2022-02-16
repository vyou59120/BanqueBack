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
    public class AgencesController : ControllerBase
    {
        private readonly BanqueContext _context;

        public AgencesController(BanqueContext context)
        {
            _context = context;
        }

        // GET: api/Agences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agence>>> GetAgences()
        {
            return await _context.Agences.ToListAsync();
        }

        // GET: api/Agences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agence>> GetAgence(int id)
        {
            var agence = await _context.Agences.FindAsync(id);

            if (agence == null)
            {
                return NotFound();
            }

            return agence;
        }

        // PUT: api/Agences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgence(int id, Agence agence)
        {
            if (id != agence.Agenceid)
            {
                return BadRequest();
            }

            _context.Entry(agence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgenceExists(id))
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

        // POST: api/Agences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Agence>> PostAgence(Agence agence)
        {
            _context.Agences.Add(agence);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAgence", new { id = agence.Agenceid }, agence);
        }

        // DELETE: api/Agences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgence(int id)
        {
            var agence = await _context.Agences.FindAsync(id);
            if (agence == null)
            {
                return NotFound();
            }

            _context.Agences.Remove(agence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgenceExists(int id)
        {
            return _context.Agences.Any(e => e.Agenceid == id);
        }
    }
}
