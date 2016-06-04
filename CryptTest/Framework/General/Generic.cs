/*
 * This file is part of CryptTest.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for full license information.
 *
 * CryptTest is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *
 */

using System.Collections.Generic;

namespace CryptTest.Framework.General
{
    /// <summary>
    /// Class for general methods
    /// </summary>
    public static class Generic
    {
        /// <summary>
        /// Checks in collection type if it's null or empty
        /// </summary>
        /// <typeparam name="T">Method extended for any type</typeparam>
        /// <param name="collection">A collection to check for</param>
        /// <returns>True if null or empty, False otherwise</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return ((collection == null) || (collection.Count == 0));
        }
    }
}
