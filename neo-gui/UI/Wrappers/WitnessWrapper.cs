using Neo.Network.P2P.Payloads;
using System.ComponentModel;
using System.Drawing.Design;

namespace Neo.UI.Wrappers
{
    internal class WitnessWrapper
    {
        [Editor(typeof(ScriptEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(HexConverter))]
        public byte[] InvocationScript { get; set; }
        [Editor(typeof(ScriptEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(HexConverter))]
        public byte[] VerificationScript { get; set; }

        public Witness Unwrap()
        {
            return new Witness
            {
                InvocationScript = InvocationScript,
                VerificationScript = VerificationScript
            };
        }
    }
}
