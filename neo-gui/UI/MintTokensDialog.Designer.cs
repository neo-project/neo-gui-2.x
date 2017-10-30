namespace Neo.UI
{
    partial class MintTokensDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.contractScriptHashLabel = new System.Windows.Forms.Label();
            this.assetLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.amountLabel = new System.Windows.Forms.Label();
            this.contractScriptHashTextBox = new System.Windows.Forms.TextBox();
            this.assetComboBox = new System.Windows.Forms.ComboBox();
            this.currentBalanceTextBox = new System.Windows.Forms.TextBox();
            this.amountTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // contractScriptHashLabel
            // 
            this.contractScriptHashLabel.AutoSize = true;
            this.contractScriptHashLabel.Location = new System.Drawing.Point(12, 34);
            this.contractScriptHashLabel.Name = "contractScriptHashLabel";
            this.contractScriptHashLabel.Size = new System.Drawing.Size(105, 13);
            this.contractScriptHashLabel.TabIndex = 0;
            this.contractScriptHashLabel.Text = "Contract Script Hash";
            // 
            // assetLabel
            // 
            this.assetLabel.AutoSize = true;
            this.assetLabel.Location = new System.Drawing.Point(12, 94);
            this.assetLabel.Name = "assetLabel";
            this.assetLabel.Size = new System.Drawing.Size(33, 13);
            this.assetLabel.TabIndex = 1;
            this.assetLabel.Text = "Asset";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(268, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current Balance";
            // 
            // amountLabel
            // 
            this.amountLabel.AutoSize = true;
            this.amountLabel.Location = new System.Drawing.Point(12, 165);
            this.amountLabel.Name = "amountLabel";
            this.amountLabel.Size = new System.Drawing.Size(43, 13);
            this.amountLabel.TabIndex = 3;
            this.amountLabel.Text = "Amount";
            // 
            // contractScriptHashTextBox
            // 
            this.contractScriptHashTextBox.Location = new System.Drawing.Point(15, 51);
            this.contractScriptHashTextBox.Name = "contractScriptHashTextBox";
            this.contractScriptHashTextBox.Size = new System.Drawing.Size(336, 20);
            this.contractScriptHashTextBox.TabIndex = 4;
            // 
            // assetComboBox
            // 
            this.assetComboBox.FormattingEnabled = true;
            this.assetComboBox.Location = new System.Drawing.Point(15, 111);
            this.assetComboBox.Name = "assetComboBox";
            this.assetComboBox.Size = new System.Drawing.Size(244, 21);
            this.assetComboBox.TabIndex = 5;
            this.assetComboBox.SelectedIndexChanged += new System.EventHandler(this.assetComboBox_SelectedIndexChanged);
            // 
            // currentBalanceTextBox
            // 
            this.currentBalanceTextBox.Enabled = false;
            this.currentBalanceTextBox.Location = new System.Drawing.Point(271, 112);
            this.currentBalanceTextBox.Name = "currentBalanceTextBox";
            this.currentBalanceTextBox.Size = new System.Drawing.Size(80, 20);
            this.currentBalanceTextBox.TabIndex = 6;
            // 
            // amountTextBox
            // 
            this.amountTextBox.Location = new System.Drawing.Point(15, 182);
            this.amountTextBox.Name = "amountTextBox";
            this.amountTextBox.Size = new System.Drawing.Size(336, 20);
            this.amountTextBox.TabIndex = 7;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(184, 251);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(271, 251);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // MintTokensDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(363, 286);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.amountTextBox);
            this.Controls.Add(this.currentBalanceTextBox);
            this.Controls.Add(this.assetComboBox);
            this.Controls.Add(this.contractScriptHashTextBox);
            this.Controls.Add(this.amountLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.assetLabel);
            this.Controls.Add(this.contractScriptHashLabel);
            this.Name = "MintTokensDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MintTokensDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label contractScriptHashLabel;
        private System.Windows.Forms.Label assetLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label amountLabel;
        private System.Windows.Forms.TextBox contractScriptHashTextBox;
        private System.Windows.Forms.ComboBox assetComboBox;
        private System.Windows.Forms.TextBox currentBalanceTextBox;
        private System.Windows.Forms.TextBox amountTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}