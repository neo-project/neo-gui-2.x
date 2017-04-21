using AntShares.Core;
using AntShares.Network;
using AntShares.Properties;
using AntShares.Wallets;
using System;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace AntShares.UI
{
    internal partial class DeveloperToolsForm : Form
    {
        private SignatureContext context;

        public DeveloperToolsForm()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return;
            listBox2.Items.Clear();
            if (Program.CurrentWallet == null) return;
            Contract contract = Program.CurrentWallet.GetContract(Wallet.ToScriptHash((string)listBox1.SelectedItem));
            if (contract == null) return;
            listBox2.Items.AddRange(contract.ParameterList.OfType<object>().ToArray());
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex < 0) return;
            textBox1.Clear();
            textBox2.Clear();
            byte[] parameter = context.GetParameter(Wallet.ToScriptHash((string)listBox1.SelectedItem), listBox2.SelectedIndex);
            if (parameter == null) return;
            ContractParameterType type = (ContractParameterType)listBox2.SelectedItem;
            switch (type)
            {
                case ContractParameterType.Integer:
                    textBox1.Text = new BigInteger(parameter).ToString();
                    break;
                case ContractParameterType.Hash160:
                    textBox1.Text = new UInt160(parameter).ToString();
                    break;
                case ContractParameterType.Hash256:
                    textBox1.Text = new UInt256(parameter).ToString();
                    break;
                default:
                    textBox1.Text = parameter.ToHexString();
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = InputBox.Show("SignatureContext", "SignatureContext");
            if (string.IsNullOrEmpty(input)) return;
            context = SignatureContext.Parse(input);
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            textBox1.Clear();
            textBox2.Clear();
            listBox1.Items.AddRange(context.ScriptHashes.Select(p => Wallet.ToAddress(p)).ToArray());
            button2.Enabled = true;
            button4.Visible = context.Completed;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InformationBox.Show(context.ToString(), "SignatureContext", "SignatureContext");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return;
            if (listBox2.SelectedIndex < 0) return;
            byte[] parameter;
            ContractParameterType type = (ContractParameterType)listBox2.SelectedItem;
            switch (type)
            {
                case ContractParameterType.Integer:
                    parameter = BigInteger.Parse(textBox2.Text).ToByteArray();
                    break;
                case ContractParameterType.Hash160:
                    parameter = UInt160.Parse(textBox2.Text).ToArray();
                    break;
                case ContractParameterType.Hash256:
                    parameter = UInt256.Parse(textBox2.Text).ToArray();
                    break;
                default:
                    parameter = textBox2.Text.HexToBytes();
                    break;
            }
            Contract contract = Program.CurrentWallet.GetContract(Wallet.ToScriptHash((string)listBox1.SelectedItem));
            if (!context.Add(contract, listBox2.SelectedIndex, parameter))
                throw new InvalidOperationException();
            textBox1.Text = textBox2.Text;
            button4.Visible = context.Completed;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            context.Verifiable.Scripts = context.GetScripts();
            IInventory inventory = (IInventory)context.Verifiable;
            Program.LocalNode.Relay(inventory);
            InformationBox.Show(inventory.Hash.ToString(), Strings.RelaySuccessText, Strings.RelaySuccessTitle);
        }
    }
}
