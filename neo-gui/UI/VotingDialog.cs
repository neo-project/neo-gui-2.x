using Neo.Cryptography.ECC;
using Neo.IO;
using Neo.Ledger;
using Neo.Network.P2P.Payloads;
using Neo.Wallets;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class VotingDialog : Form
    {
        private UInt160 script_hash;

        public StateTransaction GetTransaction()
        {
            return Program.CurrentWallet.MakeTransaction(new StateTransaction
            {
                Version = 0,
                Descriptors = new[]
                {
                    new StateDescriptor
                    {
                        Type = StateType.Account,
                        Key = script_hash.ToArray(),
                        Field = "Votes",
                        Value = textBox1.Lines.Select(p => ECPoint.Parse(p, ECCurve.Secp256r1)).ToArray().ToByteArray()
                    }
                }
            });
        }

        public VotingDialog(UInt160 script_hash)
        {
            InitializeComponent();
            this.script_hash = script_hash;
            AccountState account = Blockchain.Singleton.Store.GetAccounts().TryGet(script_hash);
            label1.Text = script_hash.ToAddress();
            textBox1.Lines = account.Votes.Select(p => p.ToString()).ToArray();
        }
    }
}
