using Neo.Core;
using Neo.Properties;
using Neo.VM;
using Neo.Wallets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neo
{
    public partial class MintTokensDialog : Form
    {
        string scriptHash;

        public MintTokensDialog(string scriptHash)
        {
            InitializeComponent();
            this.scriptHash = scriptHash;
            this.textBox1.Text = scriptHash;
            string address = Wallet.ToAddress(UInt160.Parse(scriptHash));
            this.textBox4.Text = address;
            foreach (UInt256 asset_id in Program.CurrentWallet.FindUnspentCoins().Select(p => p.Output.AssetId).Distinct())
            {
                comboBox1.Items.Add(Blockchain.Default.GetAssetState(asset_id));
            }
        }

        public Transaction GetTransaction()
        {
            string command = "mintTokens";
            UInt160 script_hash = UInt160.Parse(this.scriptHash);

            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitPush(0);
                sb.EmitPush(command);
                sb.EmitAppCall(script_hash.ToArray());
                script = sb.ToArray();
            }

            AssetState asset = (AssetState)comboBox1.SelectedItem;
            TransactionOutput[] outputs = new TransactionOutput[1];
            outputs[0] = new TransactionOutput
            {
                AssetId = asset.AssetId,
                ScriptHash = UInt160.Parse(textBox1.Text),
                Value = Fixed8.Parse(textBox2.Text)
            };

            return Program.CurrentWallet.MakeTransaction(new InvocationTransaction
            {
                Outputs = outputs,
                Script = script
            });
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
            //try
            //{
            //    Wallet.ToScriptHash(textBox1.Text);
            //}
            //catch (FormatException)
            //{
            //    button1.Enabled = false;
            //    return;
            //}
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
