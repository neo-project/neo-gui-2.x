using AntShares.Core;
using AntShares.Wallets;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AntShares.UI
{
    public partial class BulkPayDialog : Form
    {
        public string AssetName => (comboBox1.SelectedItem as AssetState).GetName();

        public BulkPayDialog(AssetState asset = null)
        {
            InitializeComponent();
            if (asset == null)
            {
                foreach (UInt256 asset_id in Program.CurrentWallet.FindUnspentCoins().Select(p => p.Output.AssetId).Distinct())
                {
                    comboBox1.Items.Add(Blockchain.Default.GetAssetState(asset_id));
                }
            }
            else
            {
                comboBox1.Items.Add(asset);
                comboBox1.SelectedIndex = 0;
                comboBox1.Enabled = false;
            }
        }

        public TransactionOutput[] GetOutputs()
        {
            return textBox1.Lines.Select(p =>
            {
                UInt256 asset_id = (comboBox1.SelectedItem as AssetState).AssetId;
                string[] line = p.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return new TransactionOutput
                {
                    AssetId = asset_id,
                    Value = Fixed8.Parse(line[1]),
                    ScriptHash = Wallet.ToScriptHash(line[0])
                };
            }).ToArray();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssetState asset = comboBox1.SelectedItem as AssetState;
            if (asset == null)
            {
                textBox3.Text = "";
            }
            else
            {
                textBox3.Text = Program.CurrentWallet.GetAvailable(asset.AssetId).ToString();
            }
            textBox1_TextChanged(this, EventArgs.Empty);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = comboBox1.SelectedIndex >= 0 && textBox1.TextLength > 0;
        }
    }
}
