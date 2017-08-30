using Neo.Network;
using Neo.Properties;
using Neo.SmartContract;
using System;
using System.Windows.Forms;

namespace Neo.UI
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
            ContractParametersContext context = ContractParametersContext.Parse(textBox1.Text);
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
            ContractParametersContext context = ContractParametersContext.Parse(textBox2.Text);
            context.Verifiable.Scripts = context.GetScripts();
            IInventory inventory = (IInventory)context.Verifiable;
            Program.LocalNode.Relay(inventory);
            InformationBox.Show(inventory.Hash.ToString(), Strings.RelaySuccessText, Strings.RelaySuccessTitle);
            button4.Visible = false;
        }
    }
}
