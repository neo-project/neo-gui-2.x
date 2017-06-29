using AntShares.Core;
using AntShares.IO.Caching;
using AntShares.VM;

namespace AntShares.SmartContract
{
    internal class CachedScriptTable : IScriptTable
    {
        private DataCache<UInt160, ContractState> contracts;

        public CachedScriptTable(DataCache<UInt160, ContractState> contracts)
        {
            this.contracts = contracts;
        }

        byte[] IScriptTable.GetScript(byte[] script_hash)
        {
            return contracts[new UInt160(script_hash)].Code.Script;
        }
    }
}
