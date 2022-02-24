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
    public class UsersController : ControllerBase
    {
        private readonly BanqueContext _context;

        public UsersController(BanqueContext context)
        {
            _context = context;
        }

        //[HttpPost("authenticate")]
        //public AuthenticateResponse Authenticate(AuthenticateRequest model)
        //{
        //    var user = _context.Users.SingleOrDefault(x => x.Email == model.Email && x.Motdepasse == Common.Secure.Encrypteur(model.Motdepasse));

        //    if (user == null) return null;

        //    var token = generateJwtToken(user);

        //    return new AuthenticateResponse(user, token);
        //}

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("email/{email}")]
        public AuthenticateResponse GetUserByEmail(string email)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email);

            if (user == null) return null;

            //var token = generateJwtToken(user);

            return new AuthenticateResponse(user.Userid,user.Nom, user.Prenom, user.Adresse, user.Cp,user.Ville, user.Email, "CLIENT", user.Datenaissance);
        }

        [HttpGet("{id}/accounts")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserAccounts(int id)
        {
            var user = await _context.Users
                       .Where(s => s.Userid == id)
                       .Include(s => s.Accounts)
                       .ThenInclude(s => s.Transactions.OrderByDescending(o => o.Date))
                       .ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        //[HttpGet("{id}/accounts222")]
        //public async Task<AccountUser> GetUserAccounts2222(int id)
        //{
        //    var query = from user in _context.Users
        //                join account in _context.Accounts
        //                on user equals account.User
        //                where user.Userid == id
        //                select new AccountUser
        //                {
        //                    User = user,
        //                    Accounts = user.Accounts
        //                };

        //    AccountUser monuser = query.FirstOrDefault();
         
        //    return monuser;
        //}


        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Userid)
            {
                return BadRequest();
            }

            User temp = new User(user.Userid, user.Nom, user.Prenom, user.Adresse, user.Cp, user.Ville, user.Email, Common.Secure.Encrypteur(user.Motdepasse), user.Role, user.Datenaissance);

            _context.Users.Update(temp);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            User temp = new User(user.Nom, user.Prenom, user.Adresse, user.Cp, user.Ville, user.Email, user.Role, user.Datenaissance);

             _context.Users.Add(temp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = temp.Userid }, temp);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var key = Encoding.ASCII.GetBytes("blablavlkfdqjlkvndsjkfnbsdlkbnlkdqfnfbkjnslkdvnkjdqnkbjndsfkj");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", user.Userid.ToString()),
                    new Claim("role", "CLIENT")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Userid == id);
        }
    }
}
