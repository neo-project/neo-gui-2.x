using System;

namespace Neo.UI
{
    partial class SmartContractList
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
            this.components = new System.ComponentModel.Container();
            this.listViewSmartContracts = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAuthor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHasStorage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CopySHtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDescriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewSmartContracts
            // 
            this.listViewSmartContracts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewSmartContracts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeaderVersion,
            this.columnHeaderAuthor,
            this.columnHasStorage,
            this.columnHeader5,
            this.columnHeader4});
            this.listViewSmartContracts.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewSmartContracts.FullRowSelect = true;
            this.listViewSmartContracts.GridLines = true;
            this.listViewSmartContracts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewSmartContracts.HideSelection = false;
            this.listViewSmartContracts.Location = new System.Drawing.Point(13, 14);
            this.listViewSmartContracts.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listViewSmartContracts.Name = "listViewSmartContracts";
            this.listViewSmartContracts.ShowGroups = false;
            this.listViewSmartContracts.Size = new System.Drawing.Size(1550, 562);
            this.listViewSmartContracts.TabIndex = 0;
            this.listViewSmartContracts.UseCompatibleStateImageBehavior = false;
            this.listViewSmartContracts.View = System.Windows.Forms.View.Details;
            this.listViewSmartContracts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewSmartContracts_MouseDown);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Name = "columnHeader3";
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 203;
            // 
            // columnHeaderVersion
            // 
            this.columnHeaderVersion.Text = "Version";
            this.columnHeaderVersion.Width = 89;
            // 
            // columnHeaderAuthor
            // 
            this.columnHeaderAuthor.Text = "Author";
            this.columnHeaderAuthor.Width = 107;
            // 
            // columnHasStorage
            // 
            this.columnHasStorage.Text = "Storage";
            this.columnHasStorage.Width = 78;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Name = "columnHeader5";
            this.columnHeader5.Text = "Status";
            this.columnHeader5.Width = 90;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Name = "columnHeader4";
            this.columnHeader4.Text = "Script Hash";
            this.columnHeader4.Width = 396;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopySHtoolStripMenuItem,
            this.showDescriptionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(224, 64);
            // 
            // CopySHtoolStripMenuItem
            // 
            this.CopySHtoolStripMenuItem.Name = "CopySHtoolStripMenuItem";
            this.CopySHtoolStripMenuItem.Size = new System.Drawing.Size(223, 30);
            this.CopySHtoolStripMenuItem.Text = "Copy Script Hash";
            this.CopySHtoolStripMenuItem.Click += new System.EventHandler(this.CopySHtoolStripMenuItem_Click);
            // 
            // showDescriptionToolStripMenuItem
            // 
            this.showDescriptionToolStripMenuItem.Name = "showDescriptionToolStripMenuItem";
            this.showDescriptionToolStripMenuItem.Size = new System.Drawing.Size(223, 30);
            this.showDescriptionToolStripMenuItem.Text = "Show Description";
            this.showDescriptionToolStripMenuItem.Click += new System.EventHandler(this.showDescriptionMenuItem_Click);
            // 
            // SmartContractList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1576, 590);
            this.Controls.Add(this.listViewSmartContracts);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SmartContractList";
            this.Text = "Smart Contract Monitor";
            this.Activated += new System.EventHandler(this.SmartContractList_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SmartContractList_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewSmartContracts;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CopySHtoolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeaderAuthor;
        private System.Windows.Forms.ColumnHeader columnHeaderVersion;
        private System.Windows.Forms.ColumnHeader columnHasStorage;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripMenuItem showDescriptionToolStripMenuItem;
    }
}