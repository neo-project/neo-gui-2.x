using Neo.Core;
using Neo.Properties;
using Neo.VM;
using Neo.Wallets;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using Neo.SmartContract;

namespace Neo.UI
{
    public partial class InflationRateDialog : Form
    {
        public InflationRateDialog()
        {
            InitializeComponent();
        }

        public Transaction GetTransaction()
        {
            string command = "inflationRate";
            string scriptHash = Settings.Default.NEP5Watched.OfType<string>().ToArray()[0];
            Debug.WriteLine(scriptHash);
            UInt160 script_hash = UInt160.Parse(scriptHash);
            string address = Wallet.ToAddress(script_hash);
            Debug.WriteLine(address);

            BigInteger iRate = BigInteger.Parse(this.textBox1.Text);
            object[] param = { iRate};
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
