using AntShares.Core;
using AntShares.Network;
using AntShares.Wallets;
using System;
using System.IO;
using System.Linq;

namespace AntShares.Consensus
{
    internal class ConsensusWithPolicy : ConsensusService
    {
        private string log_dictionary;

        public ConsensusWithPolicy(LocalNode localNode, Wallet wallet, string log_dictionary)
            : base(localNode, wallet)
        {
            this.log_dictionary = log_dictionary;
        }

        protected override bool CheckPolicy(Transaction tx)
        {
            switch (Policy.Default.PolicyLevel)
            {
                case PolicyLevel.AllowAll:
                    return true;
                case PolicyLevel.AllowList:
                    return tx.Scripts.All(p => Policy.Default.List.Contains(p.VerificationScript.ToScriptHash())) || tx.Outputs.All(p => Policy.Default.List.Contains(p.ScriptHash));
                case PolicyLevel.DenyList:
                    return tx.Scripts.All(p => !Policy.Default.List.Contains(p.VerificationScript.ToScriptHash())) && tx.Outputs.All(p => !Policy.Default.List.Contains(p.ScriptHash));
                default:
                    return base.CheckPolicy(tx);
            }
        }

        protected override void Log(string message)
        {
            DateTime now = DateTime.Now;
            string line = $"[{now.TimeOfDay:hh\\:mm\\:ss}] {message}";
            Console.WriteLine(line);
            if (string.IsNullOrEmpty(log_dictionary)) return;
            lock (log_dictionary)
            {
                Directory.CreateDirectory(log_dictionary);
                string path = Path.Combine(log_dictionary, $"{now:yyyy-MM-dd}.log");
                File.AppendAllLines(path, new[] { line });
            }
        }

        public void RefreshPolicy()
        {
            Policy.Default.Refresh();
        }
    }
}
