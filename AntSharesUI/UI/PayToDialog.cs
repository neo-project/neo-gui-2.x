using AntShares.Core;
using AntShares.Wallets;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AntShares.UI
{
    internal partial class PayToDialog : Form
    {
        public string AssetName => (comboBox1.SelectedItem as AssetState).GetName();

        public PayToDialog(AssetState asset = null, UInt160 scriptHash = null)
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
            if (scriptHash != null)
            {
                textBox1.Text = Wallet.ToAddress(scriptHash);
                textBox1.ReadOnly = true;
            }
        }

        public TransactionOutput GetOutput()
        {
            return new TransactionOutput
            {
                AssetId = (comboBox1.SelectedItem as AssetState).AssetId,
                Value = Fixed8.Parse(textBox2.Text),
                ScriptHash = Wallet.ToScriptHash(textBox1.Text)
            };
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
            Fixed8 amount;
            if (!Fixed8.TryParse(textBox2.Text, out amount))
            {
                button1.Enabled = false;
                return;
            }
            if (amount.GetData() % (long)Math.Pow(10, 8 - (comboBox1.SelectedItem as AssetState).Precision) != 0)
            {
                button1.Enabled = false;
                return;
            }
            button1.Enabled = true;
        }
    }
}
