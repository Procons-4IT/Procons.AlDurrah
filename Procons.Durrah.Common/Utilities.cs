﻿namespace Procons.Durrah.Common
{
    using NLog;
    using Procons.Durrah.Common.Enumerators;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading;

    public static class Utilities
    {
        private static byte[] KEY_64 = { 42, 16, 93, 156, 78, 4, 218, 32 };
        private static byte[] IV_64 = { 55, 103, 246, 79, 36, 99, 167, 3 };
        private static LogService _logService;
        public static LogService SiteLogService
        {
            get
            {
                if (_logService == null)
                    _logService = new LogService();
                return _logService;
            }
        }
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

        public static string RandomString()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 15)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetCurrentLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        }

        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static string GetLocalizedField(string fieldName)
        {
            var currentLanguage = GetCurrentLanguage();
            if (currentLanguage == Languages.Arabic.GetDescription())
                return $"{fieldName}_AR";
            else
                return fieldName;
        }

        public static void LogException(Exception ex)
        {
            SiteLogService.LogException(ex);
        }
    }
}

