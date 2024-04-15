using System.Security.Cryptography;
using System.Text;

public interface IPasswordService
{
    string HashPassword(string password);
}

public class PasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        // Implement password hashing logic here (using SHA256)
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
