using System.Runtime.InteropServices;

namespace Neo.UI
{
    partial class NetFeeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetFeeDialog));
            this.IsPriority = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.CostContext = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // IsPriority
            // 
            this.IsPriority.AutoSize = true;
            this.IsPriority.Checked = true;
            this.IsPriority.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsPriority.Location = new System.Drawing.Point(75, 160);
            this.IsPriority.Name = "IsPriority";
            this.IsPriority.Size = new System.Drawing.Size(174, 16);
            this.IsPriority.TabIndex = 0;
            this.IsPriority.Text = global::Neo.Properties.Strings.HighPriority;
            this.IsPriority.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.IsPriority.UseVisualStyleBackColor = true;
            this.IsPriority.Visible = false;
            this.IsPriority.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(63, 122);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = global::Neo.Properties.Strings.Confirm;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(198, 122);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = global::Neo.Properties.Strings.Cancel;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // CostContext
            // 
            this.CostContext.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CostContext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CostContext.Location = new System.Drawing.Point(12, 9);
            this.CostContext.Name = "CostContext";
            this.CostContext.Size = new System.Drawing.Size(305, 110);
            this.CostContext.TabIndex = 3;
            this.CostContext.Text = "label1";
            this.CostContext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NetFeeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 203);
            this.Controls.Add(this.CostContext);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.IsPriority);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NetFeeDialog";
            this.Text = "Cost Warning";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox IsPriority;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label CostContext;
    }
}