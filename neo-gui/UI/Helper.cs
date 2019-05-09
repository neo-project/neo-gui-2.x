using Akka.Actor;
using Neo.Network.P2P;
using Neo.Network.P2P.Payloads;
using Neo.Properties;
using Neo.SmartContract;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace Neo.UI
{
    internal static class Helper
    {
        private static Dictionary<Type, Form> tool_forms = new Dictionary<Type, Form>();

        private static void Helper_FormClosing(object sender, FormClosingEventArgs e)
        {
            tool_forms.Remove(sender.GetType());
        }

        public static void Show<T>() where T : Form, new()
        {
            Type t = typeof(T);
            if (!tool_forms.ContainsKey(t))
            {
                tool_forms.Add(t, new T());
                tool_forms[t].FormClosing += Helper_FormClosing;
            }
            tool_forms[t].Show();
            tool_forms[t].Activate();
        }

        public static void SignAndShowInformation(Transaction tx)
        {
            if (tx == null)
            {
                MessageBox.Show(Strings.InsufficientFunds);
                return;
            }
            ContractParametersContext context;
            try
            {
                context = new ContractParametersContext(tx);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(Strings.UnsynchronizedBlock);
                return;
            }
            Program.CurrentWallet.Sign(context);
            if (context.Completed)
            {
                tx.Witnesses = context.GetWitnesses();
                Program.CurrentWallet.ApplyTransaction(tx);
                Program.NeoSystem.LocalNode.Tell(new LocalNode.Relay { Inventory = tx });
                InformationBox.Show(tx.Hash.ToString(), Strings.SendTxSucceedMessage, Strings.SendTxSucceedTitle);
            }
            else
            {
                InformationBox.Show(context.ToString(), Strings.IncompletedSignatureMessage, Strings.IncompletedSignatureTitle);
            }
        }

        public static bool CostRemind(Fixed8 fee)
        {
            StringBuilder sb = new StringBuilder(32);

            string content = sb.AppendFormat("{0} {1}",fee.ToString(),Strings.CostTips).ToString();
            string BoxTilte = Strings.CostTitle.ToString();

            DialogResult dr = MessageBox.Show(content, BoxTilte, MessageBoxButtons.OKCancel);

            if (dr == DialogResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static InvocationTransaction DecorateTransaction(InvocationTransaction tx)
        {
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            if (tx.Inputs == null) tx.Inputs = new CoinReference[0];
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Witnesses == null) tx.Witnesses = new Witness[0];

            Fixed8 fee = Fixed8.FromDecimal(0.001m);

            if (tx.Size > 1024)
            {
                fee += Fixed8.FromDecimal(tx.Size * 0.00001m);
            }
            if (!CostRemind(fee))
            {
                return null;
            }            
            InvocationTransaction result = Program.CurrentWallet.MakeTransaction(new InvocationTransaction
            {
                Version = tx.Version,
                Script = tx.Script,
                Gas = tx.Gas,
                Attributes = tx.Attributes,
                Inputs = tx.Inputs,
                Outputs = tx.Outputs
            }, fee: fee);

            return result;
        }

        public static IssueTransaction DecorateTransaction(IssueTransaction tx)
        {
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            if (tx.Inputs == null) tx.Inputs = new CoinReference[0];
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Witnesses == null) tx.Witnesses = new Witness[0];
            Fixed8 fee = Fixed8.FromDecimal(0.001m);

            if (tx.Size > 1024)
            {
                fee += Fixed8.FromDecimal(tx.Size * 0.00001m);
            }
            if (!CostRemind(fee))
            {
                return null;
            }
            IssueTransaction result = Program.CurrentWallet.MakeTransaction(new IssueTransaction
            {
                Version = tx.Version,
                Attributes = tx.Attributes,
                Inputs = tx.Inputs,
                Outputs = tx.Outputs
            }, fee: fee);

            return result;
        }

        public static StateTransaction DecorateTransaction(StateTransaction tx)
        {
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            if (tx.Inputs == null) tx.Inputs = new CoinReference[0];
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Witnesses == null) tx.Witnesses = new Witness[0];
            Fixed8 fee = Fixed8.FromDecimal(0.001m);

            if (tx.Size > 1024)
            {
                fee += Fixed8.FromDecimal(tx.Size * 0.00001m);
            }
            if (!CostRemind(fee))
            {
                return null;
            }

            StateTransaction result = Program.CurrentWallet.MakeTransaction(new StateTransaction
            {
                Version = tx.Version,
                Attributes = tx.Attributes,
                Inputs = tx.Inputs,
                Outputs = tx.Outputs,
                Descriptors = tx.Descriptors
            }, fee: fee);

            return result;
        }

        public static ContractTransaction DecorateTransaction(ContractTransaction tx)
        {
            if (tx.Attributes == null) tx.Attributes = new TransactionAttribute[0];
            if (tx.Inputs == null) tx.Inputs = new CoinReference[0];
            if (tx.Outputs == null) tx.Outputs = new TransactionOutput[0];
            if (tx.Witnesses == null) tx.Witnesses = new Witness[0];
            Fixed8 fee = Fixed8.FromDecimal(0.001m);

            if (tx.Size > 1024)
            {
                fee += Fixed8.FromDecimal(tx.Size * 0.00001m);
            }
            if (!CostRemind(fee))
            {
                return null;
            }

            ContractTransaction result = Program.CurrentWallet.MakeTransaction(new ContractTransaction
            {
                Version = tx.Version,
                Attributes = tx.Attributes,
                Inputs = tx.Inputs,
                Outputs = tx.Outputs
            }, fee:fee);

            return result;
        }
    }
}
