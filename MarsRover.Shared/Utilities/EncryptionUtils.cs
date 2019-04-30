using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MarsRover.Shared.Utilities
{
    //TODO:KudretKurt-->Shared should not be decompile.
    public sealed class EncryptionUtils
    {
        private static readonly Lazy<EncryptionUtils> Lazy =
            new Lazy<EncryptionUtils>(() => new EncryptionUtils());

        public static EncryptionUtils Instance => Lazy.Value;
        private static SymmetricAlgorithm SymmetricAlgorithm { get; set; }
        private static string Password => "40abd4d1-1b1d-4799-88a3-0b57a134";
        private static byte[] Salt => Encoding.UTF8.GetBytes("40abd4d1-1b1d-4799-88a3-0b57a135");

        private EncryptionUtils()
        {
            SymmetricAlgorithm = new RijndaelManaged
            {
                IV = Encoding.UTF8.GetBytes("@1B2c3D4e5F6g7H8"),
                KeySize = 256,
                Mode = CipherMode.CBC
            };
        }

        public string Encrypt(string plainText)
        {
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                var passwordDeriveBytes = new Rfc2898DeriveBytes(Password, Salt);
                var keyBytes = passwordDeriveBytes.GetBytes(SymmetricAlgorithm.KeySize / 8);
                var encryptor = SymmetricAlgorithm.CreateEncryptor(keyBytes, SymmetricAlgorithm.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        var cipherTextBytes = memoryStream.ToArray();
                        return Convert.ToBase64String(cipherTextBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CryptographicException($"Encrypt failed:{ex.Message}");
            }
        }

        public string Decrypt(string cipherText)
        {
            try
            {
                var cipherTextBytes = Convert.FromBase64String(cipherText);
                var passwordDeriveBytes = new Rfc2898DeriveBytes(Password, Salt);
                var keyBytes = passwordDeriveBytes.GetBytes(SymmetricAlgorithm.KeySize / 8);
                var decryptor = SymmetricAlgorithm.CreateDecryptor(keyBytes, SymmetricAlgorithm.IV);


                using (var memoryStream = new MemoryStream(cipherTextBytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        var plainTextBytes = cipherTextBytes;
                        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        var val = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CryptographicException($"Decrypt failed:{ex.Message}");
            }
        }

        public IDictionary<string, string> DecryptConfigurationJson(IDictionary<string, string> cipherDictionary)
        {
            try
            {
                return cipherDictionary.ToDictionary(cipher => cipher.Key, cipher => Decrypt(cipher.Value));
            }
            catch (Exception ex)
            {
                throw new CryptographicException($"Decrypt failed:{ex.Message}");
            }
        }
    }
}
