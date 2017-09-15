using Neo.Core;
using Neo.Properties;
using Neo.VM;
using Neo.Wallets;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using Neo.SmartContract;
using System;

namespace Neo.UI
{
    public partial class InflationRateDialog : Form
    {
        private string scriptHash;
        private const long multiplier = 1000000000000;
        public InflationRateDialog(string scriptHash)
        {
            InitializeComponent();
            this.scriptHash = scriptHash;
        }

        public Transaction GetTransaction()
        {
            string command = "inflationRate";
            UInt160 script_hash = UInt160.Parse(this.scriptHash);
            string strRate = this.textBox1.Text;
            double _iRate = double.Parse(strRate);
            _iRate = Math.Floor(_iRate * multiplier);
            
            BigInteger iRate = new BigInteger(_iRate);
            object[] param = { iRate };
            byte[] script;

            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(script_hash, command, param);
                script = sb.ToArray();
            }

            return Program.CurrentWallet.MakeTransaction(new InvocationTransaction
            {
                Script = script
            });
        }
    }
}
