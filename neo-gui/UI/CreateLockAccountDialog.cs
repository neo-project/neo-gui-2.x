using Neo.SmartContract;
using Neo.VM;
using Neo.Wallets;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class CreateLockAccountDialog : Form
    {
        public CreateLockAccountDialog()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(Program.CurrentWallet.GetAccounts().Where(p => !p.WatchOnly && p.Contract.Script.IsStandardContract()).Select(p => p.GetKey()).ToArray());
        }

        public Contract GetContract()
        {
            uint timestamp = dateTimePicker1.Value.ToTimestamp();
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitPush(GetKey().PublicKey);
                sb.EmitPush(timestamp);
                // Lock 2.0 in mainnet tx:4e84015258880ced0387f34842b1d96f605b9cc78b308e1f0d876933c2c9134b
                sb.EmitAppCall(UInt160.Parse("d3cce84d0800172d09c88ccad61130611bd047a4"));
                return Contract.Create(new[] { ContractParameterType.Signature }, sb.ToArray());
            }
        }

        public KeyPair GetKey()
        {
            return (KeyPair)comboBox1.SelectedItem;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = comboBox1.SelectedIndex >= 0;
        }
    }
}
