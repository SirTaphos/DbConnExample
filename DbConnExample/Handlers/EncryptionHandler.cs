using System;
using System.Security.Cryptography;
using System.Text;

namespace DbConnExample.Domain.Handlers
{
    public class EncryptionHandler
    {        
        private const int HashByteSize = 32;
        private const int HashByteIterationSize = 10101;
        private const int SaltByteSize = 24;

        /// <summary>
        /// For hashing, we are using Rfc2898DeriveBytes class which Implements password-based key derivation functionality, PBKDF2, by using a pseudo-random number generator based on HMACSHA1.
        /// Source: https://janaks.com.np/password-hashing-in-csharp/
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncryptPassword(string password, string salt)
        {
            Console.WriteLine("Calling EncryptPassword...");
            try
            {
                using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(salt), HashByteIterationSize))
                {
                    Console.WriteLine("Password has been encrypted");
                    return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(HashByteSize));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"EncryptPassword failed with message: {e}");
                throw;
            }
        }

        /// <summary>
        /// Compare stored password with inputted clear-text password (us this for login)
        /// </summary>
        /// <param name="encryptedPwd1"></param>
        /// <param name="pwd"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static bool ComparePasswords(string encryptedPwd1, string pwd, string salt)
        {
            Console.WriteLine("Calling ComparePasswords...");
            try
            {
                // Encrypt the password
                var encryptedPwd2 = EncryptPassword(pwd, salt);
                // Compare passwords in byte[]
                return encryptedPwd1.Equals(encryptedPwd2);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ComparePasswords failed with message: {e}");
                throw;
            }
        }

        /// <summary>
        /// Use this when generating random salt for use in the encryption function above
        /// </summary>
        /// <param name="saltByteSize"></param>
        /// <returns></returns>
        public static string GenerateSalt(int saltByteSize = SaltByteSize)
        {
            Console.WriteLine("Calling GenerateSalt...");
            try
            {
                using (var saltGenerator = new RNGCryptoServiceProvider())
                {
                    var salt = new byte[saltByteSize];
                    saltGenerator.GetBytes(salt);
                    return Convert.ToBase64String(salt);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"GenerateSalt failed with message: {e}");
                throw;
            }

        }
    }
}
