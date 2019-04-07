using Akka.Actor;
using Neo.Ledger;
using Neo.Network.P2P.Payloads;
using Neo.Properties;
using Neo.SmartContract;
using Neo.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    partial class DeveloperToolsForm
    {
        private ContractParametersContext context;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return;
            listBox2.Items.Clear();
            if (Program.CurrentWallet == null) return;
            UInt160 hash = ((string)listBox1.SelectedItem).ToScriptHash();
            var parameters = context.GetParameters(hash);
            if (parameters == null)
            {
                var parameterList = Program.CurrentWallet.GetAccount(hash).Contract.ParameterList ?? Blockchain.Singleton.Store.GetContracts()[hash].ParameterList;
                if (parameterList != null)
                {
                    var pList = new List<ContractParameter>();
                    for (int i = 0; i < parameterList.Length; i++)
                    {
                        pList.Add(new ContractParameter(parameterList[i]));
                        context.Add(Program.CurrentWallet.GetAccount(hash).Contract, i, null);
                    }
                }
            }
            listBox2.Items.AddRange(context.GetParameters(hash).ToArray());
            button4.Visible = context.Completed;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex < 0) return;
            textBox1.Text = listBox2.SelectedItem.ToString();
            textBox2.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = InputBox.Show("ParametersContext", "ParametersContext");
            if (string.IsNullOrEmpty(input)) return;
            try
            {
                context = ContractParametersContext.Parse(input);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            textBox1.Clear();
            textBox2.Clear();
            listBox1.Items.AddRange(context.ScriptHashes.Select(p => p.ToAddress()).ToArray());
            button2.Enabled = true;
            button4.Visible = context.Completed;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InformationBox.Show(context.ToString(), "ParametersContext", "ParametersContext");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return;
            if (listBox2.SelectedIndex < 0) return;
            ContractParameter parameter = (ContractParameter)listBox2.SelectedItem;
            parameter.SetValue(textBox2.Text);
            listBox2.Items[listBox2.SelectedIndex] = parameter;
            textBox1.Text = textBox2.Text;
            button4.Visible = context.Completed;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!(context.Verifiable is Transaction tx))
            {
                MessageBox.Show("Only support to broadcast transaction.");
                return;
            }
            tx.Witnesses = context.GetWitnesses();
            RelayResultReason reason = Program.NeoSystem.Blockchain.Ask<RelayResultReason>(tx).Result;
            if (reason == RelayResultReason.Succeed)
            {
                InformationBox.Show(tx.Hash.ToString(), Strings.RelaySuccessText, Strings.RelaySuccessTitle);
            }
            else
            {
                MessageBox.Show($"Transaction cannot be broadcast: {reason}");
            }
        }
    }
}
