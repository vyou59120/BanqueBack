using System;
using System.Collections.Generic;

namespace BanqueBack.Models
{
    public class Manager
    {
        public int managerid { get; set; }
        public string? Nom { get; set; } = null!;
        public string? Prenom { get; set; } = null!;
        public string? Adresse { get; set; } = null!;
        public string? Cp { get; set; } = null!;
        public string? Ville { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public DateTime? Datenaissance { get; set; }
    }
}
