using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.Implementations.Blockchains.LevelDB;
using Neo.IO.Caching;
using Neo.VM;
using System;

namespace Neo.SmartContract
{
    internal static class TestEngine
    {
        private static readonly LevelDBBlockchain blockchain = (LevelDBBlockchain)Blockchain.Default;

        public static StackItem Call(UInt160 scriptHash, string operation, params object[] args)
        {
            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                script = sb.EmitAppCall(scriptHash, operation, args).ToArray();
            }
            ApplicationEngine engine = Run(script);
            if (engine == null) throw new Exception();
            return engine.EvaluationStack.Pop();
        }

        public static ApplicationEngine Run(byte[] script, IScriptContainer container = null)
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
