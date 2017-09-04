using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.SmartContract;
using Neo.VM;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class ElectionDialog : Form
    {
        public ElectionDialog()
        {
            InitializeComponent();
        }

        public InvocationTransaction GetTransaction()
        {
            ECPoint pubkey = (ECPoint)comboBox1.SelectedItem;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitSysCall("Neo.Validator.Register", pubkey);
                return new InvocationTransaction
                {
                    Attributes = new[]
                    {
                        new TransactionAttribute
                        {
                            Usage = TransactionAttributeUsage.Script,
                            Data = Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash().ToArray()
                        }
                    },
                    Script = sb.ToArray()
                };
            }
        }

        private void ElectionDialog_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(Program.CurrentWallet.GetContracts().Where(p => p.IsStandard).Select(p => Program.CurrentWallet.GetKey(p.PublicKeyHash).PublicKey).ToArray());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = comboBox1.SelectedIndex >= 0;
        }
    }
}
