using CERTENROLLLib;
using Neo.SmartContract;
using Neo.Wallets;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class CertificateRequestWizard : Form
    {
        public CertificateRequestWizard()
        {
            InitializeComponent();
        }

        private void CertificateRequestWizard_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(Program.CurrentWallet.GetAccounts().Where(p => !p.WatchOnly && p.Contract.Script.IsStandardContract()).Select(p => p.GetKey()).ToArray());
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = comboBox1.SelectedIndex >= 0 && groupBox1.Controls.OfType<TextBox>().All(p => p.TextLength > 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            const int ECDSA_PRIVATE_P256_MAGIC = 0x32534345;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            KeyPair key = (KeyPair)comboBox1.SelectedItem;
            byte[] pubkey = key.PublicKey.EncodePoint(false).Skip(1).ToArray();
            byte[] prikey = BitConverter.GetBytes(ECDSA_PRIVATE_P256_MAGIC).Concat(BitConverter.GetBytes(32)).Concat(pubkey).Concat(key.PrivateKey).ToArray();
            CX509PrivateKey x509key = new CX509PrivateKey();
            x509key.AlgorithmName = "ECDSA_P256";
            x509key.Import("ECCPRIVATEBLOB", Convert.ToBase64String(prikey));
            Array.Clear(prikey, 0, prikey.Length);
            CX509CertificateRequestPkcs10 request = new CX509CertificateRequestPkcs10();
            request.InitializeFromPrivateKey(X509CertificateEnrollmentContext.ContextUser, x509key, null);
            request.Subject = new CX500DistinguishedName();
            request.Subject.Encode($"CN={textBox1.Text},C={textBox2.Text},S={textBox3.Text},SERIALNUMBER={textBox4.Text}");
            request.Encode();
            File.WriteAllText(saveFileDialog1.FileName, "-----BEGIN NEW CERTIFICATE REQUEST-----\r\n" + request.RawData + "-----END NEW CERTIFICATE REQUEST-----\r\n");
            Close();
        }
    }
}
