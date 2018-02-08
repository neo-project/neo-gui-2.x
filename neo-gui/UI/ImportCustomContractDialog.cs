using Neo.SmartContract;
using Neo.Wallets;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class ImportCustomContractDialog : Form
    {
        public Contract GetContract()
        {
            ContractParameterType[] parameterList = textBox1.Text.HexToBytes().Select(p => (ContractParameterType)p).ToArray();
            byte[] redeemScript = textBox2.Text.HexToBytes();
            return Contract.Create(parameterList, redeemScript);
        }

        public KeyPair GetKey()
        {
            if (textBox3.TextLength == 0) return null;
            byte[] privateKey;
            try
            {
                privateKey = Wallet.GetPrivateKeyFromWIF(textBox3.Text);
            }
            catch (FormatException)
            {
                privateKey = textBox3.Text.HexToBytes();
            }
            return new KeyPair(privateKey);
        }

        public ImportCustomContractDialog()
        {
            InitializeComponent();
        }

        private void Input_Changed(object sender, EventArgs e)
        {
            button1.Enabled = textBox1.TextLength > 0 && textBox2.TextLength > 0;
        }
    }
}
