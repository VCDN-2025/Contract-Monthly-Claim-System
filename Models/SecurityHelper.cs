using System.Security.Cryptography;
using System.Text;

namespace CMCS.Security
{
    /// <summary>
    /// Provides static methods for symmetric encryption and decryption using AES.
    /// </summary>
    public static class SecurityHelper
    {
        // Constant Field: 32-byte key used for AES-256 encryption.
        private static readonly string Key = "ThisIsAStrongAndSecure256BitKeyForCMCS";

        // Constant Field: 16-byte Initialization Vector (IV) used for AES encryption.
        private static readonly string IV = "ASimpleIVForAES16B";

        // Static Field: Byte array representation of the encryption key.
        private static readonly byte[] KeyBytes = Encoding.UTF8.GetBytes(Key);
        // Static Field: Byte array representation of the Initialization Vector (IV).
        private static readonly byte[] IVBytes = Encoding.UTF8.GetBytes(IV);

        /// <summary>
        /// Method: Encrypts a plain text string using AES-256 and returns a Base64-encoded result.
        /// </summary>
        /// <param name="plainText">The string to encrypt (e.g., serialized JSON).</param>
        /// <returns>The Base64 encoded encrypted string.</returns>
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = KeyBytes;
                aesAlg.IV = IVBytes;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // FIX: Must pass plainText to Write()
                            swEncrypt.Write(plainText);
                        }
                        // Return the encrypted bytes as a Base64 string for easy file storage
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Method: Decrypts a Base64-encoded, AES-encrypted string back to its original plain text.
        /// </summary>
        /// <param name="cipherText">The encrypted string (e.g., loaded from a file).</param>
        /// <returns>The decrypted plain text string.</returns>
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            string plainText;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = KeyBytes;
                aesAlg.IV = IVBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }
    }
}