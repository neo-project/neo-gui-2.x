using Neo.Properties;
using Neo.Wallets;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class PayToDialog : Form
    {
        public PayToDialog(AssetDescriptor asset = null, UInt160 scriptHash = null)
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
            if (scriptHash != null)
            {
                textBox1.Text = scriptHash.ToAddress();
                textBox1.ReadOnly = true;
            }
        }

        public TxOutListBoxItem GetOutput()
        {
            AssetDescriptor asset = (AssetDescriptor)comboBox1.SelectedItem;
            return new TxOutListBoxItem
            {
                AssetName = asset.AssetName,
                AssetId = asset.AssetId,
                Value = BigDecimal.Parse(textBox2.Text, asset.Decimals),
                ScriptHash = textBox1.Text.ToScriptHash()
            };
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
            textBox_TextChanged(this, EventArgs.Empty);
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0 || textBox1.TextLength == 0 || textBox2.TextLength == 0)
            {
                button1.Enabled = false;
                return;
            }
            try
            {
                textBox1.Text.ToScriptHash();
            }
            catch (FormatException)
            {
                button1.Enabled = false;
                return;
            }
            AssetDescriptor asset = (AssetDescriptor)comboBox1.SelectedItem;
            if (!BigDecimal.TryParse(textBox2.Text, asset.Decimals, out BigDecimal amount))
            {
                button1.Enabled = false;
                return;
            }
            if (amount.Sign <= 0)
            {
                button1.Enabled = false;
                return;
            }
            button1.Enabled = true;
        }
    }
}
