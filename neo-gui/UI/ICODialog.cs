using Neo.Core;
using Neo.Properties;
using Neo.SmartContract;
using Neo.VM;
using Neo.Wallets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class ICODialog : Form
    {
        public ICODialog(AssetState asset = null, UInt160 scriptHash = null)
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
                //textBox1.Text = Wallet.ToAddress(scriptHash);
                //textBox1.ReadOnly = true;
            }
            else {
                string _scriptHash = Settings.Default.NEP5Watched.OfType<string>().ToArray()[0];
                textBox1.Text = _scriptHash;
                textBox1.ReadOnly = true;
            }
        }

        public Transaction GetTransaction2()
        {
            AssetState asset = (AssetState)comboBox1.SelectedItem;
            TransactionOutput[] outputs = new TransactionOutput[1];
            outputs[0] = new TransactionOutput
            {
                AssetId = asset.AssetId,
                ScriptHash = Wallet.ToScriptHash(textBox1.Text),
                Value = Fixed8.Parse(textBox2.Text)
            };
            ContractTransaction ctx = Program.CurrentWallet.MakeTransaction(new ContractTransaction { Outputs = outputs }, fee: Fixed8.Zero);
            return ctx;
        }

        public Transaction GetTransaction()
        {
            string command = "mintTokens";
            string scriptHash = Settings.Default.NEP5Watched.OfType<string>().ToArray()[0];
            Debug.WriteLine(scriptHash);
            UInt160 script_hash = UInt160.Parse(scriptHash);

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
                //ScriptHash = Wallet.ToScriptHash(textBox1.Text),
                ScriptHash = UInt160.Parse(textBox1.Text),
                Value = Fixed8.Parse(textBox2.Text)
            };

            //return new InvocationTransaction
            //{
            //    Version=1,
            //    Outputs = outputs,
            //    Script = script
            //};

            return Program.CurrentWallet.MakeTransaction(new InvocationTransaction
            {
                //Version = tx.Version,
                //Gas = tx.Gas,
                //Attributes = tx.Attributes,
                //Inputs = tx.Inputs,
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

        private void button2_Click(object sender, EventArgs e)
        {
            //UInt160 script_hash = UInt160.Parse("b90449b9c21fbb4535b5b51648cdf6416ef3ec82");
            string scriptHash = Settings.Default.NEP5Watched.OfType<string>().ToArray()[0];
            Debug.WriteLine(scriptHash);
            UInt160 script_hash = UInt160.Parse(scriptHash);

            string address = Wallet.ToAddress(script_hash);
            Debug.WriteLine(address);

            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(script_hash, "decimals");
                sb.EmitAppCall(script_hash, "totalSupply");
                script = sb.ToArray();
            }
            ApplicationEngine engine = TestEngine.Run(script);
            if (engine != null)
            {
                BigInteger _totalSupply = engine.EvaluationStack.Pop().GetBigInteger();
                byte decimals = (byte)engine.EvaluationStack.Pop().GetBigInteger();
                BigDecimal totalSupply = new BigDecimal(_totalSupply, decimals);
                this.textBox4.Text = totalSupply.ToString();
            }
            else {
                MessageBox.Show("Query Failed");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string scriptHash = Settings.Default.NEP5Watched.OfType<string>().ToArray()[0];
            UInt160 script_hash = UInt160.Parse(scriptHash);

            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(script_hash, "queryInflationRate");
                script = sb.ToArray();
            }
            ApplicationEngine engine = TestEngine.Run(script);
            if (engine != null)
            {
                BigInteger iRate = engine.EvaluationStack.Pop().GetBigInteger();
                this.textBox5.Text = iRate.ToString();
            }
            else
            {
                MessageBox.Show("Query Failed");
            }
        }

        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string scriptHash = Settings.Default.NEP5Watched.OfType<string>().ToArray()[0];
            UInt160 script_hash = UInt160.Parse(scriptHash);

            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(script_hash, "queryInflationStartTime");
                script = sb.ToArray();
            }
            ApplicationEngine engine = TestEngine.Run(script);
            if (engine != null)
            {
                BigInteger startTime = engine.EvaluationStack.Pop().GetBigInteger();

                this.textBox6.Text = ConvertIntDateTime((double)startTime).ToString();
            }
            else
            {
                MessageBox.Show("Query Failed");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string command = "balanceOf";
            string scriptHash = Settings.Default.NEP5Watched.OfType<string>().ToArray()[0];
            UInt160 script_hash = UInt160.Parse(scriptHash);

            UInt160[] addresses = Program.CurrentWallet.GetAddresses().ToArray();
            object[] param = { addresses[0] };
            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(script_hash, command, param);
                script = sb.ToArray();
            }
            ApplicationEngine engine = TestEngine.Run(script);
            if (engine != null)
            {
                BigInteger balance = engine.EvaluationStack.Pop().GetBigInteger();
                this.textBox7.Text = balance.ToString();
            }
            else
            {
                MessageBox.Show("Query Failed");
            }
        }
    }
}
