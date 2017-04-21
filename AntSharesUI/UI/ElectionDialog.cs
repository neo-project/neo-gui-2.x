using AntShares.Core;
using AntShares.Cryptography.ECC;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AntShares.UI
{
    public partial class ElectionDialog : Form
    {
        public ElectionDialog()
        {
            InitializeComponent();
        }

        public EnrollmentTransaction GetTransaction()
        {
            return Program.CurrentWallet.MakeTransaction(new EnrollmentTransaction
            {
                PublicKey = (ECPoint)comboBox1.SelectedItem,
            }, fee: Fixed8.Zero);
        }

        private void ElectionDialog_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(Program.CurrentWallet.GetContracts().Where(p => p.IsStandard).Select(p => Program.CurrentWallet.GetKey(p.PublicKeyHash).PublicKey).ToArray());
            label4.Text = $"{new EnrollmentTransaction { }.SystemFee } ANC";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = comboBox1.SelectedIndex >= 0;
        }
    }
}
