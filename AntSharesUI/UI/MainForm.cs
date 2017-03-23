using AntShares.Core;
using AntShares.Cryptography;
using AntShares.Implementations.Blockchains.LevelDB;
using AntShares.Implementations.Wallets.EntityFramework;
using AntShares.IO;
using AntShares.Properties;
using AntShares.VM;
using AntShares.Wallets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AntShares.UI
{
    internal partial class MainForm : Form
    {
        private static readonly UInt160 RecycleScriptHash = new[] { (byte)OpCode.OP_TRUE }.ToScriptHash();
        private bool balance_changed = false;
        private DateTime persistence_time = DateTime.MinValue;

        public MainForm(XDocument xdoc = null)
        {
            InitializeComponent();
            if (xdoc != null)
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                Version latest = Version.Parse(xdoc.Element("update").Attribute("latest").Value);
                if (version < latest)
                {
                    toolStripStatusLabel3.Tag = xdoc;
                    toolStripStatusLabel3.Text += $": {latest}";
                    toolStripStatusLabel3.Visible = true;
                }
            }
        }

        private void AddAddressToListView(UInt160 scriptHash, bool selected = false)
        {
            string address = Wallet.ToAddress(scriptHash);
            ListViewItem item = listView1.Items[address];
            if (item == null)
            {
                ListViewGroup group = listView1.Groups["watchOnlyGroup"];
                item = listView1.Items.Add(new ListViewItem(address, group)
                {
                    Name = address,
                    Tag = scriptHash
                });
            }
            item.Selected = selected;
        }

        private void AddContractToListView(Contract contract, bool selected = false)
        {
            ListViewItem item = listView1.Items[contract.Address];
            if (item?.Tag is UInt160)
            {
                listView1.Items.Remove(item);
                item = null;
            }
            if (item == null)
            {
                ListViewGroup group = contract.IsStandard ? listView1.Groups["standardContractGroup"] : listView1.Groups["nonstandardContractGroup"];
                item = listView1.Items.Add(new ListViewItem(contract.Address, group)
                {
                    Name = contract.Address,
                    Tag = contract
                });
            }
            item.Selected = selected;
        }

        private void Blockchain_PersistCompleted(object sender, Block block)
        {
            persistence_time = DateTime.Now;
            if (Program.CurrentWallet?.GetCoins().Any(p => !p.State.HasFlag(CoinState.Spent) && p.Output.AssetId.Equals(Blockchain.AntShare.Hash)) == true)
                balance_changed = true;
            CurrentWallet_TransactionsChanged(null, Enumerable.Empty<TransactionInfo>());
        }

        private void ChangeWallet(UserWallet wallet)
        {
            if (Program.CurrentWallet != null)
            {
                Program.CurrentWallet.BalanceChanged -= CurrentWallet_BalanceChanged;
                Program.CurrentWallet.TransactionsChanged -= CurrentWallet_TransactionsChanged;
                Program.CurrentWallet.Dispose();
            }
            Program.CurrentWallet = wallet;
            listView3.Items.Clear();
            if (Program.CurrentWallet != null)
            {
                CurrentWallet_TransactionsChanged(null, Program.CurrentWallet.LoadTransactions());
                Program.CurrentWallet.BalanceChanged += CurrentWallet_BalanceChanged;
                Program.CurrentWallet.TransactionsChanged += CurrentWallet_TransactionsChanged;
            }
            修改密码CToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            重建钱包数据库RToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            restoreAccountsToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            交易TToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            提取小蚁币CToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            requestCertificateToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            注册资产RToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            资产分发IToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            选举EToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            创建新地址NToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            导入私钥IToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            创建智能合约SToolStripMenuItem.Enabled = Program.CurrentWallet != null;
            listView1.Items.Clear();
            if (Program.CurrentWallet != null)
            {
                foreach (UInt160 scriptHash in Program.CurrentWallet.GetAddresses().ToArray())
                {
                    Contract contract = Program.CurrentWallet.GetContract(scriptHash);
                    if (contract == null)
                        AddAddressToListView(scriptHash);
                    else
                        AddContractToListView(contract);
                }
            }
            balance_changed = true;
        }

        private void CurrentWallet_BalanceChanged(object sender, EventArgs e)
        {
            balance_changed = true;
        }

        private void CurrentWallet_TransactionsChanged(object sender, IEnumerable<TransactionInfo> transactions)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, IEnumerable<TransactionInfo>>(CurrentWallet_TransactionsChanged), sender, transactions);
            }
            else
            {
                foreach (TransactionInfo info in transactions)
                {
                    string txid = info.Transaction.Hash.ToString();
                    if (listView3.Items.ContainsKey(txid))
                    {
                        listView3.Items[txid].Tag = info;
                    }
                    else
                    {
                        listView3.Items.Insert(0, new ListViewItem(new[]
                        {
                            new ListViewItem.ListViewSubItem
                            {
                                Name = "time",
                                Text = info.Time.ToString()
                            },
                            new ListViewItem.ListViewSubItem
                            {
                                Name = "hash",
                                Text = txid
                            },
                            new ListViewItem.ListViewSubItem
                            {
                                Name = "confirmations",
                                Text = Strings.Unconfirmed
                            },
                            //add transaction type to list by phinx
                            new ListViewItem.ListViewSubItem
                            {
                                Name = "txtype",
                                Text = info.Transaction.Type.ToString()
                            }
                            //end

                        }, -1)
                        {
                            Name = txid,
                            Tag = info
                        });
                    }
                }
                foreach (ListViewItem item in listView3.Items)
                {
                    int? confirmations = (int)Blockchain.Default.Height - (int?)((TransactionInfo)item.Tag).Height + 1;
                    if (confirmations <= 0) confirmations = null;
                    item.SubItems["confirmations"].Text = confirmations?.ToString() ?? Strings.Unconfirmed;
                }
            }
        }

        private void ImportBlocks(Stream stream)
        {
            LevelDBBlockchain blockchain = (LevelDBBlockchain)Blockchain.Default;
            blockchain.VerifyBlocks = false;
            using (BinaryReader r = new BinaryReader(stream))
            {
                uint count = r.ReadUInt32();
                for (int height = 0; height < count; height++)
                {
                    byte[] array = r.ReadBytes(r.ReadInt32());
                    if (height > Blockchain.Default.Height)
                    {
                        Block block = array.AsSerializable<Block>();
                        Blockchain.Default.AddBlock(block);
                    }
                }
            }
            blockchain.VerifyBlocks = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                const string acc_path = "chain.acc";
                const string acc_zip_path = acc_path + ".zip";
                if (File.Exists(acc_path))
                {
                    using (FileStream fs = new FileStream(acc_path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        ImportBlocks(fs);
                    }
                    File.Delete(acc_path);
                }
                else if (File.Exists(acc_zip_path))
                {
                    using (FileStream fs = new FileStream(acc_zip_path, FileMode.Open, FileAccess.Read, FileShare.None))
                    using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Read))
                    using (Stream zs = zip.GetEntry(acc_path).Open())
                    {
                        ImportBlocks(zs);
                    }
                    File.Delete(acc_zip_path);
                }
                Blockchain.PersistCompleted += Blockchain_PersistCompleted;
                Program.LocalNode.Start(Settings.Default.NodePort);
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Blockchain.PersistCompleted -= Blockchain_PersistCompleted;
            ChangeWallet(null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_height.Text = $"{Blockchain.Default.Height}/{Blockchain.Default.HeaderHeight}";
            lbl_count_node.Text = Program.LocalNode.RemoteNodeCount.ToString();
            TimeSpan persistence_span = DateTime.Now - persistence_time;
            if (persistence_span > Blockchain.TimePerBlock)
            {
                toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                toolStripProgressBar1.Value = persistence_span.Seconds;
                toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
            }
            if (Program.CurrentWallet?.WalletHeight > Blockchain.Default.Height + 1)
                return;
            if (balance_changed)
            {
                IEnumerable<Coin> coins = Program.CurrentWallet?.GetCoins().Where(p => !p.State.HasFlag(CoinState.Spent)) ?? Enumerable.Empty<Coin>();
                Fixed8 anc_claim_available = Wallet.CalculateClaimAmount(Program.CurrentWallet.GetUnclaimedCoins().Select(p => p.Reference));
                Fixed8 anc_claim_unavailable = Wallet.CalculateClaimAmountUnavailable(coins.Where(p => p.State.HasFlag(CoinState.Confirmed) && p.Output.AssetId.Equals(Blockchain.AntShare.Hash)).Select(p => p.Reference), Blockchain.Default.Height + 1);
                Fixed8 anc_claim = anc_claim_available + anc_claim_unavailable;
                var assets = coins.GroupBy(p => p.Output.AssetId, (k, g) => new
                {
                    Asset = (RegisterTransaction)Blockchain.Default.GetTransaction(k),
                    Value = g.Sum(p => p.Output.Value),
                    Claim = k.Equals(Blockchain.AntCoin.Hash) ? anc_claim : Fixed8.Zero
                }).ToDictionary(p => p.Asset.Hash);
                if (anc_claim != Fixed8.Zero && !assets.ContainsKey(Blockchain.AntCoin.Hash))
                {
                    assets[Blockchain.AntCoin.Hash] = new
                    {
                        Asset = Blockchain.AntCoin,
                        Value = Fixed8.Zero,
                        Claim = anc_claim
                    };
                }
                foreach (RegisterTransaction tx in listView2.Items.OfType<ListViewItem>().Select(p => (RegisterTransaction)p.Tag).ToArray())
                {
                    if (!assets.ContainsKey(tx.Hash))
                    {
                        listView2.Items.RemoveByKey(tx.Hash.ToString());
                    }
                }
                foreach (var asset in assets.Values)
                {
                    string value_text = asset.Value.ToString() + (asset.Asset.Hash.Equals(Blockchain.AntCoin.Hash) ? $"+({asset.Claim})" : "");
                    if (listView2.Items.ContainsKey(asset.Asset.Hash.ToString()))
                    {
                        listView2.Items[asset.Asset.Hash.ToString()].SubItems["value"].Text = value_text;
                    }
                    else
                    {
                        listView2.Items.Add(new ListViewItem(new[]
                        {
                            new ListViewItem.ListViewSubItem
                            {
                                Name = "name",
                                Text = asset.Asset.GetName()
                            },
                            new ListViewItem.ListViewSubItem
                            {
                                Name = "type",
                                Text = asset.Asset.AssetType.ToString()
                            },
                            new ListViewItem.ListViewSubItem
                            {
                                Name = "value",
                                Text = value_text
                            },
                            new ListViewItem.ListViewSubItem
                            {
                                ForeColor = Color.Gray,
                                Name = "issuer",
                                Text = $"{Strings.UnknownIssuer}[{asset.Asset.Issuer}]"
                            }
                        }, -1, listView2.Groups["unchecked"])
                        {
                            Name = asset.Asset.Hash.ToString(),
                            Tag = asset.Asset,
                            UseItemStyleForSubItems = false
                        });
                    }
                }
                balance_changed = false;
            }
            foreach (ListViewItem item in listView2.Groups["unchecked"].Items.OfType<ListViewItem>().ToArray())
            {
                ListViewItem.ListViewSubItem subitem = item.SubItems["issuer"];
                RegisterTransaction asset = (RegisterTransaction)item.Tag;
                CertificateQueryResult result;
                if (asset.AssetType == AssetType.AntShare || asset.AssetType == AssetType.AntCoin)
                {
                    result = new CertificateQueryResult { Type = CertificateQueryResultType.System };
                }
                else
                {
                    result = CertificateQueryService.Query(asset.Issuer);
                }
                using (result)
                {
                    subitem.Tag = result.Type;
                    switch (result.Type)
                    {
                        case CertificateQueryResultType.Querying:
                        case CertificateQueryResultType.QueryFailed:
                            break;
                        case CertificateQueryResultType.System:
                            subitem.ForeColor = Color.Green;
                            subitem.Text = Strings.SystemIssuer;
                            break;
                        case CertificateQueryResultType.Invalid:
                            subitem.ForeColor = Color.Red;
                            subitem.Text = $"[{Strings.InvalidCertificate}][{asset.Issuer}]";
                            break;
                        case CertificateQueryResultType.Expired:
                            subitem.ForeColor = Color.Yellow;
                            subitem.Text = $"[{Strings.ExpiredCertificate}]{result.Certificate.Subject}[{asset.Issuer}]";
                            break;
                        case CertificateQueryResultType.Good:
                            subitem.ForeColor = Color.Black;
                            subitem.Text = $"{result.Certificate.Subject}[{asset.Issuer}]";
                            break;
                    }
                    switch (result.Type)
                    {
                        case CertificateQueryResultType.System:
                        case CertificateQueryResultType.Missing:
                        case CertificateQueryResultType.Invalid:
                        case CertificateQueryResultType.Expired:
                        case CertificateQueryResultType.Good:
                            item.Group = listView2.Groups["checked"];
                            break;
                    }
                }
            }
        }

        private void 创建钱包数据库NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CreateWalletDialog dialog = new CreateWalletDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                ChangeWallet(UserWallet.Create(dialog.WalletPath, dialog.Password));
                Settings.Default.LastWalletPath = dialog.WalletPath;
                Settings.Default.Save();
            }
        }

        private void 打开钱包数据库OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenWalletDialog dialog = new OpenWalletDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                if (UserWallet.GetVersion(dialog.WalletPath) < Version.Parse("1.3.5"))
                {
                    if (MessageBox.Show(Strings.MigrateWalletMessage, Strings.MigrateWalletCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                        return;
                    string path_old = Path.ChangeExtension(dialog.WalletPath, ".old.db3");
                    string path_new = Path.ChangeExtension(dialog.WalletPath, ".new.db3");
                    UserWallet.Migrate(dialog.WalletPath, path_new);
                    File.Move(dialog.WalletPath, path_old);
                    File.Move(path_new, dialog.WalletPath);
                    MessageBox.Show($"{Strings.MigrateWalletSucceedMessage}\n{path_old}");
                }
                UserWallet wallet;
                try
                {
                    wallet = UserWallet.Open(dialog.WalletPath, dialog.Password);
                }
                catch (CryptographicException)
                {
                    MessageBox.Show(Strings.PasswordIncorrect);
                    return;
                }
                if (dialog.RepairMode) wallet.Rebuild();
                ChangeWallet(wallet);
                Settings.Default.LastWalletPath = dialog.WalletPath;
                Settings.Default.Save();
            }
        }

        private void 修改密码CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ChangePasswordDialog dialog = new ChangePasswordDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                if (Program.CurrentWallet.ChangePassword(dialog.OldPassword, dialog.NewPassword))
                    MessageBox.Show(Strings.ChangePasswordSuccessful);
                else
                    MessageBox.Show(Strings.PasswordIncorrect);
            }
        }

        private void 重建钱包数据库RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            listView3.Items.Clear();
            Program.CurrentWallet.Rebuild();
        }

        private void restoreAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RestoreAccountsDialog dialog = new RestoreAccountsDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                foreach (Contract contract in dialog.GetContracts())
                {
                    Program.CurrentWallet.AddContract(contract);
                    AddContractToListView(contract, true);
                }
            }
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 转账TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (TransferDialog dialog = new TransferDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                Helper.SignAndShowInformation(dialog.GetTransaction());
            }
        }

        private void 交易TToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (TradeForm form = new TradeForm())
            {
                form.ShowDialog();
            }
        }

        private void 签名SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SigningDialog dialog = new SigningDialog())
            {
                dialog.ShowDialog();
            }
        }

        private void 提取小蚁币CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helper.Show<ClaimForm>();
        }

        private void requestCertificateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CertificateRequestWizard wizard = new UI.CertificateRequestWizard())
            {
                wizard.ShowDialog();
            }
        }

        private void 注册资产RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AssetRegisterDialog dialog = new AssetRegisterDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                Helper.SignAndShowInformation(dialog.GetTransaction());
            }
        }

        private void 资产分发IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (IssueDialog dialog = new IssueDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                Helper.SignAndShowInformation(dialog.GetTransaction());
            }
        }

        private void 选举EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ElectionDialog dialog = new ElectionDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                Helper.SignAndShowInformation(dialog.GetTransaction());
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OptionsDialog dialog = new OptionsDialog())
            {
                dialog.ShowDialog();
            }
        }

        private void 官网WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.antshares.org/");
        }

        private void 开发人员工具TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Helper.Show<DeveloperToolsForm>();
        }

        private void 关于AntSharesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"{Strings.AboutMessage} {Strings.AboutVersion}{Assembly.GetExecutingAssembly().GetName().Version}", Strings.About);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            查看私钥VToolStripMenuItem.Enabled =
                listView1.SelectedIndices.Count == 1 &&
                listView1.SelectedItems[0].Tag is Contract &&
                ((Contract)listView1.SelectedItems[0].Tag).IsStandard;
            viewContractToolStripMenuItem.Enabled =
                listView1.SelectedIndices.Count == 1 &&
                listView1.SelectedItems[0].Tag is Contract;
            复制到剪贴板CToolStripMenuItem.Enabled = listView1.SelectedIndices.Count == 1;
            删除DToolStripMenuItem.Enabled = listView1.SelectedIndices.Count > 0;
        }

        private void 创建新地址NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.SelectedIndices.Clear();
            Account account = Program.CurrentWallet.CreateAccount();
            foreach (Contract contract in Program.CurrentWallet.GetContracts(account.PublicKeyHash))
            {
                AddContractToListView(contract, true);
            }
        }

        private void importWIFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ImportPrivateKeyDialog dialog = new ImportPrivateKeyDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                listView1.SelectedIndices.Clear();
                foreach (string wif in dialog.WifStrings)
                {
                    Account account;
                    try
                    {
                        account = Program.CurrentWallet.Import(wif);
                    }
                    catch (FormatException)
                    {
                        continue;
                    }
                    foreach (Contract contract in Program.CurrentWallet.GetContracts(account.PublicKeyHash))
                    {
                        AddContractToListView(contract, true);
                    }
                }
            }
        }

        private void importCertificateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SelectCertificateDialog dialog = new SelectCertificateDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                listView1.SelectedIndices.Clear();
                Account account = Program.CurrentWallet.Import(dialog.SelectedCertificate);
                foreach (Contract contract in Program.CurrentWallet.GetContracts(account.PublicKeyHash))
                {
                    AddContractToListView(contract, true);
                }
            }
        }

        private void importWatchOnlyAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = InputBox.Show(Strings.Address, Strings.ImportWatchOnlyAddress);
            if (string.IsNullOrEmpty(text)) return;
            using (StringReader reader = new StringReader(text))
            {
                while (true)
                {
                    string address = reader.ReadLine();
                    if (address == null) break;
                    address = address.Trim();
                    if (string.IsNullOrEmpty(address)) continue;
                    UInt160 scriptHash;
                    try
                    {
                        scriptHash = Wallet.ToScriptHash(address);
                    }
                    catch (FormatException)
                    {
                        continue;
                    }
                    Program.CurrentWallet.AddWatchOnly(scriptHash);
                    AddAddressToListView(scriptHash, true);
                }
            }
        }

        private void 多方签名MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CreateMultiSigContractDialog dialog = new CreateMultiSigContractDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                Contract contract = dialog.GetContract();
                if (contract == null)
                {
                    MessageBox.Show(Strings.AddContractFailedMessage);
                    return;
                }
                Program.CurrentWallet.AddContract(contract);
                listView1.SelectedIndices.Clear();
                AddContractToListView(contract, true);
            }
        }

        private void 自定义CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ImportCustomContractDialog dialog = new ImportCustomContractDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;
                Contract contract = dialog.GetContract();
                Program.CurrentWallet.AddContract(contract);
                listView1.SelectedIndices.Clear();
                AddContractToListView(contract, true);
            }
        }

        private void 查看私钥VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contract contract = (Contract)listView1.SelectedItems[0].Tag;
            Account account = Program.CurrentWallet.GetAccountByScriptHash(contract.ScriptHash);
            using (ViewPrivateKeyDialog dialog = new ViewPrivateKeyDialog(account, contract.ScriptHash))
            {
                dialog.ShowDialog();
            }
        }

        private void viewContractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contract contract = (Contract)listView1.SelectedItems[0].Tag;
            using (ViewContractDialog dialog = new ViewContractDialog(contract))
            {
                dialog.ShowDialog();
            }
        }

        private void 复制到剪贴板CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(listView1.SelectedItems[0].Text);
            }
            catch (ExternalException) { }
        }

        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Strings.DeleteAddressConfirmationMessage, Strings.DeleteAddressConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            object[] tags = listView1.SelectedItems.OfType<ListViewItem>().Select(p => p.Tag).ToArray();
            foreach (object tag in tags)
            {
                UInt160 scriptHash = (tag as UInt160) ?? ((Contract)tag).ScriptHash;
                listView1.Items.RemoveByKey(Wallet.ToAddress(scriptHash));
                Program.CurrentWallet.DeleteAddress(scriptHash);
            }
            balance_changed = true;
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            viewCertificateToolStripMenuItem.Enabled = listView2.SelectedIndices.Count == 1;
            if (viewCertificateToolStripMenuItem.Enabled)
            {
                CertificateQueryResultType type = (CertificateQueryResultType)listView2.SelectedItems[0].SubItems["issuer"].Tag;
                viewCertificateToolStripMenuItem.Enabled = type == CertificateQueryResultType.Good || type == CertificateQueryResultType.Expired || type == CertificateQueryResultType.Invalid;
            }
            删除DToolStripMenuItem1.Enabled = listView2.SelectedIndices.Count > 0;
            if (删除DToolStripMenuItem1.Enabled)
            {
                删除DToolStripMenuItem1.Enabled = listView2.SelectedItems.OfType<ListViewItem>().Select(p => (RegisterTransaction)p.Tag).All(p => p.AssetType != AssetType.AntShare && p.AssetType != AssetType.AntCoin);
            }
        }

        private void viewCertificateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegisterTransaction asset = (RegisterTransaction)listView2.SelectedItems[0].Tag;
            UInt160 hash = Contract.CreateSignatureRedeemScript(asset.Issuer).ToScriptHash();
            string address = Wallet.ToAddress(hash);
            string path = Path.Combine(Settings.Default.CertCachePath, $"{address}.cer");
            Process.Start(path);
        }

        private void 删除DToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedIndices.Count == 0) return;
            var delete = listView2.SelectedItems.OfType<ListViewItem>().Select(p => (RegisterTransaction)p.Tag).Select(p => new
            {
                Asset = p,
                Value = Program.CurrentWallet.GetAvailable(p.Hash)
            }).ToArray();
            if (MessageBox.Show($"{Strings.DeleteAssetConfirmationMessage}\n"
                + string.Join("\n", delete.Select(p => $"{p.Asset.GetName()}:{p.Value}"))
                , Strings.DeleteConfirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            ContractTransaction tx = Program.CurrentWallet.MakeTransaction(new ContractTransaction
            {
                Outputs = delete.Select(p => new TransactionOutput
                {
                    AssetId = p.Asset.Hash,
                    Value = p.Value,
                    ScriptHash = RecycleScriptHash
                }).ToArray()
            }, Fixed8.Zero);
            Helper.SignAndShowInformation(tx);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0) return;
            Clipboard.SetDataObject(listView3.SelectedItems[0].SubItems[1].Text);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0) return;
            string url = "http://antcha.in/address/info/" + listView1.SelectedItems[0].Text;
            Process.Start(url);
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            if (listView2.SelectedIndices.Count == 0) return;
            string url = "http://antcha.in/tokens/hash/" + listView2.SelectedItems[0].Name;
            Process.Start(url);
        }

        private void listView3_DoubleClick(object sender, EventArgs e)
        {
            if (listView3.SelectedIndices.Count == 0) return;
            string url = "http://antcha.in/tx/hash/" + listView3.SelectedItems[0].Name;
            Process.Start(url);
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {
            using (UpdateDialog dialog = new UpdateDialog((XDocument)toolStripStatusLabel3.Tag))
            {
                dialog.ShowDialog();
            }
        }
    }
}
