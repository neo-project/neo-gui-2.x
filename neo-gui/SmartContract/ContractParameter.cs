using Neo.Core;
using Neo.Cryptography.ECC;

namespace Neo.SmartContract
{
    internal class ContractParameter
    {
        public ContractParameterType Type;
        public object Value;

        public ContractParameter() { }

        public ContractParameter(ContractParameterType type)
        {
            this.Type = type;
            switch (type)
            {
                case ContractParameterType.Signature:
                    this.Value = new byte[64];
                    break;
                case ContractParameterType.Boolean:
                    this.Value = false;
                    break;
                case ContractParameterType.Integer:
                    this.Value = 0;
                    break;
                case ContractParameterType.Hash160:
                    this.Value = new UInt160();
                    break;
                case ContractParameterType.Hash256:
                    this.Value = new UInt256();
                    break;
                case ContractParameterType.ByteArray:
                    this.Value = new byte[0];
                    break;
                case ContractParameterType.PublicKey:
                    this.Value = ECCurve.Secp256r1.G;
                    break;
                case ContractParameterType.String:
                    this.Value = "";
                    break;
                case ContractParameterType.Array:
                    this.Value = new ContractParameter[0];
                    break;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
