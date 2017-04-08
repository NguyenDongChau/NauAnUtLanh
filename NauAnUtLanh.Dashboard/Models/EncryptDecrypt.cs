using System;
using System.Security.Cryptography;
using System.Text;

namespace NauAnUtLanh.Dashboard.Models
{
    public static class EncryptDecrypt
    {
        public static string GetMd5(string plain)
        {
            var md5Hasher = new MD5CryptoServiceProvider();
            var encoder = new UTF8Encoding();
            var hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(plain));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        public static bool CompareTwoMd5String(string plain, string md5)
        {
            var strMd5 = GetMd5(plain);
            return string.Equals(md5, strMd5, StringComparison.CurrentCulture);
        }
    }
}
