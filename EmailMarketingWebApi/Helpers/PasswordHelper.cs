namespace EmailMarketingWebApi.Helpers
{
    using System;
    using System.Security.Cryptography;

    public class PasswordHelper
    {
        // Generate a new salt for each user and store it in the database
        public static (string PasswordHash, string Salt) HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);

                string salt = Convert.ToBase64String(saltBytes);

                using (var sha256 = new SHA256Managed())
                {
                    byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
                    byte[] passwordHashBytes = sha256.ComputeHash(passwordBytes);
                    string passwordHash = Convert.ToBase64String(passwordHashBytes);

                    return (passwordHash, salt);
                }
            }
        }

        // Validate the entered password against the stored password hash and salt
        public static bool VerifyPassword(string enteredPassword, string storedPasswordHash, string salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] enteredPasswordBytes = System.Text.Encoding.UTF8.GetBytes(enteredPassword + salt);
                byte[] enteredPasswordHashBytes = sha256.ComputeHash(enteredPasswordBytes);
                string enteredPasswordHash = Convert.ToBase64String(enteredPasswordHashBytes);

                return string.Equals(storedPasswordHash, enteredPasswordHash);
            }
        }
    }

}
