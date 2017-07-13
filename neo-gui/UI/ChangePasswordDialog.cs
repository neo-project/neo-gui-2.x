using System;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class ChangePasswordDialog : Form
    {
        public string OldPassword
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        public string NewPassword
        {
            get
            {
                return textBox2.Text;
            }
            set
            {
                textBox2.Text = value;
                textBox3.Text = value;
            }
        }

        public ChangePasswordDialog()
        {
            InitializeComponent();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = textBox1.TextLength > 0 && textBox2.TextLength > 0 && textBox3.Text == textBox2.Text;
        }
    }
}
