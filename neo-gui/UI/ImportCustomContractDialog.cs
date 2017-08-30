using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.SmartContract;
using Neo.Wallets;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class ImportCustomContractDialog : Form
    {
        public VerificationContract GetContract()
        {
            UInt160 publicKeyHash = ((ECPoint)comboBox1.SelectedItem).EncodePoint(true).ToScriptHash();
            ContractParameterType[] parameterList = textBox1.Text.HexToBytes().Select(p => (ContractParameterType)p).ToArray();
            byte[] redeemScript = textBox2.Text.HexToBytes();
            return VerificationContract.Create(publicKeyHash, parameterList, redeemScript);
        }

        public ImportCustomContractDialog()
        {
            InitializeComponent();
        }

        private void ImportCustomContractDialog_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(Program.CurrentWallet.GetContracts().Where(p => p.IsStandard).Select(p => Program.CurrentWallet.GetKey(p.PublicKeyHash).PublicKey).ToArray());
        }

        private void Input_Changed(object sender, EventArgs e)
        {
            button1.Enabled = comboBox1.SelectedIndex >= 0 && textBox1.TextLength > 0 && textBox2.TextLength > 0;
        }
    }
}
