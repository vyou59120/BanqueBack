using System.Security.Cryptography;
using System.Text;

namespace BanqueBack.Common
{
    public class Secure
    {
        public static string Encrypteur(string password)
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding encoding = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(encoding.GetBytes(password));

                for (int i = 0; i < result.Length; i++)
                {
                    stringBuilder.Append(result[i].ToString("x2"));
                }
            }
            Console.WriteLine(stringBuilder.ToString());
            return stringBuilder.ToString();
        }

    }
}
