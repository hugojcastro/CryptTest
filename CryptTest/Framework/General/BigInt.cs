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
using System.Numerics;

/// <summary>
/// Common Big Integer operations.
/// Based on http://www.codeproject.com/Articles/421656/RSA-Library-with-Private-Key-Encryption-in-Csharp 
/// </summary>
namespace CryptTest.Framework.General
{
    public static class BigInt
    {
        /// <summary>
        /// Get byte array representation of Big Integer
        /// </summary>
        /// <param name="bi">The Big Integer</param>
        /// <param name="alength">Size of byte array representation</param>
        /// <param name="toLittleEndian">Representation as little endian or not</param>
        /// <returns>Byte array representation of Big Interger</returns>
        public static byte[] GetBytes(BigInteger bi, int alength, bool toLittleEndian = true)
        {
            byte[] array = new byte[alength];
            var idx = 0;

            while ((bi > 0) && (idx < alength))
            {
                array[idx++] = (byte)(bi % 256);
                bi /= 256;
            }

            if (!toLittleEndian)
                Array.Reverse(array);

            return array;
        }

        /// <summary>
        /// Converts a Big Integer byte array representation to its value
        /// </summary>
        /// <param name="array">Byte array representing the Big Integer</param>
        /// <param name="fromLittleEndian">Convention for byte array</param>
        /// <returns>Big Integer value of byte array representation</returns>
        public static BigInteger GetBigInt(byte[] array, bool fromLittleEndian = true)
        {
            BigInteger bi = 0;

            var delta = (fromLittleEndian) ? 1 : -1;
            var pos   = (fromLittleEndian) ? 0 : array.Length - 1;

            for (var i = 0; i < array.Length; i++)
            {
                bi  += BigInteger.Pow(256, i) * array[pos];
                pos += delta;
            }

            return bi;
        }

        /// <summary>
        /// Performs Bitwise Exclusive OR operation for two byte arrays.
        /// </summary>
        /// <param name="X">First byte array.</param>
        /// <param name="Y">Second byte array.</param>
        /// <returns>Bitwise Exclusive OR result.</returns>
        public static byte[] XOR(byte[] X, byte[] Y)
        {
            if (X.Length != Y.Length)
            {
                throw new ArgumentException("XOR: Parameter length mismatch");
            }
            else
            {
                byte[] result = new byte[X.Length];

                for (int i = 0; i < X.Length; i++)
                {
                    result[i] = (byte)(X[i] ^ Y[i]);
                }
                return result;
            }
        }
    }
}
