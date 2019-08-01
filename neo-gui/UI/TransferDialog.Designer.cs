namespace Neo.UI
{
    partial class TransferDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferDialog));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxFrom = new System.Windows.Forms.ComboBox();
            this.labelFrom = new System.Windows.Forms.Label();
            this.comboBoxChangeAddress = new System.Windows.Forms.ComboBox();
            this.labelChangeAddress = new System.Windows.Forms.Label();
            this.textBoxFee = new System.Windows.Forms.TextBox();
            this.labelFee = new System.Windows.Forms.Label();
            this.txOutListBox1 = new Neo.UI.TxOutListBox();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.txOutListBox1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Image = global::Neo.Properties.Resources.remark;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.comboBoxFrom);
            this.groupBox1.Controls.Add(this.labelFrom);
            this.groupBox1.Controls.Add(this.comboBoxChangeAddress);
            this.groupBox1.Controls.Add(this.labelChangeAddress);
            this.groupBox1.Controls.Add(this.textBoxFee);
            this.groupBox1.Controls.Add(this.labelFee);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // comboBoxFrom
            // 
            resources.ApplyResources(this.comboBoxFrom, "comboBoxFrom");
            this.comboBoxFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFrom.FormattingEnabled = true;
            this.comboBoxFrom.Name = "comboBoxFrom";
            // 
            // labelFrom
            // 
            resources.ApplyResources(this.labelFrom, "labelFrom");
            this.labelFrom.Name = "labelFrom";
            // 
            // comboBoxChangeAddress
            // 
            resources.ApplyResources(this.comboBoxChangeAddress, "comboBoxChangeAddress");
            this.comboBoxChangeAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChangeAddress.FormattingEnabled = true;
            this.comboBoxChangeAddress.Name = "comboBoxChangeAddress";
            // 
            // labelChangeAddress
            // 
            resources.ApplyResources(this.labelChangeAddress, "labelChangeAddress");
            this.labelChangeAddress.Name = "labelChangeAddress";
            // 
            // textBoxFee
            // 
            resources.ApplyResources(this.textBoxFee, "textBoxFee");
            this.textBoxFee.Name = "textBoxFee";
            // 
            // labelFee
            // 
            resources.ApplyResources(this.labelFee, "labelFee");
            this.labelFee.Name = "labelFee";
            // 
            // txOutListBox1
            // 
            resources.ApplyResources(this.txOutListBox1, "txOutListBox1");
            this.txOutListBox1.Asset = null;
            this.txOutListBox1.Name = "txOutListBox1";
            this.txOutListBox1.ReadOnly = false;
            this.txOutListBox1.ScriptHash = null;
            this.txOutListBox1.ItemsChanged += new System.EventHandler(this.txOutListBox1_ItemsChanged);
            // 
            // TransferDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "TransferDialog";
            this.ShowInTaskbar = false;
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private TxOutListBox txOutListBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxFee;
        private System.Windows.Forms.Label labelFee;
        private System.Windows.Forms.Label labelChangeAddress;
        private System.Windows.Forms.ComboBox comboBoxChangeAddress;
        private System.Windows.Forms.ComboBox comboBoxFrom;
        private System.Windows.Forms.Label labelFrom;
    }
}