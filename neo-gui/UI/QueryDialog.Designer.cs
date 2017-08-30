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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtbx_totalSupply = new System.Windows.Forms.TextBox();
            this.txtbx_inflationRate = new System.Windows.Forms.TextBox();
            this.txtbx_icoNeo = new System.Windows.Forms.TextBox();
            this.txtbx_totalIcoNeo = new System.Windows.Forms.TextBox();
            this.txtbx_infStartTime = new System.Windows.Forms.TextBox();
            this.Query = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 88);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择合约";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(114, 33);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(375, 24);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "脚本哈希";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtbx_totalSupply);
            this.groupBox2.Controls.Add(this.txtbx_inflationRate);
            this.groupBox2.Controls.Add(this.txtbx_icoNeo);
            this.groupBox2.Controls.Add(this.txtbx_totalIcoNeo);
            this.groupBox2.Controls.Add(this.txtbx_infStartTime);
            this.groupBox2.Controls.Add(this.Query);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(12, 106);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(535, 218);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "查询";
            // 
            // txtbx_totalSupply
            // 
            this.txtbx_totalSupply.Location = new System.Drawing.Point(104, 25);
            this.txtbx_totalSupply.Name = "txtbx_totalSupply";
            this.txtbx_totalSupply.ReadOnly = true;
            this.txtbx_totalSupply.Size = new System.Drawing.Size(135, 22);
            this.txtbx_totalSupply.TabIndex = 24;
            // 
            // txtbx_inflationRate
            // 
            this.txtbx_inflationRate.Location = new System.Drawing.Point(104, 73);
            this.txtbx_inflationRate.Name = "txtbx_inflationRate";
            this.txtbx_inflationRate.ReadOnly = true;
            this.txtbx_inflationRate.Size = new System.Drawing.Size(135, 22);
            this.txtbx_inflationRate.TabIndex = 23;
            // 
            // txtbx_icoNeo
            // 
            this.txtbx_icoNeo.Location = new System.Drawing.Point(362, 73);
            this.txtbx_icoNeo.Name = "txtbx_icoNeo";
            this.txtbx_icoNeo.ReadOnly = true;
            this.txtbx_icoNeo.Size = new System.Drawing.Size(134, 22);
            this.txtbx_icoNeo.TabIndex = 22;
            // 
            // txtbx_totalIcoNeo
            // 
            this.txtbx_totalIcoNeo.Location = new System.Drawing.Point(362, 25);
            this.txtbx_totalIcoNeo.Name = "txtbx_totalIcoNeo";
            this.txtbx_totalIcoNeo.ReadOnly = true;
            this.txtbx_totalIcoNeo.Size = new System.Drawing.Size(134, 22);
            this.txtbx_totalIcoNeo.TabIndex = 21;
            // 
            // txtbx_infStartTime
            // 
            this.txtbx_infStartTime.Location = new System.Drawing.Point(104, 122);
            this.txtbx_infStartTime.Name = "txtbx_infStartTime";
            this.txtbx_infStartTime.ReadOnly = true;
            this.txtbx_infStartTime.Size = new System.Drawing.Size(135, 22);
            this.txtbx_infStartTime.TabIndex = 20;
            // 
            // Query
            // 
            this.Query.Location = new System.Drawing.Point(395, 171);
            this.Query.Name = "Query";
            this.Query.Size = new System.Drawing.Size(101, 28);
            this.Query.TabIndex = 18;
            this.Query.Text = "Query";
            this.Query.UseVisualStyleBackColor = true;
            this.Query.Click += new System.EventHandler(this.Query_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "infStartTime";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 17);
            this.label6.TabIndex = 15;
            this.label6.Text = "inflationRate";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 17);
            this.label5.TabIndex = 16;
            this.label5.Text = "totalSupply";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(295, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 17);
            this.label9.TabIndex = 12;
            this.label9.Text = "ICONEO";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(268, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "totalICONEO";
            // 
            // QueryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 337);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "QueryDialog";
            this.Text = "QueryDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtbx_totalSupply;
        private System.Windows.Forms.TextBox txtbx_inflationRate;
        private System.Windows.Forms.TextBox txtbx_icoNeo;
        private System.Windows.Forms.TextBox txtbx_totalIcoNeo;
        private System.Windows.Forms.TextBox txtbx_infStartTime;
        private System.Windows.Forms.Button Query;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}