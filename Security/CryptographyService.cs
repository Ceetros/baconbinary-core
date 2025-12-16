using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BaconBinary.Core.Security
{
    public static class CryptographyService
    {
        private const int KeySize = 256;
        private const int BlockSize = 128;

        /// <summary>
        /// Derives a 32-byte key from a user-provided password string using SHA256.
        /// </summary>
        private static byte[] DeriveKey(string password)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        /// <summary>
        /// Encrypts a byte array using AES-256-CBC.
        /// </summary>
        /// <param name="dataToEncrypt">The raw data to encrypt.</param>
        /// <param name="key">The user-provided password.</param>
        /// <param name="iv">The generated initialization vector.</param>
        /// <returns>The encrypted data.</returns>
        public static byte[] Encrypt(byte[] dataToEncrypt, string key, out byte[] iv)
        {
            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            aes.Key = DeriveKey(key);
            aes.GenerateIV();
            iv = aes.IV;

            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                cryptoStream.FlushFinalBlock();
            }
            
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Decrypts a byte array using AES-256-CBC.
        /// </summary>
        /// <param name="dataToDecrypt">The encrypted data.</param>
        /// <param name="key">The user-provided password.</param>
        /// <param name="iv">The initialization vector read from the file header.</param>
        /// <returns>The decrypted raw data.</returns>
        public static byte[] Decrypt(byte[] dataToDecrypt, string key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            aes.Key = DeriveKey(key);
            aes.IV = iv;

            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                cryptoStream.FlushFinalBlock();
            }

            return memoryStream.ToArray();
        }
    }
}
