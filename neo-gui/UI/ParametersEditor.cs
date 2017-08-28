using Neo.Core;
using Neo.Cryptography.ECC;
using Neo.SmartContract;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Forms;

namespace Neo.UI
{
    internal partial class ParametersEditor : Form
    {
        private IList<ContractParameter> parameters;

        public ParametersEditor(IList<ContractParameter> parameters)
        {
            InitializeComponent();
            this.parameters = parameters;
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
            panel1.Enabled = !parameters.IsReadOnly;
        }

        private static string GetValueString(object value)
        {
            switch (value)
            {
                case null:
                    return "(null)";
                case byte[] data:
                    return data.ToHexString();
                case UIntBase data:
                    return $"0x{data}";
                case IList<ContractParameter> data:
                    StringBuilder sb = new StringBuilder();
                    sb.Append('[');
                    foreach (ContractParameter item in data)
                    {
                        sb.Append(GetValueString(item.Value));
                        sb.Append(", ");
                    }
                    if (data.Count > 0)
                        sb.Length -= 2;
                    sb.Append(']');
                    return sb.ToString();
                default:
                    return value.ToString();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                textBox1.Text = listView1.SelectedItems[0].SubItems["value"].Text;
                textBox2.Enabled = ((ContractParameter)listView1.SelectedItems[0].Tag).Type != ContractParameterType.Array;
                button2.Enabled = !textBox2.Enabled;
                button4.Enabled = true;
            }
            else
            {
                textBox1.Clear();
                textBox2.Enabled = true;
                button2.Enabled = false;
                button4.Enabled = false;
            }
            textBox2.Clear();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = listView1.SelectedIndices.Count > 0 && textBox2.TextLength > 0;
            button3.Enabled = textBox2.TextLength > 0;
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
                case ContractParameterType.String:
                    parameter.Value = textBox2.Text;
                    break;
            }
            listView1.SelectedItems[0].SubItems["value"].Text = GetValueString(parameter.Value);
            textBox1.Text = listView1.SelectedItems[0].SubItems["value"].Text;
            textBox2.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0) return;
            ContractParameter parameter = (ContractParameter)listView1.SelectedItems[0].Tag;
            using (ParametersEditor dialog = new ParametersEditor((IList<ContractParameter>)parameter.Value))
            {
                dialog.ShowDialog();
                listView1.SelectedItems[0].SubItems["value"].Text = GetValueString(parameter.Value);
                textBox1.Text = listView1.SelectedItems[0].SubItems["value"].Text;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string s = textBox2.Text;
            ContractParameter parameter = new ContractParameter();
            if (string.Equals(s, "true", StringComparison.OrdinalIgnoreCase))
            {
                parameter.Type = ContractParameterType.Boolean;
                parameter.Value = true;
            }
            else if (string.Equals(s, "false", StringComparison.OrdinalIgnoreCase))
            {
                parameter.Type = ContractParameterType.Boolean;
                parameter.Value = false;
            }
            else if (long.TryParse(s, out long num))
            {
                parameter.Type = ContractParameterType.Integer;
                parameter.Value = num;
            }
            else if (s.StartsWith("0x"))
            {
                if (UInt160.TryParse(s, out UInt160 i160))
                {
                    parameter.Type = ContractParameterType.Hash160;
                    parameter.Value = i160;
                }
                else if (UInt256.TryParse(s, out UInt256 i256))
                {
                    parameter.Type = ContractParameterType.Hash256;
                    parameter.Value = i256;
                }
                else if (BigInteger.TryParse(s.Substring(2), NumberStyles.AllowHexSpecifier, null, out BigInteger bi))
                {
                    parameter.Type = ContractParameterType.Integer;
                    parameter.Value = bi;
                }
                else
                {
                    parameter.Type = ContractParameterType.String;
                    parameter.Value = s;
                }
            }
            else if (ECPoint.TryParse(s, ECCurve.Secp256r1, out ECPoint point))
            {
                parameter.Type = ContractParameterType.PublicKey;
                parameter.Value = point;
            }
            else
            {
                try
                {
                    parameter.Value = s.HexToBytes();
                    parameter.Type = ContractParameterType.ByteArray;
                }
                catch (FormatException)
                {
                    parameter.Type = ContractParameterType.String;
                    parameter.Value = s;
                }
            }
            parameters.Add(parameter);
            listView1.Items.Add(new ListViewItem(new[]
            {
                new ListViewItem.ListViewSubItem
                {
                    Name = "index",
                    Text = $"[{listView1.Items.Count}]"
                },
                new ListViewItem.ListViewSubItem
                {
                    Name = "type",
                    Text = parameter.Type.ToString()
                },
                new ListViewItem.ListViewSubItem
                {
                    Name = "value",
                    Text = GetValueString(parameter.Value)
                }
            }, -1)
            {
                Tag = parameter
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int index = listView1.SelectedIndices[0];
            parameters.RemoveAt(index);
            listView1.Items.RemoveAt(index);
        }
    }
}
