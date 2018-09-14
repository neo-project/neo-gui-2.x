using Neo.Network.P2P.Payloads;
using System.Collections.Generic;
using System.Linq;

namespace Neo.UI.Wrappers
{
    internal class ClaimTransactionWrapper : TransactionWrapper
    {
        public List<CoinReferenceWrapper> Claims { get; set; } = new List<CoinReferenceWrapper>();

        public override Transaction Unwrap()
        {
            ClaimTransaction tx = (ClaimTransaction)base.Unwrap();
            tx.Claims = Claims.Select(p => p.Unwrap()).ToArray();
            return tx;
        }
    }
}
