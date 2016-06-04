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
    public sealed class CryptChaCha20 : IDisposable
    {
        #region Properties
        /// <summary>
        /// ChaCha20 is a specific implementation of salsa with 20 rounds
        /// You can do your own salsa/chacha versions just changing this :)
        /// </summary>
        private int numRounds { get; set; }
        /// <summary>
        /// The ChaCha20 state (aka "context")
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
        /// Set up a new ChaCha20 state. The lengths of the given parameters are 
        /// checked before encryption happens. 
        /// </summary>
        /// <remarks>
        /// See <a href="https://tools.ietf.org/html/rfc7539#page-10">ChaCha20 Spec Section 2.4</a>
        /// for a detailed description of the inputs. 
        /// </remarks>
        /// <param name="key">A 32-byte (256-bit) key, treated as a concatenation of eight 32-bit little-endian integers</param>
        /// <param name="nonce">A 8-byte (64-bit) nonce, treated as a concatenation of two 32-bit little-endian integers</param>
        /// <param name="rounds">Number of rounds used. Default, 20</param>
        /// <param name="blockcount">Number of blocks to process data. Default, 0</param>
        public CryptChaCha20(byte[] key, byte[] nonce, int rounds = 20, uint blockcount = 0)
        {
            // Check if bad arguments are passed
            if (key == null)
                throw new ArgumentNullException("Key is null!");
            if (nonce == null)
                throw new ArgumentNullException("Nonce is null!");
            if ((key.Length != 16) && (key.Length != 32))
                throw new ArgumentException("Key length must be 16 or 32 bytes. Actual is " + key.Length.ToString());
            if ((nonce.Length != 8) && (nonce.Length != 12))
                throw new ArgumentException("Nonce length should have 8 or 12 bytes. Actual is " + nonce.Length.ToString());
            // Init stuff...
            InitCryptChaCha20(key, nonce, rounds, blockcount);
        }

        /// <summary>
        /// Set up a new ChaCha20 state. The lengths of the given parameters are 
        /// checked before encryption happens. 
        /// </summary>
        /// <remarks>
        /// See <a href="https://tools.ietf.org/html/rfc7539#page-10">ChaCha20 Spec Section 2.4</a>
        /// for a detailed description of the inputs. 
        /// </remarks>
        /// <param name="key">A 32-byte (256-bit) key, treated as a concatenation of eight 32-bit little-endian integers</param>
        /// <param name="nonce">A 8-byte (64-bit) nonce, treated as a concatenation of two 32-bit little-endian integers</param>
        /// <param name="rounds">Number of rounds used. Default, 20</param>
        /// <param name="blockcount">Number of blocks to process data. Default, 0</param>
        public CryptChaCha20(string key, string nonce, int rounds = 20, uint blockcount = 0)
        {
            // Check if bad argument is passed
            if (key == "")
                throw new ArgumentNullException("Key is empty!");
            if (nonce == "")
                throw new ArgumentNullException("Nonce is empty!");
            // Store values
            var aKey   = Encoding.ASCII.GetBytes(key);
            var aNonce = Encoding.ASCII.GetBytes(nonce);
            // Check again
            if ((aKey.Length != 16) && (aKey.Length != 32))
                throw new ArgumentException("Key length must be 16 or 32 bytes. Actual is " + aKey.Length.ToString());
            if ((aNonce.Length != 8) && (aNonce.Length != 12))
                throw new ArgumentException("Nonce length should have 8 or 12 bytes. Actual is " + aNonce.Length.ToString());
            // Init stuff
            InitCryptChaCha20(aKey, aNonce, rounds, blockcount);
        }
        /// <summary>
        /// Init state for encrypt/decrypt process
        /// </summary>
        /// <param name="key">A 32-byte (256-bit) key, treated as a concatenation of eight 32-bit little-endian integers</param>
        /// <param name="nonce">A 8-byte (64-bit) nonce, treated as a concatenation of two 32-bit little-endian integers</param>
        /// <param name="rounds">Number of rounds used. Default, 20</param>
        /// <param name="blockcount">Number of blocks to process data. Default, 0</param>
        private void InitCryptChaCha20(byte[] key, byte[] nonce, int rounds = 20, uint blockcount = 0)
        {
            isDisposed = false;
            state      = new uint[16];
            numRounds  = rounds;

            // Initialize the Salsa state with the given key and nonce. A 32-byte (256-bit) key is required. 
            // The nonce must be at least 8-bytes (64-bits) long. If it is any larger, only the first 64 bits will be used. 
            // These are the same constants defined in the reference implementation
            // see http://cr.yp.to/streamciphers/timings/estreambench/submissions/salsa20/chacha8/ref/chacha.c
            var sigma     = Encoding.ASCII.GetBytes("expand 32-byte k");
            var tau       = Encoding.ASCII.GetBytes("expand 16-byte k");
            var constants = (key.Length == 32) ? sigma : tau;
            int offset    = (key.Length == 32) ? 16 : 0;
            // 12 byte nonce. If the given one has 8, just fill with zeros at the end
            var fullnonce = new byte[12];
            var init      = (nonce.Length == 8) ? 4 : 0;
            for (var i = 0; i < nonce.Length; i++)
            {
                fullnonce[init + i] = nonce[i];
            }
            // Add initializer to state
            state[ 0] = (uint)(((constants[0] | (constants[1] << 8)) | (constants[2] << 16)) | (constants[3] << 24));
            state[ 1] = (uint)(((constants[4] | (constants[5] << 8)) | (constants[6] << 16)) | (constants[7] << 24));
            state[ 2] = (uint)(((constants[8] | (constants[9] << 8)) | (constants[10] << 16)) | (constants[11] << 24));
            state[ 3] = (uint)(((constants[12] | (constants[13] << 8)) | (constants[14] << 16)) | (constants[15] << 24));
            // Add key to state
            state[ 4] = (uint)(((key[ 0] | (key[ 1] << 8)) | (key[ 2] << 16)) | (key[ 3] << 24));
            state[ 5] = (uint)(((key[ 4] | (key[ 5] << 8)) | (key[ 6] << 16)) | (key[ 7] << 24));
            state[ 6] = (uint)(((key[ 8] | (key[ 9] << 8)) | (key[10] << 16)) | (key[11] << 24));
            state[ 7] = (uint)(((key[12] | (key[13] << 8)) | (key[14] << 16)) | (key[15] << 24));
            // If key is 16 bytes long, repeat key here
            state[ 8] = (uint)(((key[offset +  0] | (key[offset +  1] << 8)) | (key[offset +  2] << 16)) | (key[offset +  3] << 24));
            state[ 9] = (uint)(((key[offset +  4] | (key[offset +  5] << 8)) | (key[offset +  6] << 16)) | (key[offset +  7] << 24));
            state[10] = (uint)(((key[offset +  8] | (key[offset +  9] << 8)) | (key[offset + 10] << 16)) | (key[offset + 11] << 24));
            state[11] = (uint)(((key[offset + 12] | (key[offset + 13] << 8)) | (key[offset + 14] << 16)) | (key[offset + 15] << 24));
            // Block counter
            state[12] = blockcount;
            // Add IV to state
            state[13] = (uint)(((fullnonce[0] | (fullnonce[1] << 8)) | (fullnonce[ 2] << 16)) | (fullnonce[ 3] << 24));
            state[14] = (uint)(((fullnonce[4] | (fullnonce[5] << 8)) | (fullnonce[ 6] << 16)) | (fullnonce[ 7] << 24));
            state[15] = (uint)(((fullnonce[8] | (fullnonce[9] << 8)) | (fullnonce[10] << 16)) | (fullnonce[11] << 24));
        }

        /// <summary>
        /// Encrypt an arbitrary-length plaintext message (input), writing the 
        /// resulting ciphertext to the output buffer. The number of bytes to read 
        /// from the input buffer is determined by numBytes. 
        /// </summary>
        /// <param name="input">Data to be processed as byte array</param>
        /// <returns>Byte array with processed data</returns>
        public byte[] ProcessBytes(byte[] input)
        {
            // Check if can run (not disposed)
            if (isDisposed)
                throw new ObjectDisposedException("state", "The ChaCha state has been cleared (i.e. Dispose() has been called)");

            var inputCount = input.Length;
            var offset     = 0;
            var block      = new byte[64];
            var output     = new byte[inputCount];
            var stateClone = (uint[])state.Clone();
            uint x;
            uint val;

            while (inputCount > 0)
            {
                // The Salsa20 Core Function reads a 64-byte vector x and produces a 64-byte vector Salsa20(x).
                // This is the basis of the Salsa20 Stream Cipher. 
                var v = (uint[])stateClone.Clone();

                for (int i = numRounds; i > 0; i -= 2)
                {
                    unchecked
                    {
                        v[00] += v[04]; x = v[12] ^ v[00]; v[12] = (x << 16) | (x >> 16);
                        v[08] += v[12]; x = v[04] ^ v[08]; v[04] = (x << 12) | (x >> 20);
                        v[00] += v[04]; x = v[12] ^ v[00]; v[12] = (x <<  8) | (x >> 24);
                        v[08] += v[12]; x = v[04] ^ v[08]; v[04] = (x <<  7) | (x >> 25);
                        v[01] += v[05]; x = v[13] ^ v[01]; v[13] = (x << 16) | (x >> 16);
                        v[09] += v[13]; x = v[05] ^ v[09]; v[05] = (x << 12) | (x >> 20);
                        v[01] += v[05]; x = v[13] ^ v[01]; v[13] = (x <<  8) | (x >> 24);
                        v[09] += v[13]; x = v[05] ^ v[09]; v[05] = (x <<  7) | (x >> 25);
                        v[02] += v[06]; x = v[14] ^ v[02]; v[14] = (x << 16) | (x >> 16);
                        v[10] += v[14]; x = v[06] ^ v[10]; v[06] = (x << 12) | (x >> 20);
                        v[02] += v[06]; x = v[14] ^ v[02]; v[14] = (x <<  8) | (x >> 24);
                        v[10] += v[14]; x = v[06] ^ v[10]; v[06] = (x <<  7) | (x >> 25);
                        v[03] += v[07]; x = v[15] ^ v[03]; v[15] = (x << 16) | (x >> 16);
                        v[11] += v[15]; x = v[07] ^ v[11]; v[07] = (x << 12) | (x >> 20);
                        v[03] += v[07]; x = v[15] ^ v[03]; v[15] = (x <<  8) | (x >> 24);
                        v[11] += v[15]; x = v[07] ^ v[11]; v[07] = (x <<  7) | (x >> 25);
                        v[00] += v[05]; x = v[15] ^ v[00]; v[15] = (x << 16) | (x >> 16);
                        v[10] += v[15]; x = v[05] ^ v[10]; v[05] = (x << 12) | (x >> 20);
                        v[00] += v[05]; x = v[15] ^ v[00]; v[15] = (x <<  8) | (x >> 24);
                        v[10] += v[15]; x = v[05] ^ v[10]; v[05] = (x <<  7) | (x >> 25);
                        v[01] += v[06]; x = v[12] ^ v[01]; v[12] = (x << 16) | (x >> 16);
                        v[11] += v[12]; x = v[06] ^ v[11]; v[06] = (x << 12) | (x >> 20);
                        v[01] += v[06]; x = v[12] ^ v[01]; v[12] = (x <<  8) | (x >> 24);
                        v[11] += v[12]; x = v[06] ^ v[11]; v[06] = (x <<  7) | (x >> 25);
                        v[02] += v[07]; x = v[13] ^ v[02]; v[13] = (x << 16) | (x >> 16);
                        v[08] += v[13]; x = v[07] ^ v[08]; v[07] = (x << 12) | (x >> 20);
                        v[02] += v[07]; x = v[13] ^ v[02]; v[13] = (x <<  8) | (x >> 24);
                        v[08] += v[13]; x = v[07] ^ v[08]; v[07] = (x <<  7) | (x >> 25);
                        v[03] += v[04]; x = v[14] ^ v[03]; v[14] = (x << 16) | (x >> 16);
                        v[09] += v[14]; x = v[04] ^ v[09]; v[04] = (x << 12) | (x >> 20);
                        v[03] += v[04]; x = v[14] ^ v[03]; v[14] = (x <<  8) | (x >> 24);
                        v[09] += v[14]; x = v[04] ^ v[09]; v[04] = (x <<  7) | (x >> 25);
                    }
                }

                for (uint i = 0; i < 16; i++)
                {
                    x = 4 * i;
                    unchecked
                    {
                        val = v[i] + stateClone[i];

                        block[x + 0] = (byte)val;
                        block[x + 1] = (byte)(val >> 8);
                        block[x + 2] = (byte)(val >> 16);
                        block[x + 3] = (byte)(val >> 24);
                    }
                }

                stateClone[12] = unchecked(stateClone[12] + 1);

                int blockSize = Math.Min(64, inputCount);

                for (int i = 0; i < blockSize; i++)
                {
                    output[offset + i] =
                        (byte)(input[offset + i] ^ block[i]);
                }

                inputCount -= 64;
                offset     += 64;
            }

            return output;
        }
        /// <summary>
        /// Wrapper to encrypt text using ChaCha20 algorythm
        /// </summary>
        /// <param name="plaintext">Input data to be encrypted</param>
        /// <returns>String with Base64 representation of encrypted data</returns>
        public string Encrypt(string plaintext)
        {
            // Convert chars to bytes, process them and convert result to Base64
            return Convert.ToBase64String(ProcessBytes(Encoding.ASCII.GetBytes(plaintext)));
        }
        /// <summary>
        /// Wrapper to decrypt text using ChaCha20 algorythm
        /// </summary>
        /// <param name="plaintext">String with Base64 representation of encrypted data</param>
        /// <returns>Decrypted text string</returns>
        public string Decrypt(string plaintext)
        {
            // Convert from Base64 and process bytes; Result is converted back to ASCII
            return Encoding.ASCII.GetString(ProcessBytes(Convert.FromBase64String(plaintext)));
        }

        #region Destructor and Disposer
        /// <summary>
        /// Clear and dispose of the internal state. The finalizer is only called 
        /// if Dispose() was never called on this cipher. 
        /// </summary>
        ~CryptChaCha20()
        {
            Dispose(false);
        }
        /// <summary>
        /// Clear and dispose of the internal state. Also request the GC not to 
        /// call the finalizer, because all cleanup has been taken care of. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            /* 
             * The Garbage Collector does not need to invoke the finalizer because 
             * Dispose(bool) has already done all the cleanup needed. 
             */
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// This method should only be invoked from Dispose() or the finalizer. 
        /// This handles the actual cleanup of the resources. 
        /// </summary>
        /// <param name="disposing">
        /// Should be true if called by Dispose(); false if called by the finalizer
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    /* Cleanup managed objects by calling their Dispose() methods */
                }
                /* Cleanup any unmanaged objects here */
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
