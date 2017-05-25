using AntShares.Core;
using AntShares.VM;
using AntShares.Wallets;
using System.Linq;
using System.Windows.Forms;

namespace AntShares.UI
{
    internal partial class VotingDialog : Form
    {
        private UInt160 script_hash;

        public InvocationTransaction GetTransaction()
        {
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                foreach (string line in textBox1.Lines.Reverse())
                    sb.EmitPush(line.HexToBytes());
                sb.EmitPush(textBox1.Lines.Length);
                sb.Emit(OpCode.PACK);
                sb.EmitPush(script_hash.ToArray());
                sb.EmitSysCall("AntShares.Blockchain.GetAccount");
                sb.EmitSysCall("AntShares.Account.SetVotes");
                return Program.CurrentWallet.MakeTransaction(new InvocationTransaction
                {
                    Version = 1,
                    Script = sb.ToArray(),
                    Gas = Fixed8.Zero,
                    Attributes = new[]
                    {
                        new TransactionAttribute
                        {
                            Usage = TransactionAttributeUsage.Script,
                            Data = script_hash.ToArray()
                        }
                    }
                }, fee: Fixed8.One);
            }
        }

        public VotingDialog(UInt160 script_hash)
        {
            InitializeComponent();
            this.script_hash = script_hash;
            AccountState account = Blockchain.Default.GetAccountState(script_hash);
            label1.Text = Wallet.ToAddress(script_hash);
            textBox1.Lines = account.Votes.Select(p => p.ToString()).ToArray();
        }
    }
}
