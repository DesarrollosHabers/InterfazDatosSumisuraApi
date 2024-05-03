using System.Security.Cryptography;
using System.Text;

namespace InterfazDatosSumisuraApi.Utilities
{
    public interface ITools
    {
        string ToolsSha256(string Password);
    }
    public class Tools : ITools
    {
        public string ToolsSha256(string Password)
        {
            byte[] bytesTextoOriginal = Encoding.UTF8.GetBytes(Password);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(bytesTextoOriginal);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                string hashCifrado = sb.ToString();

                Console.WriteLine("Texto original: " + Password);
                Console.WriteLine("Hash SHA-256: " + hashCifrado);
                return hashCifrado;
            }
        }
    }
}
