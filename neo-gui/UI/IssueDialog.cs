using Neo.Ledger;
using Neo.Network.P2P.Payloads;
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
                Outputs = txOutListBox1.Items.GroupBy(p => p.ScriptHash).Select(g => new TransactionOutput
                {
                    AssetId = (UInt256)txOutListBox1.Asset.AssetId,
                    Value = g.Sum(p => new Fixed8((long)p.Value.Value)),
                    ScriptHash = g.Key
                }).ToArray()
            }, fee: Fixed8.One);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            AssetState state;
            if (UInt256.TryParse(textBox5.Text, out UInt256 asset_id))
            {
                state = Blockchain.Singleton.Store.GetAssets().TryGet(asset_id);
                txOutListBox1.Asset = new AssetDescriptor(asset_id);
            }
            else
            {
                state = null;
                txOutListBox1.Asset = null;
            }
            if (state == null)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                groupBox3.Enabled = false;
            }
            else
            {
                textBox1.Text = state.Owner.ToString();
                textBox2.Text = state.Admin.ToAddress();
                textBox3.Text = state.Amount == -Fixed8.Satoshi ? "+\u221e" : state.Amount.ToString();
                textBox4.Text = state.Available.ToString();
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
