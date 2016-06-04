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
using System.Security.Cryptography;
using System.Text;
using System.Xml;

using CryptTest.Framework.Constants;
using CryptTest.Framework.General;

namespace CryptTest.Framework.Crypt
{
    /// <summary>
    /// Static class to implement RSA certificates for encryption/decryption.
    /// It uses RSACert class to make its magic :)
    /// References:
    /// https://msdn.microsoft.com/en-us/library/bb397867(v=vs.110).aspx
    /// https://msdn.microsoft.com/es-es/library/system.security.cryptography.rsacryptoserviceprovider(v=vs.110).aspx
    /// http://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET
    /// http://www.technical-recipes.com/2013/using-rsa-to-encrypt-large-data-files-in-c/
    /// </summary>
    public class CryptRSA : IDisposable
    {
        #region Properties
        /// <summary>
        /// Determines if the objects in this class have been disposed of. Set to true by the Dispose() method.
        /// No need to made this class IDisposable, but I like "using" clauses :D
        /// </summary>
        private bool isDisposed { get; set; } = false;
        /// <summary>
        /// Using Optimal Asymmetric Encryption Padding? Recommended, but not today
        /// </summary>
        private bool useOAEP { get; } = false;

        // Private: vars to be used as ReadOnly and flags
        private int hashLength { get; set; } = 0;
        private RSAHashAlgorithm hashAlgorithm { get; set; } = RSAHashAlgorithm.UNDEFINED;
        private bool HasPublicInfo { get; set; } = false;
        private bool HasPrivateInfo { get; set; } = false;
        private bool HasCRTInfo { get; set; } = false;
        // Public
        // BigIntegers to do math on data and other vars
        private BigInteger E { get; set; }
        private BigInteger N { get; set; }
        private BigInteger D { get; set; }
        private BigInteger P { get; set; }
        private BigInteger Q { get; set; }
        private BigInteger DP { get; set; }
        private BigInteger DQ { get; set; }
        private BigInteger IQ { get; set; }
        private int ModulusSize { get; } = 0;
        private int HashLength { get { return hashLength; } }
        private int maxDataLength { get; set; } = 0;
        private RSAHashAlgorithm Hash { get { return hashAlgorithm; } }
        #endregion

        #region Methods

