using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BanqueBack.Models
{
    public partial class Transaction
    {
        public int Transactionid { get; set; }
        public int Accountid { get; set; }
        public DateTimeOffset? Date { get; set; }
        public decimal? Montant { get; set; }
        public string? Operation { get; set; } = null!;
        public string? Description { get; set; } = null!;

        [JsonIgnore]
        public virtual Account? Account { get; set; } = null!;
    }
}
