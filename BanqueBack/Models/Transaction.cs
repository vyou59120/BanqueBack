using System;
using System.Collections.Generic;

namespace BanqueBack.Models
{
    public partial class Transaction
    {
        public int Transactionid { get; set; }
        public int Accountid { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Montant { get; set; }
        public string? Operation { get; set; } = null!;
        public string? Description { get; set; } = null!;

        public virtual Account? Account { get; set; } = null!;
    }
}
