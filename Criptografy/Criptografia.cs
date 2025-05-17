using System.Security.Cryptography;
using System.Text;

namespace nanolink.Criptografy
{
    public class Criptografia
    {
        public static string GerarHash(string text)
        {
            var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            byte[] hash = sha256.ComputeHash(bytes);
            
            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}