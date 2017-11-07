using Neo.SmartContract;
using Neo.VM;
using Neo.Wallets;
using System;
using System.Numerics;
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
            RefreshRefundsButton.Enabled = false;
        }

        private void Query_Click(object sender, EventArgs e)
        {

            scriptHash = UInt160.Parse(contractScriptHashTextBox.Text);
            asset = new AssetDescriptor(this.scriptHash);
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
                txtbx_totalSupply.Text = totalSupply.ToString();
                RefreshRefundsButton.Enabled = true;                
            }
            else
            {
                MessageBox.Show("Query Failed");
                RefreshRefundsButton.Enabled = false;
            }
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

        private void RefreshRefundsButton_Click(object sender, EventArgs e)
        {
            byte[] script;
            /** Now load refunds asynchronously, since this can be slow to load. */
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(scriptHash, "listRefund");                
                script = sb.ToArray();
            }             
            refundsGridView.Rows.Clear();
            ApplicationEngine engine = ApplicationEngine.Run(script);                 
            if (!engine.State.HasFlag(VMState.FAULT))
            {
                byte[] refunds = engine.EvaluationStack.Pop().GetByteArray();
                int currentStart = 0;
                for (int i = 0; i < refunds.Length; i++)
                {
                    /** Move until we find the first marker, the '=' symbol */
                    if ((char) refunds[i] == '=')
                    {                
                        byte[] currentKey = new byte[i -  currentStart];
                        /* Get the bytes for the script hash of the address for the refund */
                        Array.Copy(refunds, currentStart , currentKey, 0, currentKey.Length);
                        var j = i + 1;
                        
                        /* Move until the end or until we find the next refund, with the marker '@' */
                        while (j < refunds.Length && (char) refunds[j] != '@' ){ j++; }
                        
                        /** Get the bytes of the refund numeric value */
                        byte[] intBytes = new byte[j - i - 1];
                        Array.Copy(refunds, i + 1, intBytes, 0, j - i - 1);
                        
                        /**  Add the new row to the refunds */
                        refundsGridView.Rows.Add(new object[]{Wallet.ToAddress(new UInt160(currentKey)), new BigDecimal(new BigInteger(intBytes), asset.Precision)});                            
                        i = j + 1;
                        currentStart = i;
                    }                       
                }                                                                               
            }
        }
    }
}
