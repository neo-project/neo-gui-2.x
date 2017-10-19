using System.Numerics;

namespace Neo
{
    internal struct BigDecimal
    {
        private readonly BigInteger value;
        private readonly byte decimals;

        public BigInteger Value => value;
        public byte Decimals => decimals;

        public BigDecimal(BigInteger value, byte decimals)
        {
            this.value = value;
            this.decimals = decimals;
        }

        public override string ToString()
        {
            BigInteger divisor = BigInteger.Pow(10, decimals);
            BigInteger result = BigInteger.DivRem(value, divisor, out BigInteger remainder);
            if (remainder == 0) return result.ToString();
            return $"{result}.{remainder.ToString("d" + decimals)}".TrimEnd('0');
        }
    }
}
