using AntShares.Core;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AntShares.UI
{
    internal partial class DeployContractDialog : Form
    {
        public DeployContractDialog()
        {
            InitializeComponent();
            label9.Text = $"{new PublishTransaction().SystemFee} ANC";
        }

        public PublishTransaction GetTransaction()
        {
            return Program.CurrentWallet.MakeTransaction(new PublishTransaction
            {
                Version = checkBox1.Checked ? (byte)1 : (byte)0,
                Code = new FunctionCode
                {
                    Script = textBox8.Text.HexToBytes(),
                    ParameterList = textBox6.Text.HexToBytes().Select(p => (ContractParameterType)p).ToArray(),
                    ReturnType = textBox7.Text.HexToBytes().Select(p => (ContractParameterType?)p).FirstOrDefault() ?? ContractParameterType.Void
                },
                NeedStorage = checkBox1.Checked,
                Name = textBox1.Text,
                CodeVersion = textBox2.Text,
                Author = textBox3.Text,
                Email = textBox4.Text,
                Description = textBox5.Text
            });
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = textBox1.TextLength > 0
                && textBox2.TextLength > 0
                && textBox3.TextLength > 0
                && textBox4.TextLength > 0
                && textBox5.TextLength > 0
                && textBox8.TextLength > 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            textBox8.Text = File.ReadAllBytes(openFileDialog1.FileName).ToHexString();
        }
    }
}
