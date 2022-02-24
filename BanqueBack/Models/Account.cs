using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BanqueBack.Models
{
    public partial class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Accountid { get; set; }
        public string? Numaccount { get; set; }
        public int Agenceid { get; set; }
        public int Userid { get; set; }
        public DateTime? Datecreation { get; set; }
        public decimal? Solde { get; set; }
        public DateTime? Datecloture { get; set; }

        public virtual Agence? Agence { get; set; } = null!;

        [JsonIgnore]
        public virtual User? User { get; set; } = null!;
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
