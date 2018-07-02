using Neo.Wallets;

namespace Neo.UI
{
    internal class TxOutListBoxItem : TransferOutput
    {
        public string AssetName;

        public override string ToString()
        {
            return $"{ScriptHash.ToAddress()}\t{Value}\t{AssetName}";
        }
    }
}