        public CryptRSA(string _XMLKeyInfo, int keySize, string _hash = "SHA1")
        {
            if (string.IsNullOrEmpty(_XMLKeyInfo)) throw new ArgumentException("XmlKey is null or empty");
            if (!isKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid");
            maxDataLength = ((keySize - 384) / 8) + ((useOAEP) ? 7 : 37);

            // Parse XML document using system services:
            // I'll use try/catch clauses: if some data is missing, user will be able to continue execution but knowing what happened.
            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(_XMLKeyInfo);
            }
            catch (Exception ex)
            {
                throw new Exception("Malformed KeyInfo XML: " + ex.Message);
            }
            // All fine? get values: First of all, look for public info
            try
            {
                N = BigInt.GetBigInt(Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("Modulus").InnerText), false);
                E = BigInt.GetBigInt(Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("Exponent").InnerText), false);
                HasPublicInfo = true;
            }
            catch
            {
                N = 0;
                E = 0;
                HasPublicInfo = false;
            }
            // Then, private info
            try
            {
                D = BigInt.GetBigInt(Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("D").InnerText), false);
                HasPrivateInfo = true;
            }
            catch
            {
                D = 0;
                HasPrivateInfo = false;
            }
            // And compose CERT data
            try
            {
                P  = BigInt.GetBigInt(Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("P").InnerText), false);
                Q  = BigInt.GetBigInt(Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("Q").InnerText), false);
                DP = BigInt.GetBigInt(Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("DP").InnerText), false);
                DQ = BigInt.GetBigInt(Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("DQ").InnerText), false);
                IQ = BigInt.GetBigInt(Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("InverseQ").InnerText), false);
                HasCRTInfo = true;
            }
            catch
            {
                P  = 0;
                Q  = 0;
                DP = 0;
                DQ = 0;
                IQ = 0;
                HasCRTInfo = false;
            }
            ModulusSize = keySize / 8;

            SetHash(_hash);
        }

        public void SetHash(string _hash)
        {
            switch (_hash)
            {
                case "SHA1":
                    hashAlgorithm = RSAHashAlgorithm.SHA1;
                    hashLength = 20;
                    break;
                case "SHA256":
                    hashAlgorithm = RSAHashAlgorithm.SHA256;
                    hashLength = 32;
                    break;
                case "SHA384":
                    hashAlgorithm = RSAHashAlgorithm.SHA384;
                    hashLength = 48;
                    break;
                case "SHA512":
                    hashAlgorithm = RSAHashAlgorithm.SHA512;
                    hashLength = 64;
                    break;
                default:
                    hashAlgorithm = RSAHashAlgorithm.UNDEFINED;
                    hashLength = 0;
                    break;
            }
        }
        public string GetHash()
        {
            var result = "Unknown";
            switch (hashAlgorithm)
            {
                case RSAHashAlgorithm.SHA1:
                    result = "SHA1";
                    break;
                case RSAHashAlgorithm.SHA256:
                    result = "SHA256";
                    break;
                case RSAHashAlgorithm.SHA384:
                    result = "SHA384";
                    break;
                case RSAHashAlgorithm.SHA512:
                    result = "SHA512";
                    break;
            }
            return result;
        }
        public byte[] ComputeHash(byte[] data)
        {
            switch (Hash)
            {
                case RSAHashAlgorithm.SHA1:
                    using (var sham = SHA1.Create())
                    {
                        return sham.ComputeHash(data);
                    }
                case RSAHashAlgorithm.SHA256:
                    using (var sham = SHA256.Create())
                    {
                        return sham.ComputeHash(data);
                    }
                case RSAHashAlgorithm.SHA384:
                    using (var sham = SHA384.Create())
                    {
                        return sham.ComputeHash(data);
                    }
                case RSAHashAlgorithm.SHA512:
                    using (var sham = SHA512.Create())
                    {
                        return sham.ComputeHash(data);
                    }
            }
            return null;
        }

        /// <summary>
        /// Check if input value is a valid key size
        /// Key size is multiplier of eight and a value in range [384, 16384]
        /// </summary>
        /// <param name="keySize">Value to check</param>
        /// <returns>True if valid size, false otherwise</returns>
        private bool isKeySizeValid(int keySize)
        {
            return ((keySize >= 384) && (keySize <= 16384) && (keySize % 8 == 0));
        }
        /// <summary>
        /// Generate XML texts with certificate data.
        /// Take 2 string parameters to return XML text with public only data or full certificate data
        /// </summary>
        /// <param name="keySize">Size of key to use</param>
        /// <param name="publicKey">Output text with public data only</param>
        /// <param name="publicAndPrivateKey">Output text with full data</param>

        public static void GenerateKeys(int keySize, out string publicKey, out string publicAndPrivateKey)
        {
            // Uses system service to create cert. data.
            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                // And returns them into parameters
                publicAndPrivateKey = provider.ToXmlString(true);
                publicKey           = provider.ToXmlString(false);
            }
        }

        /// <summary>
        /// Encrypt a text with the given key in XML format.
        /// Can be used to encrypt using both private or public key, depending on data into XML.
        /// </summary>
        /// <param name="text">Plain text to encrypt</param>
        /// <param name="keySize">Size of key to use</param>
        /// <param name="XmlKey">Text with key to use in XML format (private or public)</param>
        /// <param name="usePrivate">true if XML has private key and to use it, false otherwise</param>
        /// <returns></returns>
        public string Encrypt(string text, bool usePrivate = true)
        {
            // Uses internal byte array encryption and return its result as Base64 string
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(text), usePrivate));
        }
        /// <summary>
        /// Decrypt a text with given key in XML format.
        /// Can be used to decrypt using both private or public key, depending on data into XML.
        /// </summary>
        /// <param name="text">Encrypted text in Base64 format</param>
        /// <param name="keySize">Size of key to use</param>
        /// <param name="XmlKey">Text with key to use in XML format (private or public)</param>
        /// <param name="usePrivate">true if XML has private key and to use it, false otherwise</param>
        /// <returns></returns>
        public string Decrypt(string text, bool usePrivate = false)
        {
            // Convert Base64 text to byte array and decrypt it, returning its value in UTF8 encoding
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(text), usePrivate));
        }
        /// <summary>
        /// Encrypt a byte array input data using RSA and the given key in XML format.
        /// It encrypts using private or public key based on text given and parameters.
        /// </summary>
        /// <param name="data">Byte array of data to be encrypted</param>
        /// <param name="keySize">Size of key to use</param>
        /// <param name="XmlKey">String with key info in XML format</param>
        /// <param name="usePrivate">True if XML has all info, false if XML has only public key</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data, bool usePrivate = true)
        {
            // Check all parameters and warn if something goes wrong
            if (data.IsNullOrEmpty())         throw new ArgumentException("Data is empty");
            // Check if data is not too big to encrypt
            if (data.Length > maxDataLength) throw new ArgumentException(String.Format("Maximum data length is {0}", maxDataLength), "data");

            if (usePrivate && !HasPrivateInfo)
            {
                throw new CryptographicException("RSA Process: Incomplete Private Key Info");
            }
            if (!usePrivate && !HasPublicInfo)
            {
                throw new CryptographicException("RSA Process: Incomplete Public Key Info");
            }

            var _E = (usePrivate) ? D : E;
            var PT = BigInt.GetBigInt(data);
            var M  = BigInteger.ModPow(PT, _E, N);

            if (M.Sign == -1)
            {
                M += N;
            }

            // Encrypt data using custom RSACert class
            return BigInt.GetBytes(M, ModulusSize);
        }
        /// <summary>
        /// Decrypt a byte array input data using RSA and the given key in XML format.
        /// It decrypts using private or public key based on text given and parameters.
        /// </summary>
        /// <param name="data">Byte array of data to be decrypted</param>
        /// <param name="keySize">Size of key to use</param>
        /// <param name="XmlKey">String with key info in XML format</param>
        /// <param name="usePrivate">True if XML has all info, false if XML has only public key</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] data, bool usePrivate = false)
        {
            // Check all parameters and warn if something goes wrong
            if (data.IsNullOrEmpty()) throw new ArgumentException("Data is empty");
            if (usePrivate && !HasPrivateInfo)
            {
                throw new CryptographicException("RSA Process: Incomplete Private Key Info");
            }
            if (!usePrivate && !HasPublicInfo)
            {
                throw new CryptographicException("RSA Process: Incomplete Public Key Info");
            }

            // Decrypt data using custom RSACert class
            var _E = (usePrivate) ? D : E;
            var PT = BigInt.GetBigInt(data);
            var M  = BigInteger.ModPow(PT, _E, N);

            if (M.Sign == -1)
            {
                M += N;
            }

            return BigInt.GetBytes(M, ModulusSize);
        }

        /// <summary>
        /// Wrapper to encrypt text using private key.
        /// </summary>
        /// <param name="text">Text to encrypt</param>
        /// <returns></returns>
        public string PrivateEncrypt(string text)
        {
            // Convert text to byte array, encrypt it with internal method and return result as Base64 string
            return Convert.ToBase64String(PrivateEncrypt(Encoding.UTF8.GetBytes(text)));
        }
        /// <summary>
        /// Wrapper to decrypt text using private key.
        /// </summary>
        /// <param name="text">Text to decrypt (in Base64 format)</param>
        /// <returns></returns>
        public string PrivateDecrypt(string text)
        {
            // Convert Base64 string to byte array, decrypt it using internal method and return result as UTF encoded text
            return Encoding.UTF8.GetString(PrivateDecrypt(Convert.FromBase64String(text)));
        }
        /// <summary>
        /// Encrypt byte array data using given private key and size.
        /// </summary>
        /// <param name="data">Byte array to encrypt</param>
        /// <returns></returns>
        public byte[] PrivateEncrypt(byte[] data)
        {
            // Check all params and warn if something is wrong
            if (data.IsNullOrEmpty()) throw new ArgumentException("Data is empty");
            // Check data length to be sure it's not too big
            if (data.Length > maxDataLength) throw new ArgumentException(String.Format("Maximum data length is {0}", maxDataLength), "data");

            if (!HasPrivateInfo)
            {
                throw new CryptographicException("RSA Process: Incomplete Private Key Info");
            }

            // Encrypt data using custom RSACert class
            var C  = BigInt.GetBigInt(data);
            var M1 = BigInteger.ModPow(C, DP, P);
            var M2 = BigInteger.ModPow(C, DQ, Q);
            var H  = ((M1 - M2) * IQ) % P;
            var M  = (M2 + (Q * H));

            if (M.Sign == -1)
                M = M + N;

            return BigInt.GetBytes(M, ModulusSize);
        }
        /// <summary>
        /// Decrypt byte array data using given private key and size
        /// </summary>
        /// <param name="data">Byte array with encrypted data to decrypt</param>
        /// <returns></returns>
        public byte[] PrivateDecrypt(byte[] data)
        {
            // Check all params and warn if something is wrong
            if (data.IsNullOrEmpty()) throw new ArgumentException("Data is empty");

            if (!HasPrivateInfo)
            {
                throw new CryptographicException("RSA Process: Incomplete Private Key Info");
            }

            // Decrypt data using custom RSACert class
            var PT = BigInt.GetBigInt(data);
            var M  = BigInteger.ModPow(PT, D, N);

            if (M.Sign == -1)
            {
                M += N;
            }

            return BigInt.GetBytes(M, ModulusSize);
        }

        /// <summary>
        /// Wrapper to encrypt text using public key.
        /// </summary>
        /// <param name="text">Text to encrypt</param>
        /// <returns></returns>
        public string PublicEncrypt(string text)
        {
            // Conver texto to byte array, encrypt it with internal method and return result as Base64 string
            return Convert.ToBase64String(PublicEncrypt(Encoding.UTF8.GetBytes(text)));
        }
        /// <summary>
        /// Wrapper to decrypt text using public key.
        /// </summary>
        /// <param name="text">Text to decrypt (in Base64 format)</param>
        /// <returns></returns>
        public string PublicDecrypt(string text)
        {
            // Convert string to byte array, decrypt it using internal method and return result as UTF encoded text
            return Encoding.UTF8.GetString(PublicDecrypt(Convert.FromBase64String(text)));
        }
        /// <summary>
        /// Encrypt byte array data using given public key and size
        /// </summary>
        /// <param name="data">Byte array to encrypt</param>
        /// <returns></returns>
        public byte[] PublicEncrypt(byte[] data)
        {
            // Check all params and warn if something is wrong
            if (data.IsNullOrEmpty()) throw new ArgumentException("Data is empty");
            // Check data length to be sure it's not too big
            if (data.Length > maxDataLength) throw new ArgumentException(String.Format("Maximum data length is {0}", maxDataLength), "data");

            if (!HasPublicInfo)
            {
                throw new CryptographicException("RSA Process: Incomplete Public Key Info");
            }

            // Encrypt data using custom RSACert class
            BigInteger PT = BigInt.GetBigInt(data);
            BigInteger M  = BigInteger.ModPow(PT, E, N);

            if (M.Sign == -1)
            {
                M += N;
            }

            return BigInt.GetBytes(M, ModulusSize);
        }
        /// <summary>
        /// Decrypt byte array data using given public key and size
        /// </summary>
        /// <param name="data">Byte array with encrypted data to decrypt</param>
        /// <returns></returns>
        public byte[] PublicDecrypt(byte[] data)
        {
            // Check all params and warn if something is wrong
            if (data.IsNullOrEmpty()) throw new ArgumentException("Data is empty");

            if (!HasPublicInfo)
            {
                throw new CryptographicException("RSA Process: Incomplete Public Key Info");
            }

            // Decrypt data using custom RSACert class
            BigInteger PT = BigInt.GetBigInt(data);
            BigInteger M  = BigInteger.ModPow(PT, E, N);

            if (M.Sign == -1)
            {
                M += N;
            }

            return BigInt.GetBytes(M, ModulusSize);
        }


        #region Destructor and Disposer
        /// <summary>
        /// Clear and dispose of the internal state. The finalizer is only called 
        /// if Dispose() was never called on this cipher. 
        /// </summary>
        ~CryptRSA()
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
            }
            isDisposed = true;
        }
        #endregion
        #endregion
    }
}