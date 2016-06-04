/*
 * This file is part of CryptTest.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for full license information.
 *
 * CryptTest is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptTest.Framework.Crypt
{
    /// <summary>
    /// Static class to encrypt/decrypt text using AES algorithm.
    /// A salt and a key string are provided but is not recommended to use them.
    /// </summary>
    public static class CryptAES
    {
        #region Properties
        // Hardcoded key and salt string. Do not use hardcoded values in your proyects :(
        private const string EncryptionKey  = "badwayforakey";
        private const string EncryptionSalt = "dontdothiskindofthigs";
        #endregion

        #region Methods
        /// <summary>
        /// Encrypt an input string using AES algorithm, with key and salt if provided.
        /// Use system services, so maybe something goes wrong if nothing is found.
        /// </summary>
        /// <param name="clearText">Text to encrypt in string format.</param>
        /// <param name="encryptionKey">String with encryption key.</param>
        /// <param name="encryptionSalt">String with salt used to encrypt.</param>
        /// <returns></returns>
        public static string Encrypt(string clearText, string encryptionKey = "", string encryptionSalt = "")
        {
            // Use default values if something is not provided
            var key    = (encryptionKey  == "") ? EncryptionKey  : encryptionKey;
            var salt   = (encryptionSalt == "") ? EncryptionSalt : encryptionSalt;
            var result = "";

            try
            {
                using (var encryptor = Aes.Create())
                {
                    using (var pdb = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(salt)))
                    {
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV  = pdb.GetBytes(16);

                        using (var ms = new MemoryStream())
                        {
                            using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                var clearBytes = Encoding.Unicode.GetBytes(clearText);
                                cs.Write(clearBytes, 0, clearBytes.Length);
                                cs.Close();
                                result = Convert.ToBase64String(ms.ToArray());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Exception thrown: " + ex.Message;
            }
            return result;
        }
        /// <summary>
        /// decrypt an input string using AES algorithm, with key and salt if provided.
        /// Use system services, so maybe something goes wrong if nothing is found.
        /// </summary>
        /// <param name="cipherText">Text to decrypt in string format.</param>
        /// <param name="encryptionKey">String with encryption key.</param>
        /// <param name="encryptionSalt">String with salt used to encrypt.</param>
        /// <returns></returns>
        public static string Decrypt(string cipherText, string encryptionKey = "", string encryptionSalt = "")
        {
            // Use default values if something is not provided
            var key    = (encryptionKey  == "") ? EncryptionKey  : encryptionKey;
            var salt   = (encryptionSalt == "") ? EncryptionSalt : encryptionSalt;
            var result = "";

            try
            {
                using (var decryptor = Aes.Create())
                {
                    using (var pdb = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(salt)))
                    {
                        decryptor.Key = pdb.GetBytes(32);
                        decryptor.IV  = pdb.GetBytes(16);

                        using (var ms = new MemoryStream())
                        {
                            using (var cs = new CryptoStream(ms, decryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                var cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                                result = Encoding.Unicode.GetString(ms.ToArray());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Exception thrown: " + ex.Message;
            }
            return result;
        }
        #endregion
    }
}
