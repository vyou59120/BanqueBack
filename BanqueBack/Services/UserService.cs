using BanqueBack.Helpers;
using BanqueBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BanqueBack.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        Task<ActionResult<IEnumerable<User>>> GetAll();
        Task<ActionResult<User>> GetById(int id);
        Task<ActionResult<int>> Post(User user);

        Task<ActionResult<int>> Delete(int id);

        Task<ActionResult<bool>> Exists(int id);
        Task<ActionResult<int>> Put(User user);
    }

    public class UserService : IUserService
    {
        private readonly BanqueContext _context = new BanqueContext();

        public UserService()
        {
            
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
           
            var user = _context.Users.SingleOrDefault(x => x.Email == model.Email && x.Motdepasse == Common.Secure.Encrypteur(model.Motdepasse));

            if (user == null) return null;

            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public async Task<ActionResult<int>> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public async Task<ActionResult<bool>> Exists(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (user == null)
            {
                return true;
            }

            return false;

            throw new NotImplementedException();
        }

        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return await _context.Users.ToListAsync();
            
        }

        public async Task<ActionResult<User>> GetById(int id)
        {

            if (id == null)
            {
                return null;
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (user == null)
            {
                return null;
            }

            return user;
            
        }

        public async Task<ActionResult<int>> Post(User user)
        {
            User temp = new User(user.Userid, user.Nom, user.Prenom, user.Adresse, user.Cp, user.Ville, user.Email, Common.Secure.Encrypteur(user.Motdepasse), user.Role, user.Datenaissance);
            _context.Users.Add(temp);

            return await _context.SaveChangesAsync();
        }


        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var key = Encoding.ASCII.GetBytes("blablavlkfdqjlkvndsjkfnbsdlkbnlkdqfnfbkjnslkdvnkjdqnkbjndsfkj");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Userid.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ActionResult<int>> Put(User user)
        {
            User temp = new User(user.Userid, user.Nom, user.Prenom, user.Adresse, user.Cp, user.Ville, user.Email, Common.Secure.Encrypteur(user.Motdepasse), user.Role, user.Datenaissance);
            _context.Users.Update(temp);
            return await _context.SaveChangesAsync();
        }

    }
}
