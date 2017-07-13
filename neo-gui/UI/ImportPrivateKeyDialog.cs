using System;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class ImportPrivateKeyDialog : Form
    {
        public ImportPrivateKeyDialog()
        {
            InitializeComponent();
        }

        public string[] WifStrings
        {
            get
            {
                return textBox1.Lines;
            }
            set
            {
                textBox1.Lines = value;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = textBox1.TextLength > 0;
        }
    }
}
