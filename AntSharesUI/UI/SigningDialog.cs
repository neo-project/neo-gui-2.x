using AntShares.Network;
using AntShares.Properties;
using AntShares.Wallets;
using System;
using System.Windows.Forms;

namespace AntShares.UI
{
    internal partial class SigningDialog : Form
    {
        public SigningDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show(Strings.SigningFailedNoDataMessage);
                return;
            }
            SignatureContext context = SignatureContext.Parse(textBox1.Text);
            if (!Program.CurrentWallet.Sign(context))
            {
                MessageBox.Show(Strings.SigningFailedKeyNotFoundMessage);
                return;
            }
            textBox2.Text = context.ToString();
            if (context.Completed) button4.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.SelectAll();
            textBox2.Copy();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SignatureContext context = SignatureContext.Parse(textBox2.Text);
            context.Verifiable.Scripts = context.GetScripts();
            IInventory inventory = (IInventory)context.Verifiable;
            Program.LocalNode.Relay(inventory);
            InformationBox.Show(inventory.Hash.ToString(), Strings.RelaySuccessText, Strings.RelaySuccessTitle);
            button4.Visible = false;
        }
    }
}
