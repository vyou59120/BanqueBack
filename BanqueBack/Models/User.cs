using System;
using System.Collections.Generic;

namespace BanqueBack.Models
{
    public partial class User
    {
        public User()
        {
            Accounts = new HashSet<Account>();
        }

        public int Userid { get; set; }
        public string? Nom { get; set; } = null!;
        public string? Prenom { get; set; } = null!;
        public string? Adresse { get; set; } = null!;
        public string? Cp { get; set; } = null!;
        public string? Ville { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? Motdepasse { get; set; } = null!;
        public string? Role { get; set; } = null!;
        public DateTimeOffset? Datenaissance { get; set; }

        public virtual ICollection<Account>? Accounts { get; set; }
    }
}
