namespace BanqueBack.Models
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;

        public string Token { get; set; }

        public LoginResponse(int id, string email, string role, string token)
        {
            Id = id;
            Email = email;
            Role = role;
            Token = token;
        }
    }
}
