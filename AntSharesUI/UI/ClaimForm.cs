using AntShares.Core;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AntShares.UI
{
    public partial class ClaimForm : Form
    {
        public ClaimForm()
        {
            InitializeComponent();
        }

        private void CalculateBonusUnavailable(uint height)
        {
            textBox2.Text = Blockchain.CalculateBonus(Program.CurrentWallet.FindUnspentCoins().Where(p => p.Output.AssetId.Equals(Blockchain.SystemShare.Hash)).Select(p => p.Reference), height).ToString();
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
                        AssetId = Blockchain.SystemCoin.Hash,
                        Value = Blockchain.CalculateBonus(claims),
                        ScriptHash = Program.CurrentWallet.GetChangeAddress()
                    }
                }
            });
            Close();
        }
    }
}
