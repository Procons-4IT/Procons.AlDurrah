namespace Procons.Durrah.Common
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Security.Cryptography;

    public class Utilities
    {
        private static byte[] KEY_64 = { 42, 16, 93, 156, 78, 4, 218, 32 };
        private static byte[] IV_64 = { 55, 103, 246, 79, 36, 99, 167, 3 };

        public static string GetConfigurationValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Encrypt the parameter provided to this method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            if (!value.Equals(string.Empty))
            {
                DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateEncryptor(KEY_64, IV_64), CryptoStreamMode.Write);
                StreamWriter streamWriter = new StreamWriter(cryptoStream);
                streamWriter.Write(value);
                streamWriter.Flush();
                cryptoStream.FlushFinalBlock();
                memoryStream.Flush();
                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)(memoryStream.Length));
            }
            return string.Empty;
        }

        /// <summary>
        /// Decrypt the parameter provided to this method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Decrypt(string value)
        {
            if (!value.Equals(string.Empty))
            {
                DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
                byte[] buffer = Convert.FromBase64String(value);
                MemoryStream memoryStream = new MemoryStream(buffer);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateDecryptor(KEY_64, IV_64), CryptoStreamMode.Read);
                StreamReader streamWriter = new StreamReader(cryptoStream);
                return streamWriter.ReadToEnd();
            }
            return string.Empty;
        }


    }
}

