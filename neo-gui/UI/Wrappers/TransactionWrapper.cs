using Neo.Network.P2P.Payloads;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Neo.UI.Wrappers
{
    internal abstract class TransactionWrapper
    {
        [Category("Basic")]
        public byte Version { get; set; }
        [Category("Basic")]
        public List<TransactionAttributeWrapper> Attributes { get; set; } = new List<TransactionAttributeWrapper>();
        [Category("Basic")]
        public List<CoinReferenceWrapper> Inputs { get; set; } = new List<CoinReferenceWrapper>();
        [Category("Basic")]
        public List<TransactionOutputWrapper> Outputs { get; set; } = new List<TransactionOutputWrapper>();
        [Category("Basic")]
        public List<WitnessWrapper> Witnesses { get; set; } = new List<WitnessWrapper>();

        public virtual Transaction Unwrap()
        {
            string typeName = GetType().Name;
            typeName = typeName.Substring(0, typeName.Length - 7);
            typeName = $"{typeof(Transaction).Namespace}.{typeName}";
            Transaction tx = (Transaction)typeof(Transaction).Assembly.CreateInstance(typeName);
            tx.Version = Version;
            tx.Attributes = Attributes.Select(p => p.Unwrap()).ToArray();
            tx.Inputs = Inputs.Select(p => p.Unwrap()).ToArray();
            tx.Outputs = Outputs.Select(p => p.Unwrap()).ToArray();
            tx.Witnesses = Witnesses.Select(p => p.Unwrap()).ToArray();
            return tx;
        }
    }
}
