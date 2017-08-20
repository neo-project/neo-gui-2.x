using Neo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class ClaimForm : Form
    {
        public ClaimForm()
        {
            InitializeComponent();
        }

        private void CalculateBonusUnavailable(uint height)
        {
            var unspent = Program.CurrentWallet.FindUnspentCoins()
                .Where(p => p.Output.AssetId.Equals(Blockchain.GoverningToken.Hash))
                .Select(p => p.Reference)
                ;

            ICollection<CoinReference> references = new HashSet<CoinReference>();

            foreach (var group in unspent.GroupBy(p => p.PrevHash))
            {
                int height_start;
                Transaction tx = Blockchain.Default.GetTransaction(group.Key, out height_start);
                if (tx == null)
                    continue; // not enough of the chain available
                foreach (var reference in group)
                    references.Add(reference);
            }

            textBox2.Text = Blockchain.CalculateBonus(references, height).ToString();
        }

        private void ClaimForm_Load(object sender, EventArgs e)
        {
            Fixed8 bonus_available = Blockchain.CalculateBonus(Program.CurrentWallet.GetUnclaimedCoins().Select(p => p.Reference));
            textBox1.Text = bonus_available.ToString();
            if (bonus_available == Fixed8.Zero) button1.Enabled = false;
            CalculateBonusUnavailable(Blockchain.Default.Height + 1);
            Blockchain.PersistCompleted += Blockchain_PersistCompleted;
        }

        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Blockchain.PersistCompleted -= Blockchain_PersistCompleted;
        }

        private void Blockchain_PersistCompleted(object sender, Block block)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, Block>(Blockchain_PersistCompleted), sender, block);
            }
            else
            {
                CalculateBonusUnavailable(block.Index + 1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CoinReference[] claims = Program.CurrentWallet.GetUnclaimedCoins().Select(p => p.Reference).ToArray();
            if (claims.Length == 0) return;
            Helper.SignAndShowInformation(new ClaimTransaction
            {
                Claims = claims,
                Attributes = new TransactionAttribute[0],
                Inputs = new CoinReference[0],
                Outputs = new[]
                {
                    new TransactionOutput
                    {
                        AssetId = Blockchain.UtilityToken.Hash,
                        Value = Blockchain.CalculateBonus(claims),
                        ScriptHash = Program.CurrentWallet.GetChangeAddress()
                    }
                }
            });
            Close();
        }
    }
}
