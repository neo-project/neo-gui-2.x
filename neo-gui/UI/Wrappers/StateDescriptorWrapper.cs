using Neo.Network.P2P.Payloads;
using System.ComponentModel;

namespace Neo.UI.Wrappers
{
    class StateDescriptorWrapper
    {
        public StateType Type { get; set; }
        [TypeConverter(typeof(HexConverter))]
        public byte[] Key { get; set; }
        public string Field { get; set; }
        [TypeConverter(typeof(HexConverter))]
        public byte[] Value { get; set; }

        public StateDescriptor Unwrap()
        {
            return new StateDescriptor
            {
                Type = Type,
                Key = Key,
                Field = Field,
                Value = Value
            };
        }
    }
}
