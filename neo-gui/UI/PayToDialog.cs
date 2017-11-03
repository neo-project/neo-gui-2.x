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
                textBox1.Text = Wallet.ToAddress(scriptHash);
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
                Value = new BigDecimal(Fixed8.Parse(textBox2.Text).GetData(), 8),
                ScriptHash = Wallet.ToScriptHash(textBox1.Text)
            };
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssetDescriptor asset = comboBox1.SelectedItem as AssetDescriptor;
            if (asset == null)
            {
                textBox3.Text = "";
            }
            else
            {
                textBox3.Text = Program.CurrentWallet.GetAvailable(asset.AssetId).ToString();
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
                Wallet.ToScriptHash(textBox1.Text);
            }
            catch (FormatException)
            {
                button1.Enabled = false;
                return;
            }
            if (!Fixed8.TryParse(textBox2.Text, out Fixed8 amount))
            {
                button1.Enabled = false;
                return;
            }
            if (amount == Fixed8.Zero)
            {
                button1.Enabled = false;
                return;
            }
            if (amount.GetData() % (long)Math.Pow(10, 8 - (comboBox1.SelectedItem as AssetDescriptor).Decimals) != 0)
            {
                button1.Enabled = false;
                return;
            }
            button1.Enabled = true;
        }
    }
}
