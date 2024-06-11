using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
namespace BTP.Models.Auth
{
    public class Encodage
    {
        public static string toHas256(string mdp)
        {
            var mdpHasher = new StringBuilder();
            using var sha256 = SHA256.Create();

            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(mdp));

            for(int i = 0; i<bytes.Length; i++)
            {
                mdpHasher.Append(bytes[i].ToString("X2"));
            }

            return mdpHasher.ToString();
        }


        //mila mi-install @ nuget using BCrypt.Net;
        public static string HashPassword(string password)
    {
        // Génère un hachage bcrypt pour le mot de passe
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());

        return hashedPassword;
    }

      public static bool VerifyPassword(string password, string hashedPassword)
       {
            // Vérifie si le mot de passe correspond au hachage
          bool passwordMatches = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

          return passwordMatches;
      }
    }
}
