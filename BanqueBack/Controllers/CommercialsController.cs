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
    public class CommercialsController : ControllerBase
    {
        private readonly BanqueContext _context;

        public CommercialsController(BanqueContext context)
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
            var commercial = await _context.Commercials.FindAsync(id);

            if (commercial == null)
            {
                return NotFound();
            }

            return commercial;
        }

        [HttpGet("email/{email}")]
        public AuthenticateResponse GetCommercialByEmail(string email)
        {
            var commercial = _context.Commercials.SingleOrDefault(x => x.Email == email);

            if (commercial == null) 
                return new AuthenticateResponse(0, "", "", "", "", "", "", "", new DateTime());

            //var token = generateJwtToken(commercial);

            return new AuthenticateResponse(commercial.commercialid, commercial.Nom, commercial.Prenom, commercial.Adresse, commercial.Cp, commercial.Ville, commercial.Email, "STAFF", commercial.Datenaissance);
            //return new AuthenticateResponse(commercial, token);
        }

        // PUT: api/Commercials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommercial(int id, Commercial commercial)
        {
            if (id != commercial.commercialid)
            {
                return BadRequest();
            }

            _context.Entry(commercial).State = EntityState.Modified;

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
        public async Task<ActionResult<Commercial>> PostCommercial(Commercial commercial)
        {
            _context.Commercials.Add(commercial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommercial", new { id = commercial.commercialid }, commercial);
        }

        // DELETE: api/Commercials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommercial(int id)
        {
            var commercial = await _context.Commercials.FindAsync(id);
            if (commercial == null)
            {
                return NotFound();
            }

            _context.Commercials.Remove(commercial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string generateJwtToken(Commercial commercial)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var key = Encoding.ASCII.GetBytes("blablavlkfdqjlkvndsjkfnbsdlkbnlkdqfnfbkjnslkdvnkjdqnkbjndsfkj");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", commercial.commercialid.ToString()),
                    new Claim("role", "STAFF")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool CommercialExists(int id)
        {
            return _context.Commercials.Any(e => e.commercialid == id);
        }
    }
}
