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
using System.Security.Cryptography;
using System.Text;

namespace CryptTest.Framework.Crypt
{
    /// <summary>
    /// Static class to implement 3DES encryption/decryption algorithm
    /// We will use sistem servide providers. No third party libs, so maybe we have issues if not supported :S
    /// </summary>
    public static class Crypt3DES
    {
        #region Properties
        /// <summary>
        /// Hardcoded key used for algorithm. Do not use hardcoded values in your projects :(
        /// </summary>
        private const string Key = "this is a bad idea and practice";
        #endregion

        #region Methods
        /// <summary>
        /// Method to encrypt the given string using the specified key.
        /// </summary>
        /// <param name="strToEncrypt">The string to be encrypted.</param>
        /// <param name="strKey">The encryption key. Use default key if empty.</param>
        /// <returns>The encrypted string or error string if something gone wrong.</returns>
        public static string Encrypt(string strToEncrypt, string strKey = "")
        {
            var key    = (strKey == "") ? Key : strKey;
            var result = "";

            // Be safe, just in case system doesn't support it
            try
            {
                // Take a service provider from system: first, md5 hash
                using (var MD5Hash = new MD5CryptoServiceProvider())
                {
                    // Then, 3DES; Initialize it with our parameters
                    using (var DESCrypt = new TripleDESCryptoServiceProvider())
                    {
                        var seed = Encoding.ASCII.GetBytes(key);

                        DESCrypt.Key  = MD5Hash.ComputeHash(seed);
                        DESCrypt.IV   = BitConverter.GetBytes(CRC64.Hash(seed));
                        DESCrypt.Mode = CipherMode.ECB; // CBC, CFB

                        var byteBuff = Encoding.ASCII.GetBytes(strToEncrypt);

                        result = Convert.ToBase64String(DESCrypt.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
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
        /// Method to decrypt the given string using the specified key.
        /// </summary>
        /// <param name="strEncrypted">The string to be decrypted.</param>
        /// <param name="strKey">The decryption key. Use default key if empty.</param>
        /// <returns>The decrypted string or error string if something gone wrong.</returns>
        public static string Decrypt(string strEncrypted, string strKey = "")
        {
            var key    = (strKey == "") ? Key : strKey;
            var result = "";
            // Be safe, just in case system doesn't support it
            try
            {
                // Take a service provider from system: first, md5 hash
                using (var MD5Hash = new MD5CryptoServiceProvider())
                {
                    // Then 3DES; Initialize it with our parameters
                    using (var DESDecrypt = new TripleDESCryptoServiceProvider())
                    {
                        var seed = Encoding.ASCII.GetBytes(key);

                        DESDecrypt.Key  = MD5Hash.ComputeHash(seed);
                        DESDecrypt.IV   = BitConverter.GetBytes(CRC64.Hash(seed));
                        DESDecrypt.Mode = CipherMode.ECB; // or CBC... or CFB...

                        var byteBuff = Convert.FromBase64String(strEncrypted);

                        result = Encoding.ASCII.GetString(DESDecrypt.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
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