namespace CryptTest
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lblText = new System.Windows.Forms.Label();
            this.lblCrypt = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();
            this.txtCrypt = new System.Windows.Forms.TextBox();
            this.btReset = new System.Windows.Forms.Button();
            this.btGo = new System.Windows.Forms.Button();
            this.txtDecrypt = new System.Windows.Forms.TextBox();
            this.lblDecrypt = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.lblKey = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.txtIV = new System.Windows.Forms.TextBox();
            this.lblIV = new System.Windows.Forms.Label();
            this.btRSA = new System.Windows.Forms.Button();
            this.lblKeySize = new System.Windows.Forms.Label();
            this.cbKeySize = new System.Windows.Forms.ComboBox();
            this.cbPrivateKey = new System.Windows.Forms.CheckBox();
            this.tbSide = new System.Windows.Forms.TrackBar();
            this.lblEncode = new System.Windows.Forms.Label();
            this.lblDecode = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbSide)).BeginInit();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblText.BackColor = System.Drawing.Color.Khaki;
            this.lblText.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.ForeColor = System.Drawing.Color.Gray;
            this.lblText.Location = new System.Drawing.Point(17, 15);
            this.lblText.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(456, 13);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Original Message";
            // 
            // lblCrypt
            // 
            this.lblCrypt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCrypt.BackColor = System.Drawing.Color.Khaki;
            this.lblCrypt.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCrypt.ForeColor = System.Drawing.Color.Gray;
            this.lblCrypt.Location = new System.Drawing.Point(17, 248);
            this.lblCrypt.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblCrypt.Name = "lblCrypt";
            this.lblCrypt.Size = new System.Drawing.Size(456, 13);
            this.lblCrypt.TabIndex = 1;
            this.lblCrypt.Text = "Encrypted Message";
            // 
            // txtText
            // 
            this.txtText.AcceptsReturn = true;
            this.txtText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtText.BackColor = System.Drawing.Color.Ivory;
            this.txtText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtText.Location = new System.Drawing.Point(17, 29);
            this.txtText.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(456, 56);
            this.txtText.TabIndex = 2;
            this.txtText.Text = "A very secret message here.";
            // 
            // txtCrypt
            // 
            this.txtCrypt.AcceptsReturn = true;
            this.txtCrypt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCrypt.BackColor = System.Drawing.Color.Azure;
            this.txtCrypt.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCrypt.Location = new System.Drawing.Point(17, 262);
            this.txtCrypt.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtCrypt.Multiline = true;
            this.txtCrypt.Name = "txtCrypt";
            this.txtCrypt.ReadOnly = true;
            this.txtCrypt.Size = new System.Drawing.Size(456, 54);
            this.txtCrypt.TabIndex = 3;
            // 
            // btReset
            // 
            this.btReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btReset.BackColor = System.Drawing.Color.Moccasin;
            this.btReset.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btReset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCoral;
            this.btReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btReset.Location = new System.Drawing.Point(17, 391);
            this.btReset.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(133, 22);
            this.btReset.TabIndex = 4;
            this.btReset.Text = "Reset";
            this.btReset.UseVisualStyleBackColor = false;
            this.btReset.Click += new System.EventHandler(this.btResetClick);
            // 
            // btGo
            // 
            this.btGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btGo.BackColor = System.Drawing.Color.YellowGreen;
            this.btGo.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btGo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btGo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btGo.Location = new System.Drawing.Point(17, 415);
            this.btGo.Margin = new System.Windows.Forms.Padding(0);
            this.btGo.Name = "btGo";
            this.btGo.Size = new System.Drawing.Size(133, 49);
            this.btGo.TabIndex = 5;
            this.btGo.Text = "Go";
            this.btGo.UseVisualStyleBackColor = false;
            this.btGo.Click += new System.EventHandler(this.btGoClick);
            // 
            // txtDecrypt
            // 
            this.txtDecrypt.AcceptsReturn = true;
            this.txtDecrypt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDecrypt.BackColor = System.Drawing.Color.Azure;
            this.txtDecrypt.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDecrypt.Location = new System.Drawing.Point(17, 331);
            this.txtDecrypt.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtDecrypt.Multiline = true;
            this.txtDecrypt.Name = "txtDecrypt";
            this.txtDecrypt.ReadOnly = true;
            this.txtDecrypt.Size = new System.Drawing.Size(456, 56);
            this.txtDecrypt.TabIndex = 6;
            // 
            // lblDecrypt
            // 
            this.lblDecrypt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDecrypt.BackColor = System.Drawing.Color.Khaki;
            this.lblDecrypt.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDecrypt.ForeColor = System.Drawing.Color.Gray;
            this.lblDecrypt.Location = new System.Drawing.Point(17, 317);
            this.lblDecrypt.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblDecrypt.Name = "lblDecrypt";
            this.lblDecrypt.Size = new System.Drawing.Size(456, 13);
            this.lblDecrypt.TabIndex = 7;
            this.lblDecrypt.Text = "Decrypted Message";
            // 
            // cbType
            // 
            this.cbType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "CryptStream3DES",
            "Crypt3DES",
            "CryptAES",
            "UUCode",
            "Mime/Base64",
            "Adler32",
            "CRC16",
            "CRC32",
            "CRC64",
            "Salsa20",
            "ChaCha20",
            "RSA",
            "Blowfish"});
            this.cbType.Location = new System.Drawing.Point(373, 392);
            this.cbType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(100, 21);
            this.cbType.TabIndex = 8;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbTypeSelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(314, 395);
            this.lblType.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(56, 13);
            this.lblType.TabIndex = 9;
            this.lblType.Text = "Algorythm:";
            // 
            // lblKey
            // 
            this.lblKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKey.BackColor = System.Drawing.Color.Khaki;
            this.lblKey.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKey.ForeColor = System.Drawing.Color.Gray;
            this.lblKey.Location = new System.Drawing.Point(17, 86);
            this.lblKey.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(456, 13);
            this.lblKey.TabIndex = 10;
            this.lblKey.Text = "Crypt Key";
            // 
            // txtKey
            // 
            this.txtKey.AcceptsReturn = true;
            this.txtKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKey.BackColor = System.Drawing.Color.MistyRose;
            this.txtKey.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKey.ForeColor = System.Drawing.Color.Black;
            this.txtKey.Location = new System.Drawing.Point(17, 100);
            this.txtKey.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtKey.Multiline = true;
            this.txtKey.Name = "txtKey";
            this.txtKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtKey.Size = new System.Drawing.Size(456, 66);
            this.txtKey.TabIndex = 11;
            this.txtKey.TextChanged += new System.EventHandler(this.txtChanged);
            // 
            // txtIV
            // 
            this.txtIV.AcceptsReturn = true;
            this.txtIV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIV.BackColor = System.Drawing.Color.MistyRose;
            this.txtIV.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIV.ForeColor = System.Drawing.Color.Black;
            this.txtIV.Location = new System.Drawing.Point(17, 181);
            this.txtIV.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.txtIV.Multiline = true;
            this.txtIV.Name = "txtIV";
            this.txtIV.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIV.Size = new System.Drawing.Size(456, 66);
            this.txtIV.TabIndex = 13;
            this.txtIV.TextChanged += new System.EventHandler(this.txtChanged);
            // 
            // lblIV
            // 
            this.lblIV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIV.BackColor = System.Drawing.Color.Khaki;
            this.lblIV.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIV.ForeColor = System.Drawing.Color.Gray;
            this.lblIV.Location = new System.Drawing.Point(17, 167);
            this.lblIV.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblIV.Name = "lblIV";
            this.lblIV.Size = new System.Drawing.Size(456, 13);
            this.lblIV.TabIndex = 12;
            this.lblIV.Text = "Initial Value";
            // 
            // btRSA
            // 
            this.btRSA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btRSA.BackColor = System.Drawing.Color.YellowGreen;
            this.btRSA.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btRSA.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btRSA.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btRSA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRSA.Location = new System.Drawing.Point(373, 441);
            this.btRSA.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btRSA.Name = "btRSA";
            this.btRSA.Size = new System.Drawing.Size(100, 25);
            this.btRSA.TabIndex = 14;
            this.btRSA.Text = "Generate";
            this.btRSA.UseVisualStyleBackColor = false;
            this.btRSA.Visible = false;
            this.btRSA.Click += new System.EventHandler(this.btRSAClick);
            // 
            // lblKeySize
            // 
            this.lblKeySize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKeySize.AutoSize = true;
            this.lblKeySize.Location = new System.Drawing.Point(314, 420);
            this.lblKeySize.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblKeySize.Name = "lblKeySize";
            this.lblKeySize.Size = new System.Drawing.Size(48, 13);
            this.lblKeySize.TabIndex = 16;
            this.lblKeySize.Text = "KeySize:";
            this.lblKeySize.Visible = false;
            // 
            // cbKeySize
            // 
            this.cbKeySize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbKeySize.FormattingEnabled = true;
            this.cbKeySize.Items.AddRange(new object[] {
            "384",
            "512",
            "768",
            "1024",
            "1280",
            "1536",
            "1792",
            "2048",
            "2304",
            "2560",
            "2616",
            "3072",
            "3328",
            "3584",
            "4096"});
            this.cbKeySize.Location = new System.Drawing.Point(373, 417);
            this.cbKeySize.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbKeySize.Name = "cbKeySize";
            this.cbKeySize.Size = new System.Drawing.Size(100, 21);
            this.cbKeySize.TabIndex = 15;
            this.cbKeySize.Visible = false;
            // 
            // cbPrivateKey
            // 
            this.cbPrivateKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPrivateKey.AutoSize = true;
            this.cbPrivateKey.Location = new System.Drawing.Point(317, 446);
            this.cbPrivateKey.Name = "cbPrivateKey";
            this.cbPrivateKey.Size = new System.Drawing.Size(47, 17);
            this.cbPrivateKey.TabIndex = 17;
            this.cbPrivateKey.Text = "Priv.";
            this.cbPrivateKey.UseVisualStyleBackColor = true;
            // 
            // tbSide
            // 
            this.tbSide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSide.AutoSize = false;
            this.tbSide.CausesValidation = false;
            this.tbSide.LargeChange = 1;
            this.tbSide.Location = new System.Drawing.Point(165, 406);
            this.tbSide.Margin = new System.Windows.Forms.Padding(0);
            this.tbSide.Maximum = 1;
            this.tbSide.Name = "tbSide";
            this.tbSide.Size = new System.Drawing.Size(136, 46);
            this.tbSide.TabIndex = 18;
            this.tbSide.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbSide.ValueChanged += new System.EventHandler(this.tbSideValueChanged);
            // 
            // lblEncode
            // 
            this.lblEncode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblEncode.AutoSize = true;
            this.lblEncode.Location = new System.Drawing.Point(162, 447);
            this.lblEncode.Name = "lblEncode";
            this.lblEncode.Size = new System.Drawing.Size(44, 13);
            this.lblEncode.TabIndex = 19;
            this.lblEncode.Text = "Encode";
            // 
            // lblDecode
            // 
            this.lblDecode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDecode.AutoSize = true;
            this.lblDecode.Location = new System.Drawing.Point(256, 447);
            this.lblDecode.Name = "lblDecode";
            this.lblDecode.Size = new System.Drawing.Size(45, 13);
            this.lblDecode.TabIndex = 20;
            this.lblDecode.Text = "Decode";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(484, 477);
            this.Controls.Add(this.lblDecode);
            this.Controls.Add(this.lblEncode);
            this.Controls.Add(this.tbSide);
            this.Controls.Add(this.cbPrivateKey);
            this.Controls.Add(this.lblKeySize);
            this.Controls.Add(this.cbKeySize);
            this.Controls.Add(this.btRSA);
            this.Controls.Add(this.txtIV);
            this.Controls.Add(this.lblIV);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.lblKey);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.txtDecrypt);
            this.Controls.Add(this.btGo);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.txtCrypt);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.lblCrypt);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.lblDecrypt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.MinimumSize = new System.Drawing.Size(495, 501);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CryptTest v1.0";
            ((System.ComponentModel.ISupportInitialize)(this.tbSide)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Label lblCrypt;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.TextBox txtCrypt;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.Button btGo;
        private System.Windows.Forms.TextBox txtDecrypt;
        private System.Windows.Forms.Label lblDecrypt;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtIV;
        private System.Windows.Forms.Label lblIV;
        private System.Windows.Forms.Button btRSA;
        private System.Windows.Forms.Label lblKeySize;
        private System.Windows.Forms.ComboBox cbKeySize;
        private System.Windows.Forms.CheckBox cbPrivateKey;
        private System.Windows.Forms.TrackBar tbSide;
        private System.Windows.Forms.Label lblEncode;
        private System.Windows.Forms.Label lblDecode;
    }
}

