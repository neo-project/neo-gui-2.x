using AntShares.Core;
using System;
using System.Linq;
using AntShares.Wallets;
using AntShares.Network;

namespace AntShares.Shell
{

    public class Coins
    {
        private Wallet current_wallet;
        private LocalNode local_node;

        public Coins(Wallet wallet, LocalNode node)
        {
            current_wallet = wallet;
            local_node = node;
        }

        public Fixed8 UnavailableBonus()
        {
            uint height = Blockchain.Default.Height + 1;
            Fixed8 unavailable;

            try
            {
                unavailable = Blockchain.CalculateBonus(current_wallet.FindUnspentCoins().Where(p => p.Output.AssetId.Equals(Blockchain.SystemShare.Hash)).Select(p => p.Reference), height);
            }
            catch (Exception)
            {
                unavailable = Fixed8.Zero;
            }

            return unavailable;
        }


        public Fixed8 AvailableBonus()
        {
            return Blockchain.CalculateBonus(current_wallet.GetUnclaimedCoins().Select(p => p.Reference));
        }


        public bool Claim()
        {

            if (this.AvailableBonus() == Fixed8.Zero)
            {
                Console.WriteLine($"no coins to claim");
                return true;
            }

            CoinReference[] claims = current_wallet.GetUnclaimedCoins().Select(p => p.Reference).ToArray();
            if (claims.Length == 0) return false;


            ClaimTransaction tx = new ClaimTransaction
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
                        ScriptHash = current_wallet.GetChangeAddress()
                    }
                }

            };

            Console.WriteLine($"Will sign tx: {tx}");
            bool result = SignTransaction(tx);

            return result;
        }


        private bool SignTransaction(Transaction tx)
        {
            if (tx == null)
            {
                Console.WriteLine($"no transaction specified");
                return false;
            }
            SignatureContext context;

            try
            {
                context = new SignatureContext(tx);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"unsynchronized block");

                return false;
            }

            current_wallet.Sign(context);

            if (context.Completed)
            {
                context.Verifiable.Scripts = context.GetScripts();
                current_wallet.SaveTransaction(tx);

                bool relay_result = local_node.Relay(tx);

                if( relay_result ) 
                {
                    Console.WriteLine($"Transaction Suceeded: {tx.Hash.ToString()}");
					return true;
				} else 
                {
					Console.WriteLine($"Local Node could not relay transaction: {tx.Hash.ToString()}");
				}
            }
            else
            {
                Console.WriteLine($"Incomplete Signature: {context.ToString()}");
            }

            return false;
        }
    }
}