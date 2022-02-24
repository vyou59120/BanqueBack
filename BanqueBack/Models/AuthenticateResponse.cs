namespace BanqueBack.Models
{
    public class AuthenticateResponse
    {
        //public int Id { get; set; }
        //public string Token { get; set; }
        public int Id { get; set; }
        public string? Nom { get; set; } = null!;
        public string? Prenom { get; set; } = null!;
        public string? Adresse { get; set; } = null!;
        public string? Cp { get; set; } = null!;
        public string? Ville { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? Role { get; set; } = null!;
        public DateTime? Datenaissance { get; set; }

        public AuthenticateResponse(int id, string nom, string prenom, string adresse, string cp, string ville, string email, string role, DateTime? datenaissance)
        {
            //Token = token;
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Adresse = adresse;
            Cp = cp;
            Ville = ville;
            Email = email;
            Role = role;
            Datenaissance = datenaissance;
        }
        //public User? User { get; set; }
        //public Directeur? Directeur { get; set; }
        //public Commercial? Commercial { get; set; }

        //public AuthenticateResponse(User user, string token)
        //{
        //    Token = token;
        //    User = user;
        //}

        //public AuthenticateResponse(Directeur directeur, string token)
        //{
        //    Token = token;
        //    Directeur = directeur;
        //}

        //public AuthenticateResponse(Commercial commercial, string token)
        //{
        //    Token = token;
        //    Commercial = commercial;
        //}
    }
}
