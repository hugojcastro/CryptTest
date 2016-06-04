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
    /// My Salsa class ^^
    /// By default it implements Salsa20 algorithm.
    /// You can customize your cryptor by changing Round count and initial blockcount.
    /// </summary>
    public sealed class CryptSalsa : IDisposable
    {
        #region Properties
        /// <summary>
        /// Salsa rounds.
        /// Default: 20 to use Salsa20 algorithm.
        /// </summary>
        private uint numRounds { get; set; }
        /// <summary>
        /// The Salsa state (aka "context")
        /// </summary>
        private uint[] state { get; set; }
        /// <summary>
        /// Determines if the objects in this class have been disposed of. Set to 
        /// true by the Dispose() method. 
        /// </summary>
        private bool isDisposed { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Constructor for our Salsa implementation using byte data.
        /// By default, we'll use Salsa20. Using diferent values here allows you to change algo. behaviour ^^
        /// </summary>
        /// <param name="key">16 or 32 byte key as array of bytes (128 or 256 bits).</param>
        /// <param name="nonce">Initialization vector. At least 8 bytes long (32 bits).</param>
        /// <param name="rounds">number of rounds for block operations. 20 as default for Salsa20.</param>
        /// <param name="blockcount">number of blocks processed. 0 as default but can be any other value (i.e. 1)</param>
        public CryptSalsa(byte[] key, byte[] nonce, uint rounds = 20, uint blockcount = 0)
        {
            if (key == null)
                throw new ArgumentNullException("Key is null!");
            if (nonce == null)
                throw new ArgumentNullException("Nonce is null!");
            if ((key.Length != 16) && (key.Length != 32))
                throw new ArgumentException("Key length must be 16 or 32 bytes. Actual is " + key.Length.ToString());
            if (nonce.Length < 8)
                throw new ArgumentException("Nonce should have 8 bytes. Actual is " + nonce.Length.ToString());
            // Init vars to allow disposing
            isDisposed = false;
            // Assign rounds count
            numRounds = rounds;
            // Initialize all other vars
            Initialize(key, nonce, blockcount);
        }
        /// <summary>
        /// Constructor for our Salsa implementation using strings.
        /// </summary>
        /// <param name="key">String of 16 or 32 chars (128 or 256 bits).</param>
        /// <param name="nonce">Initialization string, at least 8 chars long (32 bits).</param>
        /// <param name="rounds">number of rounds for block operations. 20 as default for Salsa20.</param>
        /// <param name="blockcount">number of blocks processed. 0 as default but can be any other value (i.e. 1)</param>
        public CryptSalsa(string key, string nonce, uint rounds = 20, uint blockcount = 0)
        {
            if (key == "")
                throw new ArgumentNullException("Key is empty!");
            if (nonce == "")
                throw new ArgumentNullException("Nonce is empty!");
            if ((key.Length != 16) && (key.Length != 32))
                throw new ArgumentException("Key length must be 16 or 32 bytes. Actual is " + key.Length.ToString());
            if (nonce.Length < 8)
                throw new ArgumentException("Nonce should have at least 8 bytes. Actual is " + nonce.Length.ToString());
            // Init vars to allow disposing
            isDisposed = false;
            // Assing rounds count
            numRounds = rounds;
            // Initialize all other vars
            Initialize(Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(nonce), blockcount);
        }
        /// <summary>
        /// Common initializer for salsa state array
        /// </summary>
        /// <param name="key">16 or 32 byte key as array of bytes (128 or 256 bits).</param>
        /// <param name="nonce">Initialization vector of 8 bytes (32 bits).</param>
        /// <param name="blockcount">number of blocks processed. 0 as default but can be any other value (i.e. 1)</param>
        private void Initialize(byte[] key, byte[] nonce, uint blockcount)
        {
            // Initialize the Salsa state with the given key and nonce. A 16 or 32-byte (128 or 256-bit) key is required. 
            // The nonce must be at least 8-bytes (64-bits) long. If it is any larger, only the first 64 bits will be used. 
            // These are the same constants defined in the reference implementation
            // see http://cr.yp.to/streamciphers/timings/estreambench/submissions/salsa20/chacha8/ref/chacha.c
            byte[] sigma = Encoding.ASCII.GetBytes("expand 32-byte k");
            byte[] tau   = Encoding.ASCII.GetBytes("expand 16-byte k");
            // According to key length, use a constant key or another.
            byte[] constants = (key.Length == 32) ? sigma : tau;
            // We need 32 bytes for state. If key is 16 bytes long, we'll use it twice :)
            int keyIndex = key.Length - 16;
            // Create state data
            state = new uint[16];
            // In Salsa, uints 1-4 are used to store key if 16 bytes long or half if 32 bytes long
            state[ 1] = (uint)(((key[ 0] | (key[ 1] << 8)) | (key[ 2] << 16)) | (key[ 3] << 24));
            state[ 2] = (uint)(((key[ 4] | (key[ 5] << 8)) | (key[ 6] << 16)) | (key[ 7] << 24));
            state[ 3] = (uint)(((key[ 8] | (key[ 9] << 8)) | (key[10] << 16)) | (key[11] << 24));
            state[ 4] = (uint)(((key[12] | (key[13] << 8)) | (key[14] << 16)) | (key[15] << 24));
            // uints 11-14 are used to store second half for 32 bytes key or a copy if 16 bytes key
            state[11] = (uint)(((key[keyIndex     ] | (key[keyIndex +  1] << 8)) | (key[keyIndex +  2] << 16)) | (key[keyIndex +  3] << 24));
            state[12] = (uint)(((key[keyIndex +  4] | (key[keyIndex +  5] << 8)) | (key[keyIndex +  6] << 16)) | (key[keyIndex +  7] << 24));
            state[13] = (uint)(((key[keyIndex +  8] | (key[keyIndex +  9] << 8)) | (key[keyIndex + 10] << 16)) | (key[keyIndex + 11] << 24));
            state[14] = (uint)(((key[keyIndex + 12] | (key[keyIndex + 13] << 8)) | (key[keyIndex + 14] << 16)) | (key[keyIndex + 15] << 24));
            // Initialization constants are used for these positions: 0, 5, 10 and 15 (as delimiters/separators :P)
            state[ 0] = (uint)(((constants[ 0] | (constants[ 1] << 8)) | (constants[ 2] << 16)) | (constants[ 3] << 24));
            state[ 5] = (uint)(((constants[ 4] | (constants[ 5] << 8)) | (constants[ 6] << 16)) | (constants[ 7] << 24));
            state[10] = (uint)(((constants[ 8] | (constants[ 9] << 8)) | (constants[10] << 16)) | (constants[11] << 24));
            state[15] = (uint)(((constants[12] | (constants[13] << 8)) | (constants[14] << 16)) | (constants[15] << 24));
            // Initialization key is used for uints  6 & 7. If it's bigger than 8 bytes, the other ones won't be used
            state[ 6] = (uint)(((nonce[0] | (nonce[1] << 8)) | (nonce[2] << 16)) | (nonce[3] << 24));
            state[ 7] = (uint)(((nonce[4] | (nonce[5] << 8)) | (nonce[6] << 16)) | (nonce[7] << 24));
            // Position 8 & 9 are used to store blockcount when crypting (2^64-1 rounds... wow)
            state[ 8] = blockcount;
            state[ 9] = 0;
        }
        /// <summary>
        /// Encrypt an input array of bytes and give encrypted output as result
        /// As Salsa algorithm is a XOR one (symetrical), using same initial state allows encrypting and decrypting with same function.
        /// Encryption/decryption is done in blocks of 64 bytes.
        /// </summary>
        /// <param name="input">Array of bytes to encrypt</param>
        /// <returns>An array of bytes of same length as input array, with processed data.</returns>
        public byte[] ProcessBytes(byte[] input)
        {
            // Check if disposed. If so, can't do much more :/
            if (isDisposed)
                throw new ObjectDisposedException("state", "The Salsa state has been cleared (i.e. Dispose() has been called)");
            // Oke, all fine. Let's initialize some data...
            // Input size (output data will have same size)
            var inputCount = input.Length;
            // We'll do the same stuff in 64 byte blocks
            var offset     = 0;
            // After doing some things with state array, we'll use that data to XOR input data
            var block      = new byte[64];
            // Guess what's this? O:)
            var output     = new byte[inputCount];
            // As all data is processed in this function, we will use a copy of original state data to allow encryption and decryptio
            // with the same instance
            var stateClone = (uint[])state.Clone();
            // temp. vars ^^
            uint x;
            uint val;
            // Main loop. Do stuff in 64-bytes blocks
            while (inputCount > 0)
            {
                // Take a copy of the state 
                var v = (uint[])stateClone.Clone();
                // Do rounds add'ing/roll'ing/xor'ing data
                for (uint i = numRounds; i > 0; i -= 2)
                {
                    // Note: we'll use this to allow overflow in some operations
                    unchecked
                    {
                        x = v[00] + v[12]; v[04] ^= ((x << 07) | (x >> 25));
                        x = v[04] + v[00]; v[08] ^= ((x << 09) | (x >> 23));
                        x = v[08] + v[04]; v[12] ^= ((x << 13) | (x >> 19));
                        x = v[12] + v[08]; v[00] ^= ((x << 18) | (x >> 14));
                        x = v[05] + v[01]; v[09] ^= ((x << 07) | (x >> 25));
                        x = v[09] + v[05]; v[13] ^= ((x << 09) | (x >> 23));
                        x = v[13] + v[09]; v[01] ^= ((x << 13) | (x >> 19));
                        x = v[01] + v[13]; v[05] ^= ((x << 18) | (x >> 14));
                        x = v[10] + v[06]; v[14] ^= ((x << 07) | (x >> 25));
                        x = v[14] + v[10]; v[02] ^= ((x << 09) | (x >> 23));
                        x = v[02] + v[14]; v[06] ^= ((x << 13) | (x >> 19));
                        x = v[06] + v[02]; v[10] ^= ((x << 18) | (x >> 14));
                        x = v[15] + v[11]; v[03] ^= ((x << 07) | (x >> 25));
                        x = v[03] + v[15]; v[07] ^= ((x << 09) | (x >> 23));
                        x = v[07] + v[03]; v[11] ^= ((x << 13) | (x >> 19));
                        x = v[11] + v[07]; v[15] ^= ((x << 18) | (x >> 14));
                        x = v[00] + v[03]; v[01] ^= ((x << 07) | (x >> 25));
                        x = v[01] + v[00]; v[02] ^= ((x << 09) | (x >> 23));
                        x = v[02] + v[01]; v[03] ^= ((x << 13) | (x >> 19));
                        x = v[03] + v[02]; v[00] ^= ((x << 18) | (x >> 14));
                        x = v[05] + v[04]; v[06] ^= ((x << 07) | (x >> 25));
                        x = v[06] + v[05]; v[07] ^= ((x << 09) | (x >> 23));
                        x = v[07] + v[06]; v[04] ^= ((x << 13) | (x >> 19));
                        x = v[04] + v[07]; v[05] ^= ((x << 18) | (x >> 14));
                        x = v[10] + v[09]; v[11] ^= ((x << 07) | (x >> 25));
                        x = v[11] + v[10]; v[08] ^= ((x << 09) | (x >> 23));
                        x = v[08] + v[11]; v[09] ^= ((x << 13) | (x >> 19));
                        x = v[09] + v[08]; v[10] ^= ((x << 18) | (x >> 14));
                        x = v[15] + v[14]; v[12] ^= ((x << 07) | (x >> 25));
                        x = v[12] + v[15]; v[13] ^= ((x << 09) | (x >> 23));
                        x = v[13] + v[12]; v[14] ^= ((x << 13) | (x >> 19));
                        x = v[14] + v[13]; v[15] ^= ((x << 18) | (x >> 14));
                    }
                }
                // After initial "scrambling", just create the block to XOR the input
                for (uint i = 0; i < 16; i++)
                {
                    // State are uint values; we need byte values so... each uint is a 4 byte group
                    x = 4 * i;
                    unchecked
                    {
                        // One last operation :P
                        val = v[i] + stateClone[i];

                        block[x + 0] = (byte)val;
                        block[x + 1] = (byte)(val >> 8);
                        block[x + 2] = (byte)(val >> 16);
                        block[x + 3] = (byte)(val >> 24);
                    }
                }
                // A new block is done. Update block count in state
                stateClone[8] = unchecked(stateClone[8] + 1);
                // Wow... 2^32 blocks done? well... let's keep counting... ^^
                if (stateClone[8] == 0)
                {
                    // Stopping at 2^70 bytes per nonce is the user's responsibility
                    stateClone[9] = unchecked(stateClone[9] + 1);
                }
                // Here we go... Encrypt our input data
                int blockSize = Math.Min(64, inputCount);
                // For each byte of data, XOR it with our magic values
                for (int i = 0; i < blockSize; i++)
                {
                    output[offset + i] =
                        (byte)(input[offset + i] ^ block[i]);
                }
                // And keep rolling ^^
                inputCount -= 64;
                offset     += 64;
            }
            // All done. Return our precious data :)
            return output;
        }
        /// <summary>
        /// Encrypt an input string using our state data.
        /// To encrypt byte data use raw ProcessBytes function :)
        /// </summary>
        /// <param name="plaintext">String to be encrypted</param>
        /// <returns>A string with encrypted data in Base64 format</returns>
        public string Encrypt(string plaintext)
        {
            // Take string bytes, encrypt them and return byte array result as string in Base64 format
            return Convert.ToBase64String(ProcessBytes(Encoding.ASCII.GetBytes(plaintext)));
        }
        /// <summary>
        /// Decrypt an input string using our state data.
        /// To decrypt byte data use raw ProcessBytes funcion :)
        /// </summary>
        /// <param name="plaintext">String to be decrypted. It must be a Base64 representation of encrypted data.</param>
        /// <returns>A string with decrypted data</returns>
        public string Decrypt(string plaintext)
        {
            // Take string bytes from Base64, decrypt them and return byte array result as string in ASCII format
            return Encoding.ASCII.GetString(ProcessBytes(Convert.FromBase64String(plaintext)));
        }

        #region Destructor and Disposer
        /// <summary>
        /// Clear and dispose of the internal state.
        /// The finalizer is only called if Dispose() was never called on this cipher. 
        /// </summary>
        ~CryptSalsa()
        {
            Dispose(false);
        }
        /// <summary>
        /// Clear and dispose of the internal state.
        /// Also request the GC not to call the finalizer, because all cleanup has been taken care of. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // The Garbage Collector does not need to invoke the finalizer because Dispose(bool) has already done all the cleanup needed. 
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// This method should only be invoked from Dispose() or the finalizer. 
        /// This handles the actual cleanup of the resources. 
        /// </summary>
        /// <param name="disposing">Should be true if called by Dispose(); false if called by the finalizer</param>
        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // Cleanup managed objects by calling their Dispose() methods
                }
                // Cleanup any unmanaged objects here
                if (state != null)
                {
                    Array.Clear(state, 0, state.Length);
                }
                state = null;
            }
            isDisposed = true;
        }
        #endregion
        #endregion
    }
}
