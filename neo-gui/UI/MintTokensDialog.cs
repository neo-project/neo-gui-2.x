using Neo.Core;
using Neo.VM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class MintTokensDialog : Form
    {
        public MintTokensDialog()
        {
            InitializeComponent();
            var unspentCoins = Program.CurrentWallet.FindUnspentCoins().Select(p => p.Output.AssetId).Distinct();
            foreach (UInt256 asset_id in unspentCoins)
            {
                AssetState state = Blockchain.Default.GetAssetState(asset_id);
                if (state.AssetType == AssetType.GoverningToken)
                {
                    assetComboBox.Items.Add(state);
                    break;
                }                
            }
            if (assetComboBox.Items.Count > 0)
            {
                assetComboBox.SelectedIndex = 0;
            }
        }

        private void assetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssetState asset = assetComboBox.SelectedItem as AssetState;
            this.currentBalanceTextBox.Text = asset == null ? "" : Program.CurrentWallet.GetAvailable(asset.AssetId).ToString();            
        }

        public InvocationTransaction GetMintTransaction()
        {
            string contractCommand = "mintTokens";
            UInt160 script_hash = UInt160.Parse(this.contractScriptHashTextBox.Text);
            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitPush(0);
                sb.EmitPush(contractCommand);
                sb.EmitAppCall(script_hash.ToArray());
                script = sb.ToArray();
            }

            AssetState asset = (AssetState)this.assetComboBox.SelectedItem;
            TransactionOutput[] outputs = new TransactionOutput[] {
                new TransactionOutput
                {
                    AssetId = asset.AssetId,
                    ScriptHash = UInt160.Parse(this.contractScriptHashTextBox.Text),
                    Value = Fixed8.Parse(this.amountTextBox.Text)
                }
            };

            return Program.CurrentWallet.MakeTransaction(new InvocationTransaction
            {
                Outputs = outputs,
                Script = script
            });
        }
    }
}
