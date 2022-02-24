#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BanqueBack.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using BanqueBack.Helpers;

namespace BanqueBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly BanqueContext _context;

        public ManagerController(BanqueContext context)
        {
            _context = context;
        }

        // GET: api/Manager
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manager>>> GetManager()
        {
            return await _context.Manager.ToListAsync();
        }

        // GET: api/Manager/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Manager>> GetManager(int id)
        {
            var manager = await _context.Manager.FindAsync(id);

            if (manager == null)
            {
                return NotFound();
            }

            return manager;
        }

        [HttpGet("email/{email}")]
        public AuthenticateResponse GetManagerByEmail(string email)
        {
            var manager = _context.Manager.SingleOrDefault(x => x.Email == email);

            if (manager == null) return null;

            //var token = generateJwtToken(manager);


            //return new AuthenticateResponse(manager, token);
            return new AuthenticateResponse(manager.managerid, manager.Nom, manager.Prenom, manager.Adresse, manager.Cp, manager.Ville, manager.Email, "ADMIN", manager.Datenaissance);
        }

        // PUT: api/Manager/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutManager(int id, Manager manager)
        {
            if (id != manager.managerid)
            {
                return BadRequest();
            }

            _context.Entry(manager).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManagerExists(id))
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

        // POST: api/Manager
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Manager>> PostManager(Manager manager)
        {
            _context.Manager.Add(manager);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetManager", new { id = manager.managerid }, manager);
        }

        // DELETE: api/Manager/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManager(int id)
        {
            var manager = await _context.Manager.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            _context.Manager.Remove(manager);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string generateJwtToken(Manager manager)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var key = Encoding.ASCII.GetBytes("blablavlkfdqjlkvndsjkfnbsdlkbnlkdqfnfbkjnslkdvnkjdqnkbjndsfkj");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", manager.managerid.ToString()),
                    new Claim("role", "ADMIN")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool ManagerExists(int id)
        {
            return _context.Manager.Any(e => e.managerid == id);
        }
    }
}
