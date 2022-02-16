using System;
using System.Collections.Generic;

namespace BanqueBack.Models
{
    public partial class Agence
    {
        public Agence()
        {
            Accounts = new HashSet<Account>();
        }

        public int Agenceid { get; set; }
        public string? Nomagence { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Ville { get; set; } = null!;

        public virtual ICollection<Account>? Accounts { get; set; }
    }
}
