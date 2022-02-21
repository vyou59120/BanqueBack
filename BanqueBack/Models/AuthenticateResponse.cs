namespace BanqueBack.Models
{
    public class AuthenticateResponse
    {
        //public int Id { get; set; }
        public string Token { get; set; }
        //public string? Nom { get; set; } = null!;
        //public string? Prenom { get; set; } = null!;
        //public string? Adresse { get; set; } = null!;
        //public string? Cp { get; set; } = null!;
        //public string? Ville { get; set; } = null!;
        //public string? Email { get; set; } = null!;
        //public string? Role { get; set; } = null!;
        //public DateTime? Datenaissance { get; set; }

        public User User { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            //Id = user.Userid;
            //Role = user.Role;
            Token = token;
            //Nom = user.Nom;
            //Prenom = user.Prenom;
            //Adresse = user.Adresse;
            //Cp = user.Cp;
            //Ville = user.Ville;
            //Email = user.Email;
            //Datenaissance = user.Datenaissance;
            User = user;
        }
    }
}
