/*
 * This file is part of CryptTest.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for full license information.
 *
 * CryptTest is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *
 */

using System.Text;

namespace CryptTest.Framework.Crypt
{
    /// <summary>
    /// Static class to get Adler32 Hash from input data
    /// Input can be byte array data or string
    /// Output will be an uint value
    /// </summary>
    public static class Adler32
    {
        #region Methods
        /// <summary>
        /// Get Adler32 hash from byte array
        /// </summary>
        /// <param name="input">Byte array with input data to get hash</param>
        /// <param name="a">Low part of Adler32. Default: 0</param>
        /// <param name="b">High part of Adler32. Default: 0</param>
        /// <returns>UInt with Adler32 Hash</returns>
        public static uint AdlerHash(byte[] input, uint a = 0, uint b = 0)
        {
            uint low  = a;
            uint high = b;
            for (int i = 0; i < input.Length; i++)
            {
                low  = (low + input[i]) % 65521;
                high = (high + a) % 65521;
            }
            return (low | (high << 16));
        }
        /// <summary>
        /// Get Adler32 hash from string
        /// </summary>
        /// <param name="input">String with input data to get hash</param>
        /// <returns>UInt with Adler32 Hash</returns>
        public static uint AdlerHash(string input)
        {
            // Just convert string to byte array and get its hash
            return AdlerHash(Encoding.ASCII.GetBytes(input));
        }
        #endregion
    }
}
