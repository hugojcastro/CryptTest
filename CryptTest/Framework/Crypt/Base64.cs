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
using System.Text;

namespace CryptTest.Framework.Crypt
{
    /// <summary>
    /// Static class to obtain Base64 (en)cryption of text
    /// </summary>
    public static class Base64
    {
        #region Methods
        /// <summary>
        /// Gets Base64 text encryption of input text
        /// </summary>
        /// <param name="text">Input text</param>
        /// <returns>String with Base64 encryption of input text</returns>
        public static string EncodeString(string text)
        {
            // Just call standard Convert method with general encoding for text
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }
        /// <summary>
        /// Gets plain text decrypted from Base64 string
        /// </summary>
        /// <param name="text">Input Base64 text</param>
        /// <returns>Decrypted plain text from Base64 text</returns>
        public static string DecodeString(string text)
        {
            // Just call standard Convert method with general encoding for text
            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }
        /// <summary>
        /// Obtains Base64 string text from byte array input data
        /// </summary>
        /// <param name="data">Byte array with data to encrypt</param>
        /// <returns>String with Base64 text encryption of input data</returns>
        public static string EncodeBytes(byte[] data)
        {
            // Just call standard Convert method
            return Convert.ToBase64String(data);
        }
        /// <summary>
        /// Obtain byte array data from Base64 input text string
        /// </summary>
        /// <param name="data">String in Base64 from encrypted data</param>
        /// <returns>Byte array with decrypted data from Base64 string</returns>
        public static byte[] DecodeBytes(string data)
        {
            // Just call standard Convert method
            return Convert.FromBase64String(data);
        }
        #endregion
    }
}
