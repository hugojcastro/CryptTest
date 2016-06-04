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
    /// Class to encrypt/decrypt text using streams and 3DES algorithm
    /// Just testing stuff...
    /// </summary>
    public static class CryptStream3DES
    {
        #region Properties
        // Fixed key and iv. Don't do this in your proyects: use custom key/iv values
        private const string key = "dont keep this info here";
        private const string iv  = "bad idea";
        #endregion

        #region Methods
        /// <summary>
        /// Encrypt a text using 3DES algorithm and custom key/iv values.
        /// </summary>
        /// <param name="text">Input text to encrypt.</param>
        /// <param name="Key">Key used to encrypt text. If none, use default.</param>
        /// <param name="IV">IV used to encrypt text. If none, use default.</param>
        /// <returns></returns>
        public static string Encrypt(string text, string Key = "", string IV = "")
        {
            var result  = "";
            var idx     = 0;
            // As we don't check key validity, just calculate a good one in case given one is invalid.
            var goodKey = (Key == "") ? key : Key;
            var fine    = ((goodKey.Length > 8) && (goodKey.Length % 8 == 0));
            if (!fine)
            {
                var limit = goodKey.Length + (8 - (goodKey.Length % 8));
                while (goodKey.Length != limit)
                    goodKey += key[idx++ % key.Length];
            }
            // Get a good IV based on given one or default one
            var ivBase = (IV == "") ? iv : IV;
            var goodIv = "";

            idx = 0;
            while (goodIv.Length < 8)
            {
                goodIv += ivBase[idx++ % ivBase.Length];
            }

            try
            {
                // Create a MemoryStream.
                using (var ms = new MemoryStream())
                {
                    using (var Algorithm = new TripleDESCryptoServiceProvider())
                    {
                        Algorithm.Key     = Encoding.ASCII.GetBytes(goodKey);
                        Algorithm.IV      = Encoding.ASCII.GetBytes(goodIv);
                        Algorithm.Padding = PaddingMode.PKCS7;
                        Algorithm.Mode    = CipherMode.ECB;

                        // Create a CryptoStream using the MemoryStream and the passed key and initialization vector (IV).
                        using (var cs = new CryptoStream(ms, Algorithm.CreateEncryptor(Algorithm.Key, Algorithm.IV), CryptoStreamMode.Write))
                        {
                            // Write the byte array to the crypto stream and flush it.
                            var data = Encoding.ASCII.GetBytes(text);

                            cs.Write(data, 0, data.Length);
                            cs.FlushFinalBlock();

                            // Get an array of bytes from the MemoryStream that holds the encrypted data.
                            result = Convert.ToBase64String(ms.ToArray());

                            // Close the streams.
                            cs.Close();
                            ms.Close();
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

        public static string Decrypt(string text, string Key = "", string IV = "")
        {
            var result  = "";
            var idx     = 0;
            var goodKey = (Key == "") ? key : Key;
            var fine    = ((goodKey.Length > 8) && (goodKey.Length % 8 == 0));
            // As we don't check key validity, just calculate a good one in case given one is invalid.
            if (!fine)
            {
                var limit = goodKey.Length + (8 - (goodKey.Length % 8));
                while (goodKey.Length != limit)
                    goodKey += key[idx++ % key.Length];
            }
            // Get a good IV based on given one or default one
            var ivBase = (IV == "") ? iv : IV;
            var goodIv = "";

            idx = 0;
            while (goodIv.Length < 8)
            {
                goodIv += ivBase[idx++ % ivBase.Length];
            }

            try
            {
                var data = Convert.FromBase64String(text);
                // Create a new MemoryStream using the passed array of encrypted data.
                using (var ms = new MemoryStream(data))
                {
                    using (var Algorithm = new TripleDESCryptoServiceProvider())
                    {
                        Algorithm.Key     = Encoding.ASCII.GetBytes(goodKey);
                        Algorithm.IV      = Encoding.ASCII.GetBytes(goodIv);
                        Algorithm.Padding = PaddingMode.PKCS7;
                        Algorithm.Mode    = CipherMode.ECB;
                        // Create a CryptoStream using the MemoryStream and the passed key and initialization vector (IV).
                        using (var cs = new CryptoStream(ms, Algorithm.CreateDecryptor(Algorithm.Key, Algorithm.IV), CryptoStreamMode.Read))
                        {
                            // Create buffer to hold the decrypted data.
                            byte[] value = new byte[data.Length];
                            // Read the decrypted data out of the crypto stream and place it into the temporary buffer.
                            cs.Read(value, 0, data.Length);

                            result = Encoding.ASCII.GetString(value);
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