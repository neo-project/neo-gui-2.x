using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.VM;
using Neo.Wallets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class ContractTransferDialog : Form
    {
        public ContractTransferDialog(AssetState asset = null, UInt160 scriptHash = null)
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

            comboBox2.Items.AddRange(Program.CurrentWallet.GetContracts().Where(p => p.IsStandard).Select(p => Program.CurrentWallet.GetKey(p.PublicKeyHash).PublicKey).ToArray());
        }

        public Transaction GetTransaction2()
        {
            UInt160 script_hash = Wallet.ToScriptHash(textBox1.Text);
            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(script_hash.ToArray());
                script = sb.ToArray();
            }

            AssetState asset = (AssetState)comboBox1.SelectedItem;
            TransactionOutput[] outputs = new TransactionOutput[1];
            outputs[0] = new TransactionOutput
            {
                AssetId = asset.AssetId,
                ScriptHash = Wallet.ToScriptHash(textBox1.Text),
                Value = Fixed8.Parse(textBox2.Text)
            };

            return Program.CurrentWallet.MakeTransaction(new InvocationTransaction
            {
                Outputs = outputs,
                Script = script
            });
        }

        public Transaction GetTransaction1()
        {
            ECPoint owner = (ECPoint)comboBox2.SelectedItem;
            UInt160 script_hash = Wallet.ToScriptHash(textBox1.Text);
            AssetState asset = (AssetState)comboBox1.SelectedItem;
            TransactionOutput[] outputs = new TransactionOutput[1];
            outputs[0] = new TransactionOutput
            {
                AssetId = asset.AssetId,
                ScriptHash = Wallet.ToScriptHash(textBox1.Text),
                Value = Fixed8.Parse(textBox2.Text)
            };

            using (ScriptBuilder sb = new ScriptBuilder())
            {
                return Program.CurrentWallet.MakeTransaction(new InvocationTransaction
                {
                    Attributes = new[]
                    {
                        new TransactionAttribute
                        {
                            Usage = TransactionAttributeUsage.Script,
                            Data = Contract.CreateSignatureRedeemScript(owner).ToScriptHash().ToArray()
                        }
                    },
                    Script = sb.ToArray()
                });

            }
        }

        public Transaction GetTransaction()
        {
            ECPoint owner = (ECPoint)comboBox2.SelectedItem;
            UInt160 script_hash = Wallet.ToScriptHash(textBox1.Text);
            AssetState asset = (AssetState)comboBox1.SelectedItem;
            TransactionOutput[] outputs = new TransactionOutput[1];
            outputs[0] = new TransactionOutput
            {
                AssetId = asset.AssetId,
                ScriptHash = Wallet.ToScriptHash(textBox1.Text),
                Value = Fixed8.Parse(textBox2.Text)
            };

            return Program.CurrentWallet.MakeTransaction(new ContractTransaction
            {
                Outputs = outputs,
                Attributes = new[]
                {
                    new TransactionAttribute
                    {
                        Usage = TransactionAttributeUsage.Script,
                        Data = Contract.CreateSignatureRedeemScript(owner).ToScriptHash().ToArray()
                    }
                }
            });
        }

        public Transaction GetTransaction4()
        {
            ECPoint owner = (ECPoint)comboBox2.SelectedItem;
            UInt160 script_hash = Wallet.ToScriptHash(textBox1.Text);
            AssetState asset = (AssetState)comboBox1.SelectedItem;
            TransactionOutput[] outputs = new TransactionOutput[1];
            outputs[0] = new TransactionOutput
            {
                AssetId = asset.AssetId,
                ScriptHash = Wallet.ToScriptHash(textBox1.Text),
                Value = Fixed8.Parse(textBox2.Text)
            };

            return Program.CurrentWallet.MakeTransaction(new ContractTransaction
            {
                Outputs = outputs,
                Attributes = new[]
                {
                    new TransactionAttribute
                    {
                        Usage = TransactionAttributeUsage.Script,
                        Data = script_hash.ToArray()
                    }
                }
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
