using Neo.Core;
using Neo.Wallets;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class IssueDialog : Form
    {
        public IssueDialog(AssetState asset = null)
        {
            InitializeComponent();
            if (asset != null)
            {
                textBox5.Text = asset.AssetId.ToString();
                textBox5.Enabled = false;
            }
        }

        public IssueTransaction GetTransaction()
        {
            if (txOutListBox1.Asset == null) return null;
            return Program.CurrentWallet.MakeTransaction(new IssueTransaction
            {
                Version = 1,
                Outputs = txOutListBox1.Items.GroupBy(p => p.Output.ScriptHash).Select(g => new TransactionOutput
                {
                    AssetId = txOutListBox1.Asset.AssetId,
                    Value = g.Sum(p => p.Output.Value),
                    ScriptHash = g.Key
                }).ToArray()
            }, fee: Fixed8.One);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (UInt256.TryParse(textBox5.Text, out UInt256 asset_id))
                txOutListBox1.Asset = Blockchain.Default.GetAssetState(asset_id);
            else
                txOutListBox1.Asset = null;
            if (txOutListBox1.Asset == null)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                groupBox3.Enabled = false;
            }
            else
            {
                textBox1.Text = txOutListBox1.Asset.Owner.ToString();
                textBox2.Text = Wallet.ToAddress(txOutListBox1.Asset.Admin);
                textBox3.Text = txOutListBox1.Asset.Amount == -Fixed8.Satoshi ? "+\u221e" : txOutListBox1.Asset.Amount.ToString();
                textBox4.Text = txOutListBox1.Asset.Available.ToString();
                groupBox3.Enabled = true;
            }
            txOutListBox1.Clear();
        }

        private void txOutListBox1_ItemsChanged(object sender, EventArgs e)
        {
            button3.Enabled = txOutListBox1.ItemCount > 0;
        }
    }
}
