using Neo.Core;
using Neo.Wallets;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class BulkPayDialog : Form
    {
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

        public TxOutListBoxItem[] GetOutputs()
        {
            AssetState asset = (AssetState)comboBox1.SelectedItem;
            return textBox1.Lines.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p =>
            {
                string[] line = p.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                return new TxOutListBoxItem
                {
                    AssetName = asset.GetName(),
                    AssetId = asset.AssetId,
                    Value = new BigDecimal(Fixed8.Parse(line[1]).GetData(), 8),
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
