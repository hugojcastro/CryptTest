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

namespace CryptTest.Framework.Crypt
{
    /// <summary>
    /// Static class to UUEncode/UUDecode text strings
    /// </summary>
    public static class UUCoder
    {
        #region Methods
        /// <summary>
        /// UUEncode a string
        /// </summary>
        /// <param name="sBuffer">Input string to UUEncode</param>
        /// <returns>Output string with UUEncoded expression of input string</returns>
        public static string Encode(string input)
        {
            // Output string
            var output = string.Empty;
            // Input string must have length as multiple of 3. If not, just add padding spaces
            while (input.Length % 3 != 0)
            {
                input += ' ';
            }
            // Run accross input buffer, composing output in groups of 4 chars (obtained from 3 input bytes)
            for (int i = 1; i <= input.Length; i += 3)
            {
                output = string.Concat(output, Convert.ToString((char)(Convert.ToChar(input.Substring(i - 1, 1)) / 4  + 32)));
                output = string.Concat(output, Convert.ToString((char)(Convert.ToChar(input.Substring(i - 1, 1)) % 4  * 16 + Convert.ToChar(input.Substring(i,     1)) / 16 + 32)));
                output = string.Concat(output, Convert.ToString((char)(Convert.ToChar(input.Substring(i,     1)) % 16 * 4  + Convert.ToChar(input.Substring(i + 1, 1)) / 64 + 32)));
                output = string.Concat(output, Convert.ToString((char)(Convert.ToChar(input.Substring(i + 1, 1)) % 64 + 32)));
            }
            // And return composed value
            return output;
        }
        /// <summary>
        /// UUDecode a string
        /// </summary>
        /// <param name="sBuffer">Input string with UUEncoded string</param>
        /// <returns>Output string with UUDecoded data from input string</returns>
        public static string Decode(string input)
        {
            // Init. Ouput string
            var output = string.Empty;

            // Each 4 chars of input string will lead to 3 chars of output string
            for (int i = 1; i <= input.Length; i += 4)
            {
                output = string.Concat(output, Convert.ToString((char)((Convert.ToChar(input.Substring(i - 1, 1)) - 32) * 4  + (Convert.ToChar(input.Substring(i,     1)) - 32) / 16)));
                output = string.Concat(output, Convert.ToString((char)((Convert.ToChar(input.Substring(i,     1)) % 16 * 16) + (Convert.ToChar(input.Substring(i + 1, 1)) - 32) / 4)));
                output = string.Concat(output, Convert.ToString((char)((Convert.ToChar(input.Substring(i + 1, 1)) % 4 * 64)  +  Convert.ToChar(input.Substring(i + 2, 1)) - 32)));
            }
            return output;
        }
        #endregion
    }
}
