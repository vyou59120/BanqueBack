using System.Text.Json.Serialization;

namespace BanqueBack.Models
{
    public class AccountUser
    {
        public  User? User { get; set; } = null!;
        [JsonIgnore]
        public  ICollection<Account>? Accounts { get; set; }
    }
}
