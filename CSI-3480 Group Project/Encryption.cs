using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CSI_3480_Group_Project
{
    internal class Encryption
    {
        public static string Encrypt(string password, string masterPassword)
        {
            byte[] encryptedBytes;
            byte[] iv;

            using (Aes aes = Aes.Create())
            {
                aes.Key = DeriveKey(masterPassword);
                aes.GenerateIV();
                iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(password);
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            byte[] result = new byte[encryptedBytes.Length + iv.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);
            
            return Convert.ToBase64String(result);
        }

        public static string Decrypt(string encryptedPassword, string masterPassword)
        {
            byte[] encryptedBytesWithIv = Convert.FromBase64String(encryptedPassword);
            byte[] iv = new byte[16];
            byte[] encryptedBytes = new byte[encryptedBytesWithIv.Length - iv.Length];
            Buffer.BlockCopy(encryptedBytesWithIv, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytesWithIv, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

            using (Aes aes = Aes.Create())
            {
                aes.Key = DeriveKey(masterPassword);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(encryptedBytes))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static byte[] DeriveKey(string masterPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(masterPassword));
            }
        }
    }
}
