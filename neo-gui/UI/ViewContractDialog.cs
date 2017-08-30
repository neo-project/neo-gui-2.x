using Neo.Wallets;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class ViewContractDialog : Form
    {
        public ViewContractDialog(VerificationContract contract)
        {
            InitializeComponent();
            textBox1.Text = contract.Address;
            textBox2.Text = contract.ScriptHash.ToString();
            textBox3.Text = contract.ParameterList.Cast<byte>().ToArray().ToHexString();
            textBox4.Text = contract.Script.ToHexString();
        }
    }
}
