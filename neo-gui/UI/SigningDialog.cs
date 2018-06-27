using System;
using System.Text;
using System.Windows.Forms;
using Neo.Properties;

namespace Neo.UI
{
    internal partial class SigningDialog : Form
    {
        public SigningDialog()
        {
            InitializeComponent();

            cmbFormat.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show(Strings.SigningFailedNoDataMessage);
                return;
            }

            byte[] raw, signedData = null;
            try
            {
                switch (cmbFormat.SelectedIndex)
                {
                    case 0: raw = Encoding.UTF8.GetBytes(textBox1.Text); break;
                    case 1: raw = textBox1.Text.HexToBytes(); break;
                    default: return;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Program.CurrentWallet.Sign(raw, out signedData))
            {
                MessageBox.Show(Strings.SigningFailedKeyNotFoundMessage);
                return;
            }

            textBox2.Text = signedData.ToHexString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.SelectAll();
            textBox2.Copy();
        }
    }
}
