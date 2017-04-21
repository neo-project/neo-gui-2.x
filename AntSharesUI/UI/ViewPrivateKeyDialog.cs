using AntShares.Wallets;
using System.Windows.Forms;

namespace AntShares.UI
{
    internal partial class ViewPrivateKeyDialog : Form
    {
        public ViewPrivateKeyDialog(KeyPair key, UInt160 scriptHash)
        {
            InitializeComponent();
            textBox3.Text = Wallet.ToAddress(scriptHash);
            textBox4.Text = key.PublicKey.EncodePoint(true).ToHexString();
            using (key.Decrypt())
            {
                textBox1.Text = key.PrivateKey.ToHexString();
            }
            textBox2.Text = key.Export();
        }
    }
}
