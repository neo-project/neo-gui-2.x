using Neo.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class RestoreAccountsDialog : Form
    {
        public RestoreAccountsDialog()
        {
            InitializeComponent();
        }

        public IEnumerable<VerificationContract> GetContracts()
        {
            return listView1.CheckedItems.OfType<ListViewItem>().Select(p => (VerificationContract)p.Tag);
        }

        private void RestoreAccountsDialog_Load(object sender, EventArgs e)
        {
            IEnumerable<KeyPair> keys = Program.CurrentWallet.GetKeys();
            keys = keys.Where(account => Program.CurrentWallet.GetContracts(account.PublicKeyHash).All(contract => !contract.IsStandard));
            IEnumerable<VerificationContract> contracts = keys.Select(p => VerificationContract.CreateSignatureContract(p.PublicKey));
            listView1.Items.AddRange(contracts.Select(p => new ListViewItem(p.Address)
            {
                Tag = p
            }).ToArray());
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            button1.Enabled = listView1.CheckedItems.Count > 0;
        }
    }
}
