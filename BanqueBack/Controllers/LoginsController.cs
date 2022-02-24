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
    public class LoginsController : ControllerBase
    {
        private readonly BanqueContext _context;

        public LoginsController(BanqueContext context)
        {
            _context = context;
        }

        // GET: api/Logins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Login>>> GetLogins()
        {
            return await _context.Logins.ToListAsync();
        }

        // GET: api/Logins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Login>> GetLogin(int id)
        {
            var login = await _context.Logins.FindAsync(id);

            if (login == null)
            {
                return NotFound();
            }

            return login;
        }

        // PUT: api/Logins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize("CLIENT,STAFF,ADMIN")]
        public async Task<IActionResult> PutLogin(int id, Login login)
        {
            if (id != login.Id)
            {
                return BadRequest();
            }

            _context.Entry(login).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginExists(id))
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

        [HttpPost("authenticate")]
        public LoginResponse Authenticate(AuthenticateRequest model)
        {
            var login = _context.Logins.SingleOrDefault(x => x.Email == model.Email && x.Motdepasse == Common.Secure.Encrypteur(model.Motdepasse));
            if (login == null)
                return new LoginResponse(0, "", "", "");
                //return CreatedAtAction("PostLogin", new { id = 0, email = "" });

            var token = generateJwtToken(login);

            return new LoginResponse(login.Id,login.Email,login.Role,token);

            //return CreatedAtAction("PostLogin", new { id = login.Id, email = login.Email });
        }

        // POST: api/Logins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Login>> PostLogin(Login login)
        {
            Login monlogin = new Login(login.Id, login.Email, login.Role, Common.Secure.Encrypteur(login.Motdepasse));
            
            _context.Logins.Add(monlogin);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LoginExists(login.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("PostLogin", new { id = login.Id }, login);
        }

        // DELETE: api/Logins/5
        [HttpDelete("{id}")]
        [Authorize("ADMIN")]
        public async Task<IActionResult> DeleteLogin(int id)
        {
            var login = await _context.Logins.FindAsync(id);
            if (login == null)
            {
                return NotFound();
            }

            _context.Logins.Remove(login);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string generateJwtToken(Login login)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var key = Encoding.ASCII.GetBytes("blablavlkfdqjlkvndsjkfnbsdlkbnlkdqfnfbkjnslkdvnkjdqnkbjndsfkj");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", login.Id.ToString()),
                    new Claim("role", login.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private bool LoginExists(int id)
        {
            return _context.Logins.Any(e => e.Id == id);
        }
    }
}
