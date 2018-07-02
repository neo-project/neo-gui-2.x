using Akka.Actor;
using Neo.IO.Actors;
using Neo.Ledger;
using Neo.Network.P2P.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class ClaimForm : Form
    {
        private IActorRef actor;

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
                TransactionState tx = Blockchain.Singleton.Snapshot.Transactions.TryGet(group.Key);
                if (tx == null)
                    continue; // not enough of the chain available
                foreach (var reference in group)
                    references.Add(reference);
            }

            textBox2.Text = Blockchain.Singleton.Snapshot.CalculateBonus(references, height).ToString();
        }

        private void ClaimForm_Load(object sender, EventArgs e)
        {
            Fixed8 bonus_available = Blockchain.Singleton.Snapshot.CalculateBonus(Program.CurrentWallet.GetUnclaimedCoins().Select(p => p.Reference));
            textBox1.Text = bonus_available.ToString();
            if (bonus_available == Fixed8.Zero) button1.Enabled = false;
            CalculateBonusUnavailable(Blockchain.Singleton.Snapshot.Height + 1);
            actor = Program.NeoSystem.ActorSystem.ActorOf(EventWrapper<Blockchain.PersistCompleted>.Props(Blockchain_PersistCompleted));
        }

        private void ClaimForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.NeoSystem.ActorSystem.Stop(actor);
        }

        private void Blockchain_PersistCompleted(Blockchain.PersistCompleted e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<Blockchain.PersistCompleted>(Blockchain_PersistCompleted), e);
            }
            else
            {
                CalculateBonusUnavailable(e.Block.Index + 1);
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
                        Value = Blockchain.Singleton.Snapshot.CalculateBonus(claims),
                        ScriptHash = Program.CurrentWallet.GetChangeAddress()
                    }
                }
            });
            Close();
        }
    }
}
