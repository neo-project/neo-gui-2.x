using Neo.Network.P2P.Payloads;
using Neo.Properties;
using Neo.SmartContract;
using Neo.VM;
using Neo.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Forms;
using VMArray = Neo.VM.Types.Array;

namespace Neo.UI
{
    public partial class TransferDialog : Form
    {
        private string remark = "";

        public Fixed8 Fee => Fixed8.Parse(textBoxFee.Text);
        public UInt160 ChangeAddress => ((string)comboBoxChangeAddress.SelectedItem).ToScriptHash();
        public UInt160 FromAddress;

        public TransferDialog()
        {
            InitializeComponent();
            textBoxFee.Text = "0";
            comboBoxChangeAddress.Items.AddRange(Program.CurrentWallet.GetAccounts().Where(p => !p.WatchOnly).Select(p => p.Address).ToArray());
            comboBoxChangeAddress.SelectedItem = Program.CurrentWallet.GetChangeAddress().ToAddress();
            comboBoxFrom.Items.AddRange(Program.CurrentWallet.GetAccounts().Where(p => !p.WatchOnly).Select(p => p.Address).ToArray());
        }

        public Transaction GetTransaction()
        {
            var cOutputs = txOutListBox1.Items.Where(p => p.AssetId is UInt160).GroupBy(p => new
            {
                AssetId = (UInt160)p.AssetId,
                Account = p.ScriptHash
            }, (k, g) => new
            {
                k.AssetId,
                Value = g.Aggregate(BigInteger.Zero, (x, y) => x + y.Value.Value),
                k.Account
            }).ToArray();
            Transaction tx;
            List<TransactionAttribute> attributes = new List<TransactionAttribute>();

            if (comboBoxFrom.SelectedItem == null)
            {
                FromAddress = null;
            }
            else
            {
                FromAddress = ((string)comboBoxFrom.SelectedItem).ToScriptHash();
            }

            if (cOutputs.Length == 0)
            {
                tx = new ContractTransaction();
            }
            else
            {
                UInt160[] addresses;
                if (FromAddress != null)
                {
                    addresses = Program.CurrentWallet.GetAccounts().Where(e => e.ScriptHash.Equals(FromAddress)).Select(p => p.ScriptHash).ToArray();
                }
                else
                {
                    addresses = Program.CurrentWallet.GetAccounts().Where(e => !e.WatchOnly).Select(p => p.ScriptHash).ToArray();
                }
                HashSet<UInt160> sAttributes = new HashSet<UInt160>();
                using (ScriptBuilder sb = new ScriptBuilder())
                {
                    foreach (var output in cOutputs)
                    {
                        byte[] script;
                        using (ScriptBuilder sb2 = new ScriptBuilder())
                        {

                            foreach (UInt160 address in addresses)
                            {
                                sb2.EmitAppCall(output.AssetId, "balanceOf", address);
                            }

                            sb2.Emit(OpCode.DEPTH, OpCode.PACK);
                            script = sb2.ToArray();
                        }
                        using (ApplicationEngine engine = ApplicationEngine.Run(script))
                        {
                            if (engine.State.HasFlag(VMState.FAULT)) return null;
                            var balances = ((VMArray)engine.ResultStack.Pop()).AsEnumerable().Reverse().Zip(addresses, (i, a) => new
                            {
                                Account = a,
                                Value = i.GetBigInteger()
                            }).Where(p => p.Value != 0).ToArray();

                            BigInteger sum = balances.Aggregate(BigInteger.Zero, (x, y) => x + y.Value);
                            if (sum < output.Value) return null;
                            if (sum != output.Value)
                            {
                                balances = balances.OrderByDescending(p => p.Value).ToArray();
                                BigInteger amount = output.Value;
                                int i = 0;
                                while (balances[i].Value <= amount)
                                    amount -= balances[i++].Value;
                                if (amount == BigInteger.Zero)
                                    balances = balances.Take(i).ToArray();
                                else
                                    balances = balances.Take(i).Concat(new[] { balances.Last(p => p.Value >= amount) }).ToArray();
                                sum = balances.Aggregate(BigInteger.Zero, (x, y) => x + y.Value);
                            }
                            sAttributes.UnionWith(balances.Select(p => p.Account));
                            for (int i = 0; i < balances.Length; i++)
                            {
                                BigInteger value = balances[i].Value;
                                if (i == 0)
                                {
                                    BigInteger change = sum - output.Value;
                                    if (change > 0) value -= change;
                                }
                                sb.EmitAppCall(output.AssetId, "transfer", balances[i].Account, output.Account, value);
                                sb.Emit(OpCode.THROWIFNOT);
                            }
                        }
                    }
                    tx = new InvocationTransaction
                    {
                        Version = 1,
                        Script = sb.ToArray()
                    };
                }
                attributes.AddRange(sAttributes.Select(p => new TransactionAttribute
                {
                    Usage = TransactionAttributeUsage.Script,
                    Data = p.ToArray()
                }));
            }
            if (!string.IsNullOrEmpty(remark))
                attributes.Add(new TransactionAttribute
                {
                    Usage = TransactionAttributeUsage.Remark,
                    Data = Encoding.UTF8.GetBytes(remark)
                });
            tx.Attributes = attributes.ToArray();
            tx.Outputs = txOutListBox1.Items.Where(p => p.AssetId is UInt256).Select(p => p.ToTxOutput()).ToArray();
            var tempOuts = tx.Outputs;
            if (tx is ContractTransaction copyTx)
            {
                copyTx.Witnesses = new Witness[0];
                copyTx = Program.CurrentWallet.MakeTransaction(copyTx, FromAddress, change_address: ChangeAddress, fee: Fee);
                if (copyTx == null) return null;
                ContractParametersContext transContext = new ContractParametersContext(copyTx);
                Program.CurrentWallet.Sign(transContext);
                if (transContext.Completed)
                {
                    copyTx.Witnesses = transContext.GetWitnesses();
                }
                if (copyTx.Size > 1024)
                {
                    Fixed8 PriorityFee = Fixed8.FromDecimal(0.001m) + Fixed8.FromDecimal(copyTx.Size * 0.00001m);
                    if (Fee > PriorityFee) PriorityFee = Fee;
                    if (!Helper.CostRemind(Fixed8.Zero, PriorityFee)) return null;
                    tx = Program.CurrentWallet.MakeTransaction(new ContractTransaction
                    {
                        Outputs = tempOuts,
                        Attributes = tx.Attributes
                    }, FromAddress, change_address: ChangeAddress, fee: PriorityFee);
                }
            }
            return tx;
        }

        private void txOutListBox1_ItemsChanged(object sender, EventArgs e)
        {
            button3.Enabled = txOutListBox1.ItemCount > 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            remark = InputBox.Show(Strings.EnterRemarkMessage, Strings.EnterRemarkTitle, remark);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            groupBox1.Visible = true;
            this.Height = 570;
        }
    }
}
