using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.Implementations.Blockchains.LevelDB;
using Neo.IO.Caching;
using Neo.Properties;
using Neo.SmartContract;
using Neo.VM;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class InvokeContractDialog : Form
    {
        private InvocationTransaction tx;
        private UInt160 script_hash;
        private ContractParameter[] parameters;

        public InvokeContractDialog(InvocationTransaction tx = null)
        {
            InitializeComponent();
            this.tx = tx;
            if (tx != null)
            {
                radioButton2.Checked = true;
                textBox6.Text = tx.Script.ToHexString();
            }
        }

        public InvocationTransaction GetTransaction()
        {
            return Program.CurrentWallet.MakeTransaction(new InvocationTransaction
            {
                Version = tx.Version,
                Script = tx.Script,
                Gas = tx.Gas,
                Attributes = tx.Attributes,
                Inputs = tx.Inputs,
                Outputs = tx.Outputs
            });
        }

        private void UpdateScript()
        {
            if (parameters.Any(p => p.Value == null)) return;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                foreach (ContractParameter parameter in parameters.Reverse())
                {
                    switch (parameter.Type)
                    {
                        case ContractParameterType.Signature:
                        case ContractParameterType.ByteArray:
                            sb.EmitPush((byte[])parameter.Value);
                            break;
                        case ContractParameterType.Boolean:
                            sb.EmitPush((bool)parameter.Value);
                            break;
                        case ContractParameterType.Integer:
                            sb.EmitPush((BigInteger)parameter.Value);
                            break;
                        case ContractParameterType.Hash160:
                            sb.EmitPush(((UInt160)parameter.Value).ToArray());
                            break;
                        case ContractParameterType.Hash256:
                            sb.EmitPush(((UInt256)parameter.Value).ToArray());
                            break;
                        case ContractParameterType.PublicKey:
                            sb.EmitPush(((ECPoint)parameter.Value).EncodePoint(true));
                            break;
                    }
                }
                sb.EmitAppCall(script_hash.ToArray(), true);
                textBox6.Text = sb.ToArray().ToHexString();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = radioButton1.Checked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UInt160 ignore;
            button1.Enabled = UInt160.TryParse(textBox1.Text, out ignore);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            script_hash = UInt160.Parse(textBox1.Text);
            ContractState contract = Blockchain.Default.GetContract(script_hash);
            if (contract == null) return;
            parameters = contract.Code.ParameterList.Select(p => new ContractParameter { Type = p }).ToArray();
            textBox2.Text = contract.Name;
            textBox3.Text = contract.CodeVersion;
            textBox4.Text = contract.Author;
            textBox5.Text = string.Join(", ", contract.Code.ParameterList);
            button2.Enabled = parameters.Length > 0;
            UpdateScript();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (ParametersEditor dialog = new ParametersEditor(parameters))
            {
                dialog.ShowDialog();
            }
            UpdateScript();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Enabled = radioButton2.Checked;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button5.Enabled = textBox6.TextLength > 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (tx == null) tx = new InvocationTransaction();
            tx.Version = 1;
            tx.Script = textBox6.Text.HexToBytes();
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            if (tx.Inputs == null) tx.Inputs = new CoinReference[0];
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Scripts == null) tx.Scripts = new Witness[0];
            LevelDBBlockchain blockchain = (LevelDBBlockchain)Blockchain.Default;
            DataCache<UInt160, AccountState> accounts = blockchain.GetTable<UInt160, AccountState>();
            DataCache<ECPoint, ValidatorState> validators = blockchain.GetTable<ECPoint, ValidatorState>();
            DataCache<UInt256, AssetState> assets = blockchain.GetTable<UInt256, AssetState>();
            DataCache<UInt160, ContractState> contracts = blockchain.GetTable<UInt160, ContractState>();
            DataCache<StorageKey, StorageItem> storages = blockchain.GetTable<StorageKey, StorageItem>();
            CachedScriptTable script_table = new CachedScriptTable(contracts);
            StateMachine service = new StateMachine(accounts, validators, assets, contracts, storages);
            ApplicationEngine engine = new ApplicationEngine(tx, script_table, service, Fixed8.Zero, true);
            engine.LoadScript(tx.Script, false);
            if (engine.Execute())
            {
                tx.Gas = engine.GasConsumed - Fixed8.FromDecimal(10);
                if (tx.Gas < Fixed8.One) tx.Gas = Fixed8.One;
                tx.Gas = tx.Gas.Ceiling();
                label7.Text = tx.Gas + " gas";
                button3.Enabled = true;
            }
            else
            {
                MessageBox.Show(Strings.ExecutionFailed);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            textBox6.Text = File.ReadAllBytes(openFileDialog1.FileName).ToHexString();
        }
    }
}
