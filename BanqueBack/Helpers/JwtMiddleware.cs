using BanqueBack.Models;
using BanqueBack.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BanqueBack.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly BanqueContext _context = new BanqueContext();

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, token, userService);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token, IUserService userService)
        {
            try
            {
                var key = "blablavlkfdqjlkvndsjkfnbsdlkbnlkdqfnfbkjnslkdvnkjdqnkbjndsfkj";
                
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = securityKey,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                };

                if (ValidateToken(token, tokenValidationParameters))
                {
                    var TokenInfo = new Dictionary<string, string>();
                    var handler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = handler.ReadJwtToken(token);
                    var claims = jwtSecurityToken.Claims.ToList();

                    foreach (var claim in claims)
                    {
                        TokenInfo.Add(claim.Type, claim.Value);
                    }
                                  
                    string idUser = TokenInfo["id"];
                    string role = TokenInfo["role"];

                    context.Items["User"] = userService.GetLoginById(Int32.Parse(idUser));

                    //switch (role)
                    //{
                    //    case "CLIENT":
                    //        context.Items["User"] = userService.GetClientById(Int32.Parse(idUser));
                    //        break;
                    //    case "ADMIN":
                    //        context.Items["User"] = userService.GetDirecteurById(Int32.Parse(idUser));
                    //        break;
                    //    case "STAFF":
                    //        context.Items["User"] = userService.GetCommercialById(Int32.Parse(idUser));
                    //        break;
                    //    default:
                    //        context.Items["User"] = null;
                    //        break;
                    //}

                }               
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }

        private static bool ValidateToken(string token, TokenValidationParameters tvp)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                ClaimsPrincipal principal = handler.ValidateToken(token, tvp, out securityToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
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
    }
}
