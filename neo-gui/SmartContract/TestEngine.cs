using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.Implementations.Blockchains.LevelDB;
using Neo.IO.Caching;
using Neo.VM;

namespace Neo.SmartContract
{
    internal static class TestEngine
    {
        private static readonly LevelDBBlockchain blockchain = (LevelDBBlockchain)Blockchain.Default;

        public static ApplicationEngine Run(IScriptContainer container, byte[] script)
        {
            DataCache<UInt160, AccountState> accounts = blockchain.GetTable<UInt160, AccountState>();
            DataCache<ECPoint, ValidatorState> validators = blockchain.GetTable<ECPoint, ValidatorState>();
            DataCache<UInt256, AssetState> assets = blockchain.GetTable<UInt256, AssetState>();
            DataCache<UInt160, ContractState> contracts = blockchain.GetTable<UInt160, ContractState>();
            DataCache<StorageKey, StorageItem> storages = blockchain.GetTable<StorageKey, StorageItem>();
            CachedScriptTable script_table = new CachedScriptTable(contracts);
            StateMachine service = new StateMachine(accounts, validators, assets, contracts, storages);
            ApplicationEngine engine = new ApplicationEngine(TriggerType.Application, container, script_table, service, Fixed8.Zero, true);
            engine.LoadScript(script, false);
            return engine.Execute() ? engine : null;
        }
    }
}
