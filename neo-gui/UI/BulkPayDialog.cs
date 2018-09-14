using Neo.Properties;
using Neo.Wallets;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class BulkPayDialog : Form
    {
        public BulkPayDialog(AssetDescriptor asset = null)
        {
            InitializeComponent();
            if (asset == null)
            {
                foreach (UInt256 asset_id in Program.CurrentWallet.FindUnspentCoins().Select(p => p.Output.AssetId).Distinct())
                {
                    comboBox1.Items.Add(new AssetDescriptor(asset_id));
                }
                foreach (string s in Settings.Default.NEP5Watched)
                {
                    UInt160 asset_id = UInt160.Parse(s);
                    try
                    {
                        comboBox1.Items.Add(new AssetDescriptor(asset_id));
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }
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
            AssetDescriptor asset = (AssetDescriptor)comboBox1.SelectedItem;
            return textBox1.Lines.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p =>
            {
                string[] line = p.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                return new TxOutListBoxItem
                {
                    AssetName = asset.AssetName,
                    AssetId = asset.AssetId,
                    Value = BigDecimal.Parse(line[1], asset.Decimals),
                    ScriptHash = line[0].ToScriptHash()
                };
            }).Where(p => p.Value.Value != 0).ToArray();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is AssetDescriptor asset)
            {
                textBox3.Text = Program.CurrentWallet.GetAvailable(asset.AssetId).ToString();
            }
            else
            {
                textBox3.Text = "";
            }
            textBox1_TextChanged(this, EventArgs.Empty);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = comboBox1.SelectedIndex >= 0 && textBox1.TextLength > 0;
        }
    }
}
