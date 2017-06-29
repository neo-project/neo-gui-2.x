using AntShares.Core;
using AntShares.Wallets;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AntShares.UI
{
    internal partial class IssueDialog : Form
    {
        public IssueDialog(AssetState asset = null)
        {
            InitializeComponent();
            if (asset == null)
            {
                comboBox1.Items.AddRange(Program.CurrentWallet.GetTransactions<RegisterTransaction>().Select(p => Blockchain.Default.GetAssetState(p.Hash)).Where(p => p != null && Program.CurrentWallet.ContainsAddress(p.Issuer)).ToArray());
            }
            else
            {
                comboBox1.Items.Add(asset);
            }
        }

        public IssueTransaction GetTransaction()
        {
            if (txOutListBox1.Asset == null) return null;
            return Program.CurrentWallet.MakeTransaction(new IssueTransaction
            {
                Outputs = txOutListBox1.Items.GroupBy(p => p.Output.ScriptHash).Select(g => new TransactionOutput
                {
                    AssetId = txOutListBox1.Asset.AssetId,
                    Value = g.Sum(p => p.Output.Value),
                    ScriptHash = g.Key
                }).ToArray()
            }, fee: Fixed8.Zero);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txOutListBox1.Asset = comboBox1.SelectedItem as AssetState;
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
                label6.Text = $"{new IssueTransaction { Outputs = new[] { new TransactionOutput { AssetId = txOutListBox1.Asset.AssetId } } }.SystemFee } ANC";
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
