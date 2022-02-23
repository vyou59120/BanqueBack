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
    public class DirecteursController : ControllerBase
    {
        private readonly BanqueContext _context;

        public DirecteursController(BanqueContext context)
        {
            _context = context;
        }

        // GET: api/Directeurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Directeur>>> GetDirecteur()
        {
            return await _context.Directeur.ToListAsync();
        }

        // GET: api/Directeurs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Directeur>> GetDirecteur(int id)
        {
            var directeur = await _context.Directeur.FindAsync(id);

            if (directeur == null)
            {
                return NotFound();
            }

            return directeur;
        }

        [HttpGet("email/{email}")]
        public AuthenticateResponse GetDirecteurByEmail(string email)
        {
            var directeur = _context.Directeur.SingleOrDefault(x => x.Email == email);

            if (directeur == null) return null;

            //var token = generateJwtToken(directeur);


            //return new AuthenticateResponse(directeur, token);
            return new AuthenticateResponse(directeur.directeurid,directeur.Nom, directeur.Prenom, directeur.Adresse, directeur.Cp, directeur.Ville, directeur.Email, "ADMIN", directeur.Datenaissance);
        }

        // PUT: api/Directeurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDirecteur(int id, Directeur directeur)
        {
            if (id != directeur.directeurid)
            {
                return BadRequest();
            }

            _context.Entry(directeur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirecteurExists(id))
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

        // POST: api/Directeurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Directeur>> PostDirecteur(Directeur directeur)
        {
            _context.Directeur.Add(directeur);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDirecteur", new { id = directeur.directeurid }, directeur);
        }

        // DELETE: api/Directeurs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirecteur(int id)
        {
            var directeur = await _context.Directeur.FindAsync(id);
            if (directeur == null)
            {
                return NotFound();
            }

            _context.Directeur.Remove(directeur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string generateJwtToken(Directeur directeur)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var key = Encoding.ASCII.GetBytes("blablavlkfdqjlkvndsjkfnbsdlkbnlkdqfnfbkjnslkdvnkjdqnkbjndsfkj");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", directeur.directeurid.ToString()),
                    new Claim("role", "ADMIN")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool DirecteurExists(int id)
        {
            return _context.Directeur.Any(e => e.directeurid == id);
        }
    }
}
