using System.Text;

namespace BanqueBack.Common
{
    public class Secure
    {
        public static string Key = "Saucisse";

        public static string Encrypteur(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";

            password += Key;
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }

        public static string Decrypteur(string cryptedPassword)
        {
            if (string.IsNullOrEmpty(cryptedPassword)) return "";
            var cryptedPasswordByte = Convert.FromBase64String(cryptedPassword);
            var result = Encoding.UTF8.GetString(cryptedPasswordByte);
            result = result.Substring(0, result.Length - Key.Length);
            return result;
        }
    }
}
