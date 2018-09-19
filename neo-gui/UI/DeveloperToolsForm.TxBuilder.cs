using Neo.Network.P2P.Payloads;
using Neo.Properties;
using Neo.SmartContract;
using Neo.UI.Wrappers;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Neo.UI
{
    partial class DeveloperToolsForm
    {
        private void InitializeTxBuilder()
        {
            comboBox1.Items.AddRange(new object[]
            {
                TransactionType.ContractTransaction,
                TransactionType.ClaimTransaction,
                TransactionType.IssueTransaction,
                TransactionType.InvocationTransaction,
                TransactionType.StateTransaction,
            });
            button6.Enabled = Program.CurrentWallet != null;
            button7.Enabled = Program.CurrentWallet != null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is null) return;
            string typeName = $"{typeof(TransactionWrapper).Namespace}.{comboBox1.SelectedItem}Wrapper";
            propertyGrid1.SelectedObject = Assembly.GetExecutingAssembly().CreateInstance(typeName);
        }

        private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Enabled = propertyGrid1.SelectedObject != null;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TransactionWrapper tx = (TransactionWrapper)propertyGrid1.SelectedObject;
            TransactionAttributeWrapper attribute = tx.Attributes.FirstOrDefault(p => p.Usage == TransactionAttributeUsage.Remark);
            bool found = attribute != null;
            if (!found)
            {
                attribute = new TransactionAttributeWrapper
                {
                    Usage = TransactionAttributeUsage.Remark,
                    Data = new byte[0]
                };
            }
            string remark = Encoding.UTF8.GetString(attribute.Data);
            remark = InputBox.Show(Strings.EnterRemarkMessage, Strings.EnterRemarkTitle, remark);
            if (remark != null)
            {
                attribute.Data = Encoding.UTF8.GetBytes(remark);
                if (!found) tx.Attributes.Add(attribute);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (PayToDialog dialog = new PayToDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                TxOutListBoxItem output = dialog.GetOutput();
                TransactionWrapper tx = (TransactionWrapper)propertyGrid1.SelectedObject;
                tx.Outputs.Add(new TransactionOutputWrapper
                {
                    AssetId = (UInt256)output.AssetId,
                    Value = output.Value.ToFixed8(),
                    ScriptHash = output.ScriptHash
                });
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TransactionWrapper wrapper = (TransactionWrapper)propertyGrid1.SelectedObject;
            Transaction tx = Program.CurrentWallet.MakeTransaction(wrapper.Unwrap());
            if (tx == null)
            {
                MessageBox.Show(Strings.InsufficientFunds);
            }
            else
            {
                wrapper.Inputs = tx.Inputs.Select(p => CoinReferenceWrapper.Wrap(p)).ToList();
                wrapper.Outputs = tx.Outputs.Select(p => TransactionOutputWrapper.Wrap(p)).ToList();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            TransactionWrapper wrapper = (TransactionWrapper)propertyGrid1.SelectedObject;
            ContractParametersContext context = new ContractParametersContext(wrapper.Unwrap());
            InformationBox.Show(context.ToString(), "ParametersContext", "ParametersContext");
        }
    }
}
