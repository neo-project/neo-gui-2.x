namespace Neo.UI
{
    partial class QueryDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueryDialog));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.contractScriptHashTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Query = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtbx_swap_rate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtbx_name = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbx_precision = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbx_symbol = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbx_totalSupply = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtbx_balance = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtbx_address = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.CheckBalance = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtbx_round_3 = new System.Windows.Forms.TextBox();
            this.txtbx_round_2 = new System.Windows.Forms.TextBox();
            this.txtbx_round_1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.refundsGridView = new System.Windows.Forms.DataGridView();
            this.RefreshRefundsButton = new System.Windows.Forms.Button();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.refundsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.contractScriptHashTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Query);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // contractScriptHashTextBox
            // 
            resources.ApplyResources(this.contractScriptHashTextBox, "contractScriptHashTextBox");
            this.contractScriptHashTextBox.Name = "contractScriptHashTextBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Query
            // 
            this.Query.BackgroundImage = global::Neo.Properties.Resources.refresh;
            resources.ApplyResources(this.Query, "Query");
            this.Query.Name = "Query";
            this.Query.UseVisualStyleBackColor = true;
            this.Query.Click += new System.EventHandler(this.Query_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtbx_swap_rate);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtbx_name);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtbx_precision);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtbx_symbol);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtbx_totalSupply);
            this.groupBox2.Controls.Add(this.label5);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // txtbx_swap_rate
            // 
            this.txtbx_swap_rate.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtbx_swap_rate, "txtbx_swap_rate");
            this.txtbx_swap_rate.Name = "txtbx_swap_rate";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // txtbx_name
            // 
            resources.ApplyResources(this.txtbx_name, "txtbx_name");
            this.txtbx_name.Name = "txtbx_name";
            this.txtbx_name.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtbx_precision
            // 
            resources.ApplyResources(this.txtbx_precision, "txtbx_precision");
            this.txtbx_precision.Name = "txtbx_precision";
            this.txtbx_precision.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtbx_symbol
            // 
            resources.ApplyResources(this.txtbx_symbol, "txtbx_symbol");
            this.txtbx_symbol.Name = "txtbx_symbol";
            this.txtbx_symbol.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtbx_totalSupply
            // 
            resources.ApplyResources(this.txtbx_totalSupply, "txtbx_totalSupply");
            this.txtbx_totalSupply.Name = "txtbx_totalSupply";
            this.txtbx_totalSupply.ReadOnly = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtbx_balance);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtbx_address);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.CheckBalance);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // txtbx_balance
            // 
            resources.ApplyResources(this.txtbx_balance, "txtbx_balance");
            this.txtbx_balance.Name = "txtbx_balance";
            this.txtbx_balance.ReadOnly = true;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // txtbx_address
            // 
            resources.ApplyResources(this.txtbx_address, "txtbx_address");
            this.txtbx_address.Name = "txtbx_address";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // CheckBalance
            // 
            resources.ApplyResources(this.CheckBalance, "CheckBalance");
            this.CheckBalance.Name = "CheckBalance";
            this.CheckBalance.UseVisualStyleBackColor = true;
            this.CheckBalance.Click += new System.EventHandler(this.CheckBalance_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtbx_round_3);
            this.groupBox4.Controls.Add(this.txtbx_round_2);
            this.groupBox4.Controls.Add(this.txtbx_round_1);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // txtbx_round_3
            // 
            this.txtbx_round_3.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtbx_round_3, "txtbx_round_3");
            this.txtbx_round_3.Name = "txtbx_round_3";
            // 
            // txtbx_round_2
            // 
            this.txtbx_round_2.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtbx_round_2, "txtbx_round_2");
            this.txtbx_round_2.Name = "txtbx_round_2";
            // 
            // txtbx_round_1
            // 
            this.txtbx_round_1.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtbx_round_1, "txtbx_round_1");
            this.txtbx_round_1.Name = "txtbx_round_1";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.RefreshRefundsButton);
            this.groupBox5.Controls.Add(this.refundsGridView);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // refundsGridView
            // 
            this.refundsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.refundsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Address,
            this.Value});
            resources.ApplyResources(this.refundsGridView, "refundsGridView");
            this.refundsGridView.Name = "refundsGridView";
            // 
            // RefreshRefundsButton
            // 
            this.RefreshRefundsButton.BackgroundImage = global::Neo.Properties.Resources.refresh;
            resources.ApplyResources(this.RefreshRefundsButton, "RefreshRefundsButton");
            this.RefreshRefundsButton.Name = "RefreshRefundsButton";
            this.RefreshRefundsButton.UseVisualStyleBackColor = true;
            this.RefreshRefundsButton.Click += new System.EventHandler(this.RefreshRefundsButton_Click);
            // 
            // Address
            // 
            resources.ApplyResources(this.Address, "Address");
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            // 
            // Value
            // 
            resources.ApplyResources(this.Value, "Value");
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            // 
            // QueryDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "QueryDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.refundsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtbx_totalSupply;
        private System.Windows.Forms.Button Query;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtbx_precision;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbx_symbol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtbx_name;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtbx_balance;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtbx_address;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button CheckBalance;
        private System.Windows.Forms.TextBox contractScriptHashTextBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtbx_round_3;
        private System.Windows.Forms.TextBox txtbx_round_2;
        private System.Windows.Forms.TextBox txtbx_round_1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtbx_swap_rate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView refundsGridView;
        private System.Windows.Forms.Button RefreshRefundsButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
    }
}