using AntShares.Core;
using AntShares.Properties;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AntShares.UI
{
    public partial class TransferDialog : Form
    {
        private string remark = "";

        public TransferDialog()
        {
            InitializeComponent();
        }

        public ContractTransaction GetTransaction()
        {
            return Program.CurrentWallet.MakeTransaction(new ContractTransaction
            {
                Attributes = string.IsNullOrEmpty(remark) ? new TransactionAttribute[0] : new[]
                {
                    new TransactionAttribute
                    {
                        Usage = TransactionAttributeUsage.Remark,
                        Data = Encoding.UTF8.GetBytes(remark)
                    }
                },
                Outputs = txOutListBox1.Items.GroupBy(p => new { p.Output.AssetId, p.Output.ScriptHash }).Select(g => new TransactionOutput
                {
                    AssetId = g.Key.AssetId,
                    Value = g.Sum(p => p.Output.Value),
                    ScriptHash = g.Key.ScriptHash
                }).ToArray()
            }, Fixed8.Zero);
        }

        private void txOutListBox1_ItemsChanged(object sender, EventArgs e)
        {
            button3.Enabled = txOutListBox1.ItemCount > 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            remark = InputBox.Show(Strings.EnterRemarkMessage, Strings.EnterRemarkTitle, remark);
        }
    }
}
