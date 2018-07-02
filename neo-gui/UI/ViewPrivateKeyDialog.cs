using Neo.Wallets;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class ViewPrivateKeyDialog : Form
    {
        public ViewPrivateKeyDialog(WalletAccount account)
        {
            InitializeComponent();
            KeyPair key = account.GetKey();
            textBox3.Text = account.Address;
            textBox4.Text = key.PublicKey.EncodePoint(true).ToHexString();
            textBox1.Text = key.PrivateKey.ToHexString();
            textBox2.Text = key.Export();
        }
    }
}
