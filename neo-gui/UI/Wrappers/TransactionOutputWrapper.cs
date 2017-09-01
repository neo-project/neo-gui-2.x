using Neo.Core;
using System.ComponentModel;

namespace Neo.UI.Wrappers
{
    internal class TransactionOutputWrapper
    {
        [TypeConverter(typeof(UIntBaseConverter))]
        public UInt256 AssetId { get; set; }
        [TypeConverter(typeof(Fixed8Converter))]
        public Fixed8 Value { get; set; }
        [TypeConverter(typeof(UIntBaseConverter))]
        public UInt160 ScriptHash { get; set; }

        public TransactionOutput Unwrap()
        {
            return new TransactionOutput
            {
                AssetId = AssetId,
                Value = Value,
                ScriptHash = ScriptHash
            };
        }

        public static TransactionOutputWrapper Wrap(TransactionOutput output)
        {
            return new TransactionOutputWrapper
            {
                AssetId = output.AssetId,
                Value = output.Value,
                ScriptHash = output.ScriptHash
            };
        }
    }
}
