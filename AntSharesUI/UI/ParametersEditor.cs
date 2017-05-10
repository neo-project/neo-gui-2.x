using AntShares.Core;
using AntShares.Cryptography.ECC;
using AntShares.SmartContract;
using System;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace AntShares.UI
{
    internal partial class ParametersEditor : Form
    {
        public ParametersEditor(ContractParameter[] parameters)
        {
            InitializeComponent();
            listView1.Items.AddRange(parameters.Select((p, i) => new ListViewItem(new[]
            {
                new ListViewItem.ListViewSubItem
                {
                    Name = "index",
                    Text = $"[{i}]"
                },
                new ListViewItem.ListViewSubItem
                {
                    Name = "type",
                    Text = p.Type.ToString()
                },
                new ListViewItem.ListViewSubItem
                {
                    Name = "value",
                    Text = GetValueString(p.Value)
                }
            }, -1)
            {
                Tag = p
            }).ToArray());
        }

        private static string GetValueString(object value)
        {
            if (value == null) return "(null)";
            byte[] array = value as byte[];
            if (array == null)
                return value.ToString();
            else
                return array.ToHexString();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
                textBox1.Text = listView1.SelectedItems[0].SubItems["value"].Text;
            else
                textBox1.Clear();
            textBox2.Clear();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = listView1.SelectedIndices.Count > 0 && textBox2.TextLength > 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0) return;
            ContractParameter parameter = (ContractParameter)listView1.SelectedItems[0].Tag;
            switch (parameter.Type)
            {
                case ContractParameterType.Signature:
                    try
                    {
                        byte[] signature = textBox2.Text.HexToBytes();
                        if (signature.Length != 64) return;
                        parameter.Value = signature;
                    }
                    catch (FormatException)
                    {
                        return;
                    }
                    break;
                case ContractParameterType.Boolean:
                    parameter.Value = string.Equals(textBox2.Text, bool.TrueString, StringComparison.OrdinalIgnoreCase);
                    break;
                case ContractParameterType.Integer:
                    parameter.Value = BigInteger.Parse(textBox2.Text);
                    break;
                case ContractParameterType.Hash160:
                    {
                        UInt160 hash;
                        if (!UInt160.TryParse(textBox2.Text, out hash)) return;
                        parameter.Value = hash;
                    }
                    break;
                case ContractParameterType.Hash256:
                    {
                        UInt256 hash;
                        if (!UInt256.TryParse(textBox2.Text, out hash)) return;
                        parameter.Value = hash;
                    }
                    break;
                case ContractParameterType.ByteArray:
                    try
                    {
                        parameter.Value = textBox2.Text.HexToBytes();
                    }
                    catch (FormatException)
                    {
                        return;
                    }
                    break;
                case ContractParameterType.PublicKey:
                    try
                    {
                        parameter.Value = ECPoint.Parse(textBox2.Text, ECCurve.Secp256r1);
                    }
                    catch (FormatException)
                    {
                        return;
                    }
                    break;
            }
            listView1.SelectedItems[0].SubItems["value"].Text = GetValueString(parameter.Value);
            textBox1.Text = listView1.SelectedItems[0].SubItems["value"].Text;
            textBox2.Clear();
        }
    }
}
