using Neo.Wallets;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class ContractDetailsDialog : Form
    {
        public ContractDetailsDialog(VerificationContract contract)
        {
            InitializeComponent();
            textBox1.Text = Wallet.ToAddress(contract.ScriptHash);
            textBox2.Text = contract.ScriptHash.ToString();
            textBox3.Text = contract.Script.ToHexString();
        }
    }
}
