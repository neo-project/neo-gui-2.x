using Neo.Core;

namespace Neo.SmartContract
{
    internal class ContractParameter
    {
        public ContractParameterType Type;
        public object Value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
