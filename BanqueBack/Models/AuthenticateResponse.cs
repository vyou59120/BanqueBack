namespace BanqueBack.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.Userid;
            Role = user.Role;
            Token = token;
        }
    }
}
