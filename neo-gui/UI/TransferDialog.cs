using Neo.Ledger;
using Neo.Network.P2P.Payloads;
using Neo.Properties;
using Neo.Persistence;
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

        public Fixed8 Fee => Fixed8.Parse(textBox1.Text);
        public UInt160 ChangeAddress => ((string)comboBox1.SelectedItem).ToScriptHash();

        public TransferDialog()
        {
            InitializeComponent();
            textBox1.Text = "0";
            comboBox1.Items.AddRange(Program.CurrentWallet.GetAccounts().Select(p => p.Address).ToArray());
            comboBox1.SelectedItem = Program.CurrentWallet.GetChangeAddress().ToAddress();
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
            if (cOutputs.Length == 0)
            {
                tx = new ContractTransaction();
            }
            else
            {
                UInt160[] addresses = Program.CurrentWallet.GetAccounts().Select(p => p.ScriptHash).ToArray();
                HashSet<UInt160> sAttributes = new HashSet<UInt160>();
                using (ScriptBuilder sb = new ScriptBuilder())
                {
                    foreach (var output in cOutputs)
                    {
                        var balances = new List<(UInt160 Account, BigInteger Value)>();

                        using (Snapshot snapshot = Blockchain.Singleton.GetSnapshot())
                        {
                            ContractState asset = snapshot.Contracts.TryGet(output.AssetId);
                            foreach (UInt160 address in addresses)
                            {
                                StorageKey key = new StorageKey
                                {
                                    ScriptHash = asset.ScriptHash,
                                    Key = address.ToArray()
                                };
                                StorageItem item = snapshot.Storages.TryGet(key);
                                balances.Add((address, item == null ? BigInteger.Zero : new BigInteger(item.Value)));
                            }
                        }
                        BigInteger sum = balances.Aggregate(BigInteger.Zero, (x, y) => x + y.Value);

                        if (sum < output.Value) return null;
                        if (sum != output.Value)
                        {
                            balances = balances.OrderByDescending(p => p.Value).ToList();
                            BigInteger amount = output.Value;
                            int i = 0;
                            while (balances[i].Value <= amount)
                                amount -= balances[i++].Value;
                            if (amount == BigInteger.Zero)
                                balances = balances.Take(i).ToList();
                            else
                                balances = balances.Take(i).Concat(new[] { balances.Last(p => p.Value >= amount) }).ToList();
                            sum = balances.Aggregate(BigInteger.Zero, (x, y) => x + y.Value);
                        }
                        sAttributes.UnionWith(balances.Select(p => p.Account));
                        for (int i = 0; i < balances.Count; i++)
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
            if (tx is ContractTransaction ctx)
                tx = Program.CurrentWallet.MakeTransaction(ctx, change_address: ChangeAddress, fee: Fee);
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
            this.Height = 510;
        }
    }
}
