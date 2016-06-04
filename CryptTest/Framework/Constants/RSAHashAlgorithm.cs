/*
 * This file is part of CryptTest.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for full license information.
 *
 * CryptTest is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *
 */

namespace CryptTest.Framework.Constants
{
    /// <summary>
    /// Define which hash algorithm will be used in RSA
    /// </summary>
    public enum RSAHashAlgorithm
    {
        UNDEFINED = 0,
        SHA1      = 1,
        SHA384    = 2,
        SHA256    = 3,
        SHA512    = 4
    };
}
