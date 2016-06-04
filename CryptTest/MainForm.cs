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
using System.Drawing;
using System.Windows.Forms;

using CryptTest.Framework.Crypt;

namespace CryptTest
{
    /// <summary>
    /// Main form for our app
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Key sizes for RSA crypt.
        /// Will be shown into a combobox when using RSA.
        /// </summary>
        private static int[] keySizes = { 384, 512, 768, 1024, 1280, 1536, 1792, 2048, 2304, 2560, 2616, 3072, 3328, 3584, 4096 };
        /// <summary>
        /// Main form
        /// C# standard: initialize UI components and do custom stuff
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            // Just assign default values to controls and texts
            btResetClick(null, null);
        }
        /// <summary>
        /// Set default UI status and text values when clicking reset
        /// </summary>
        /// <param name="sender">Sender of Click event</param>
        /// <param name="e">Args for Click event</param>
        private void btResetClick(object sender, EventArgs e)
        {
            // Clear result texts and set fancy input text
            txtText.Text    = "Your misterious and secret message here.";
            txtCrypt.Text   = "";
            txtDecrypt.Text = "";
            // Set default key/initializer values
            txtKey.Text = "some weird value";
            txtIV.Text  = "a simple value";
            // Set combobox indexes
            cbType.SelectedIndex    = 0;
            cbKeySize.SelectedIndex = 3;
            // Setup do Encrypt -> Decrypt
            tbSide.Value = 0;
            // For RSA use public key and not private key
            cbPrivateKey.Checked = false;
        }
        /// <summary>
        /// Actions to do when user clicks the 'Go' button
        /// </summary>
        /// <param name="sender">Sender of Click event</param>
        /// <param name="e">Arguments for Click event</param>
        private void btGoClick(object sender, EventArgs e)
        {
            // No text to crypt/decrypt? finish
            if (txtText.Text == "") return;
            // According to selected crypt algorithm...
            switch (cbType.SelectedIndex)
            {
                case 0: // CryptStream3DES
                    // No key and initializer values? finish
                    if ((txtKey.Text == "") || (txtIV.Text == "")) return;
                    // If we are encrypting, use according texts for encrypt and decrypt
                    if (tbSide.Value == 0)
                    {
                        txtCrypt.Text   = CryptStream3DES.Encrypt(txtText.Text, txtKey.Text, txtIV.Text);
                        txtDecrypt.Text = CryptStream3DES.Decrypt(txtCrypt.Text, txtKey.Text, txtIV.Text);
                    }
                    else
                    {
                        // If we are decrypting, take the inverse text values
                        txtCrypt.Text   = CryptStream3DES.Decrypt(txtText.Text, txtKey.Text, txtIV.Text);
                        txtDecrypt.Text = CryptStream3DES.Encrypt(txtCrypt.Text, txtKey.Text, txtIV.Text);
                    }
                    break;
                case 1: // Crypt3DES
                    // No key? finish
                    if (txtKey.Text == "") return;
                    // If we are encrypting, use according text for encrypt and decrypt
                    if (tbSide.Value == 0)
                    {
                        txtCrypt.Text   = Crypt3DES.Encrypt(txtText.Text, txtKey.Text);
                        txtDecrypt.Text = Crypt3DES.Decrypt(txtCrypt.Text, txtKey.Text);
                    }
                    else
                    {
                        // If we are decrypting, use the inverse text values
                        txtCrypt.Text   = Crypt3DES.Decrypt(txtText.Text, txtKey.Text);
                        txtDecrypt.Text = Crypt3DES.Encrypt(txtCrypt.Text, txtKey.Text);
                    }
                    break;
                case 2: // CryptAES
                    // No key and initializer? finish
                    if ((txtKey.Text == "") || (txtIV.Text == "")) return;
                    // Use right texts to encrypt/decrypt, based on our setup (also use provided key and salt)
                    if (tbSide.Value == 0)
                    {
                        txtCrypt.Text   = CryptAES.Encrypt(txtText.Text, txtKey.Text, txtIV.Text);
                        txtDecrypt.Text = CryptAES.Decrypt(txtCrypt.Text, txtKey.Text, txtIV.Text);
                    }
                    else
                    {
                        txtCrypt.Text   = CryptAES.Decrypt(txtText.Text, txtKey.Text, txtIV.Text);
                        txtDecrypt.Text = CryptAES.Encrypt(txtCrypt.Text, txtKey.Text, txtIV.Text);
                    }
                    break;
                case 3: // UUCode
                    // Apply encoding/decoding to right texts according to setup
                    if (tbSide.Value == 0)
                    {
                        txtCrypt.Text   = UUCoder.Encode(txtText.Text);
                        txtDecrypt.Text = UUCoder.Decode(txtCrypt.Text);
                    }
                    else
                    {
                        txtCrypt.Text   = UUCoder.Decode(txtText.Text);
                        txtDecrypt.Text = UUCoder.Encode(txtCrypt.Text);
                    }
                    break;
                case 4: // Mime/Base64
                    // Apply encoding/decoding to right texts according to setup
                    if (tbSide.Value == 0)
                    {
                        txtCrypt.Text   = Base64.EncodeString(txtText.Text);
                        txtDecrypt.Text = Base64.DecodeString(txtCrypt.Text);
                    }
                    else
                    {
                        txtCrypt.Text   = Base64.DecodeString(txtText.Text);
                        txtDecrypt.Text = Base64.EncodeString(txtCrypt.Text);
                    }
                    break;
                case 5: // Adler32
                    // Calculate hash for given text...
                    txtCrypt.Text = string.Format("{0:X}", Adler32.AdlerHash(txtText.Text));
                    break;
                case 6: // CRC16
                    // Calculate hash for given text...
                    txtCrypt.Text = string.Format("{0:X}", CRC16.Hash(txtText.Text));
                    break;
                case 7: // CRC32
                    // Calculate hash for given text...
                    txtCrypt.Text = string.Format("{0:X}", CRC32.Hash(txtText.Text));
                    break;
                case 8: // CRC64
                    // Calculate hash for given text...
                    txtCrypt.Text = string.Format("{0:X}", CRC64.Hash(txtText.Text));
                    break;
                case 9: // Salsa20
                    // No key and initializer? finish
                    if ((txtKey.Text == "") || (txtIV.Text == "")) return;
                    // Use cryptor to encrypt/decrypt data, according to setup
                    using (var salsa = new CryptSalsa(txtKey.Text, txtIV.Text))
                    {
                        // Encrypt -> Decrypt
                        if (tbSide.Value == 0)
                        {
                            txtCrypt.Text   = salsa.Encrypt(txtText.Text);
                            txtDecrypt.Text = salsa.Decrypt(txtCrypt.Text);
                        }
                        else
                        {
                            // Or Decrypt -> Encrypt
                            txtCrypt.Text   = salsa.Decrypt(txtText.Text);
                            txtDecrypt.Text = salsa.Encrypt(txtCrypt.Text);
                        }
                    }
                    break;
                case 10: // ChaCha20
                    // No key/initializer? finish
                    if ((txtKey.Text == "") || (txtIV.Text == "")) return;
                    // Use cryptor to encrypt/decrypt data, according to setup
                    using (var chacha = new CryptChaCha20(txtKey.Text, txtIV.Text))
                    {
                        // Encrypt -> Decrypt
                        if (tbSide.Value == 0)
                        {
                            txtCrypt.Text   = chacha.Encrypt(txtText.Text);
                            txtDecrypt.Text = chacha.Decrypt(txtCrypt.Text);
                        }
                        else
                        {
                            // Or Decrypt -> Encrypt
                            txtCrypt.Text   = chacha.Decrypt(txtText.Text);
                            txtDecrypt.Text = chacha.Encrypt(txtCrypt.Text);
                        }
                    }
                    break;
                case 11: // RSA
                    // if no key/initializer value, finish
                    if ((txtKey.Text == "") || (txtIV.Text == "")) return;
                    using (var rsa = new CryptRSA(txtKey.Text, keySizes[cbKeySize.SelectedIndex]))
                    {
                        // Use private key?
                        if (cbPrivateKey.Checked)
                        {
                            // Use cryptor to encrypt/decrypt data, according to setup
                            if (tbSide.Value == 0)
                            {
                                // Encrypt -> Decrypt
                                txtCrypt.Text   = rsa.PrivateEncrypt(txtText.Text);
                                txtDecrypt.Text = rsa.PublicDecrypt(txtCrypt.Text);
                            }
                            else
                            {
                                // Or Decrypt -> Encrypt
                                txtCrypt.Text   = rsa.PublicDecrypt(txtText.Text);
                                txtDecrypt.Text = rsa.PrivateEncrypt(txtCrypt.Text);
                            }
                        }
                        else
                        {
                            // Nope, use public key...
                            // Use cryptor to encrypt/decrypt data, according to setup
                            if (tbSide.Value == 0)
                            {
                                // Encrypt -> Decrypt
                                txtCrypt.Text   = rsa.PublicEncrypt(txtText.Text);
                                txtDecrypt.Text = rsa.PrivateDecrypt(txtCrypt.Text);
                            }
                            else
                            {
                                // Or Decrypt -> Encrypt
                                txtCrypt.Text   = rsa.PrivateDecrypt(txtText.Text);
                                txtDecrypt.Text = rsa.PublicEncrypt(txtCrypt.Text);
                            }
                        }
                    }
                    break;
                case 12: // Blowfish
                    // No key? finish
                    if (txtKey.Text == "") return;
                    // Use cryptor to encrypt/decrypt data, according to setup
                    using (var blowfish = new CryptBlowfish(txtKey.Text))
                    {
                        // Encrypt -> Decrypt
                        if (tbSide.Value == 0)
                        {
                            txtCrypt.Text   = blowfish.Encrypt(txtText.Text);
                            txtDecrypt.Text = blowfish.Decrypt(txtCrypt.Text);
                        }
                        else
                        {
                            // Or Decrypt -> Encrypt
                            txtCrypt.Text   = blowfish.Decrypt(txtText.Text);
                            txtDecrypt.Text = blowfish.Encrypt(txtCrypt.Text);
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Setup UI according to selected crypt algorithm
        /// </summary>
        /// <param name="sender">Sender of SelectedIndexChanged event</param>
        /// <param name="e">Args for SelectedIndexChanged event</param>
        private void cbTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            // Calculate visibility for text input fields and labels
            var showKey    = ((cbType.SelectedIndex < 3) || (cbType.SelectedIndex > 8));
            var showIV     = ((cbType.SelectedIndex == 0) || (cbType.SelectedIndex == 2) || ((cbType.SelectedIndex > 8) && (cbType.SelectedIndex != 12)));
            var showDecode = ((cbType.SelectedIndex < 5) || (cbType.SelectedIndex > 8));
            var showRSA    = (cbType.SelectedIndex == 11);
            // Assign visibility to labels and fields for Key and Initializer
            lblKey.Visible = showKey;
            txtKey.Visible = showKey;
            lblIV.Visible  = showIV;
            txtIV.Visible  = showIV;
            // Assign visibility to labels and fields for encode/decode data
            tbSide.Visible     = showDecode;
            lblEncode.Visible  = showDecode;
            lblDecode.Visible  = showDecode;
            lblDecrypt.Visible = showDecode;
            txtDecrypt.Visible = showDecode;
            // Clear output
            if (!showDecode) txtDecrypt.Text = "";
            // Assign visibility for RSA custom controls
            btRSA.Visible        = showRSA;
            lblKeySize.Visible   = showRSA;
            cbKeySize.Visible    = showRSA;
            cbPrivateKey.Visible = showRSA;
            // Assign texts according to crypt algorithm
            var strKey = "";
            var strIV  = "";
            var tbxKey = "";
            var tbxIV  = "";
            var strCrypt = "Encrypted Message";
            // See which one is the choosen one
            switch (cbType.SelectedIndex)
            {
                case 0: // CryptStream3DES
                    strKey = "Crypt Key";
                    strIV  = "Initial Value";
                    tbxKey = "some weird value";
                    tbxIV  = "a simple value";
                    break;
                case 1: // Crypt3DES
                    strKey = "Crypt Key";
                    tbxKey = "some weird value";
                    break;
                case 2: // CryptAES
                    strKey = "Crypt Key";
                    strIV  = "Salt Value";
                    tbxKey = "some weird value";
                    tbxIV  = "a simple value";
                    break;
                case 5: // Adler32
                case 6: // CRC16
                case 7: // CRC32
                case 8: // CRC64
                    strCrypt = "Hash";
                    break;
                case 9: // Salsa20
                case 10: // ChaCha20
                    strKey = "Crypt Key";
                    strIV  = "Nonce";
                    tbxKey = "Valid 32 byte key for encrypting";
                    tbxIV = (cbType.SelectedIndex == 9) ? "FunNonce" : "Funny Nonce!";
                    break;
                case 11: // RSA
                    strKey = "Private Key";
                    strIV  = "Public Key";
                    cbPrivateKey.Checked = false;
                    break;
                case 12: // Blowfish
                    strKey = "Blowfish secret key";
                    tbxKey = "Very secret key!";
                    break;
            }
            // Assign calculated texts to labels and input fields
            lblKey.Text     = strKey;
            lblIV.Text      = strIV;
            txtKey.Text     = tbxKey;
            txtIV.Text      = tbxIV;
            lblCrypt.Text   = strCrypt;
            txtCrypt.Text   = "";
            txtDecrypt.Text = "";
        }
        /// <summary>
        /// Generate keys for RSA algorithm: private and public keys
        /// </summary>
        /// <param name="sender">Sender of Click event</param>
        /// <param name="e">Args for Click event</param>
        private void btRSAClick(object sender, EventArgs e)
        {
            // Init vars and take key size
            var keyPublic  = "";
            var keyPrivate = "";
            var keySize = keySizes[cbKeySize.SelectedIndex];
            // Generate keys based on desired size
            CryptRSA.GenerateKeys(keySize, out keyPublic, out keyPrivate);
            // Show them in text fields (adjust from XML to RAW text)
            txtKey.Text = keyPrivate.Replace("><", ">\r\n<");
            txtIV.Text  = keyPublic.Replace("><", ">\r\n<");
        }
        /// <summary>
        /// Show color according to text field is empty or not
        /// Used on fields with needed input text (not empty)
        /// </summary>
        /// <param name="sender">Sender of TextChanged event</param>
        /// <param name="e">Args for TextChanged event</param>
        private void txtChanged(object sender, EventArgs e)
        {
            // Take source of event
            var source = (TextBox)sender;
            // And change background color depending on it's empty or not
            source.BackColor = (source.Text == "") ? Color.MistyRose : Color.Honeydew;
        }
        /// <summary>
        /// Select Encrypt/Decrypt direction
        /// Choose if encrypt input text (and decrypt it too) or the contrary
        /// </summary>
        /// <param name="sender">Sender of SideValueChanged event</param>
        /// <param name="e">Args for SideValueChanged event</param>
        private void tbSideValueChanged(object sender, EventArgs e)
        {
            // Both output texts will be cleared
            txtCrypt.Text   = "";
            txtDecrypt.Text = "";
            // Depending on direction, choose labels for text fields
            if (tbSide.Value == 0)
            {
                // Encrypt -> Decrypt
                lblCrypt.Text   = "Encrypted Message";
                lblDecrypt.Text = "Decrypted Message";
                lblText.Text    = "Original Message";
            }
            else
            {
                // Decrypt -> Encrypt
                lblCrypt.Text   = "Decrypted Message";
                lblDecrypt.Text = "Encrypted Message";
                lblText.Text    = "Encrypted Message";
            }
        }
    }
}
