using Neo.Properties;
using Neo.SmartContract;
using Neo.VM;
using Neo.Wallets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text; 
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class QueryDialog : Form
    {
        private UInt160 scriptHash;
        private AssetDescriptor asset;

        public QueryDialog()
        {
            InitializeComponent();
        }

        private void Query_Click(object sender, EventArgs e)
        {

            this.scriptHash = UInt160.Parse(contractScriptHashTextBox.Text);
            this.asset = new AssetDescriptor(this.scriptHash);
            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(scriptHash, "totalSupply");
                sb.EmitAppCall(scriptHash, "symbol");
                sb.EmitAppCall(scriptHash, "currentRate");
                
                for (int i = 0; i < 3; i++) {
                    sb.EmitAppCall(scriptHash, "roundTotal", new ContractParameter[] {
                      new ContractParameter{
                            Type = ContractParameterType.Integer,
                            Value = new BigInteger(i)
                      }
                    });
                }              
                script = sb.ToArray();
            }
            ApplicationEngine engine = ApplicationEngine.Run(script);
            if (!engine.State.HasFlag(VMState.FAULT))
            {
                this.txtbx_name.Text = asset.AssetName;
                this.txtbx_precision.Text = asset.Precision.ToString();

                // Round 3
                txtbx_round_3.Text = new BigDecimal(engine.EvaluationStack.Pop().GetBigInteger(), asset.Precision).ToString();

                // Round 2
                txtbx_round_2.Text = new BigDecimal(engine.EvaluationStack.Pop().GetBigInteger(), asset.Precision).ToString();

                // Round 1
                txtbx_round_1.Text = new BigDecimal(engine.EvaluationStack.Pop().GetBigInteger(), asset.Precision).ToString();               

                // Current swap rate
                txtbx_swap_rate.Text = engine.EvaluationStack.Pop().GetBigInteger().ToString();
                
                //symbol
                this.txtbx_symbol.Text = engine.EvaluationStack.Pop().GetString();

                //totalSupply
                BigInteger _totalSupply = engine.EvaluationStack.Pop().GetBigInteger();
                BigDecimal totalSupply = new BigDecimal(_totalSupply, asset.Precision);
                this.txtbx_totalSupply.Text = totalSupply.ToString();               
            }
            else
            {
                MessageBox.Show("Query Failed");
            }
        }

        public static DateTime ConvertIntDateTime(double d)
        {
            DateTime time = System.DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }

        private void CheckBalance_Click(object sender, EventArgs e)
        {
            UInt160 address = Wallet.ToScriptHash(txtbx_address.Text);
            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(scriptHash, "balanceOf", address);
                script = sb.ToArray();
            }
            ApplicationEngine engine = ApplicationEngine.Run(script);
            if (!engine.State.HasFlag(VMState.FAULT))
            {
                BigInteger _balance = engine.EvaluationStack.Pop().GetBigInteger();
                BigDecimal balance = new BigDecimal(_balance, asset.Precision);
                this.txtbx_balance.Text = balance.ToString()+ " " + asset.AssetName;
            }
            else
            {
                MessageBox.Show("Query Failed");
            }
        }
    }
}
