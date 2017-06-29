using AntShares.Core;
using AntShares.Cryptography.ECC;
using AntShares.VM;
using AntShares.Wallets;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AntShares.UI
{
    public partial class AssetRegisterDialog : Form
    {
        public AssetRegisterDialog()
        {
            InitializeComponent();
        }

        public InvocationTransaction GetTransaction()
        {
            AssetType asset_type = (AssetType)comboBox1.SelectedItem;
            string name = string.IsNullOrWhiteSpace(textBox1.Text) ? string.Empty : $"[{{\"lang\":\"{CultureInfo.CurrentCulture.Name}\",\"name\":\"{textBox1.Text}\"}}]";
            Fixed8 amount = checkBox1.Checked ? Fixed8.Parse(textBox2.Text) : -Fixed8.Satoshi;
            byte precision = (byte)numericUpDown1.Value;
            ECPoint owner = (ECPoint)comboBox2.SelectedItem;
            UInt160 admin = Wallet.ToScriptHash(comboBox3.Text);
            UInt160 issuer = Wallet.ToScriptHash(comboBox4.Text);
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitPush(issuer.ToArray());
                sb.EmitPush(admin.ToArray());
                sb.EmitPush(owner.EncodePoint(true));
                sb.EmitPush(precision);
                sb.EmitPush(amount.GetData());
                sb.EmitPush(Encoding.UTF8.GetBytes(name));
                sb.EmitPush((byte)asset_type);
                sb.EmitSysCall("AntShares.Asset.Create");
                return new InvocationTransaction
                {
                    Attributes = new[]
                    {
                        new TransactionAttribute
                        {
                            Usage = TransactionAttributeUsage.Script,
                            Data = Contract.CreateSignatureRedeemScript(owner).ToScriptHash().ToArray()
                        }
                    },
                    Script = sb.ToArray()
                };
            }
        }

        private void AssetRegisterDialog_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(new object[] { AssetType.Share, AssetType.Token });
            comboBox2.Items.AddRange(Program.CurrentWallet.GetContracts().Where(p => p.IsStandard).Select(p => Program.CurrentWallet.GetKey(p.PublicKeyHash).PublicKey).ToArray());
            comboBox3.Items.AddRange(Program.CurrentWallet.GetContracts().Select(p => p.Address).ToArray());
            comboBox4.Items.AddRange(Program.CurrentWallet.GetContracts().Select(p => p.Address).ToArray());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = (AssetType)comboBox1.SelectedItem != AssetType.Share;
            if (!numericUpDown1.Enabled) numericUpDown1.Value = 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = checkBox1.Checked;
        }
    }
}
