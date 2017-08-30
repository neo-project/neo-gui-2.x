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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class InflationStartTimeDialog : Form
    {
        private string scriptHash;

        public InflationStartTimeDialog(string scriptHash)
        {
            InitializeComponent();
            this.scriptHash = scriptHash;
        }

        public Core.Transaction GetTransaction()
        {
            string command = "inflationStartTime";
            UInt160 script_hash = UInt160.Parse(this.scriptHash);

            uint timestamp = dateTimePicker1.Value.ToTimestamp();
            object[] param = { timestamp };

            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(script_hash, command, timestamp);
                script = sb.ToArray();
            }

            return Program.CurrentWallet.MakeTransaction(new InvocationTransaction
            {
                Script = script
            });

        }
    }
}
