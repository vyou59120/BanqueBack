using System;
using System.Collections.Generic;

namespace BanqueBack.Models
{
    public partial class Login
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Motdepasse { get; set; } = null!;

        public Login(int id, string email, string role, string motdepasse)
        {
            Id = id;
            Email = email;
            Role = role;
            Motdepasse = motdepasse;
        }

    }
}
