using Neo.Properties;
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
    public partial class ExecuteDialog : Form
    {
        public ExecuteDialog()
        {
            InitializeComponent();
            string[] arrScriptHash = Settings.Default.NEP5Watched.OfType<string>().ToArray();
            foreach (string item in arrScriptHash)
            {
                comboBox1.Items.Add(item);
            }
            string[] arrCommand = { "deploy", "mintTokens", "inflation", "inflationRate", "inflationStartTime", "inner" };
            foreach (string item in arrCommand)
            {
                comboBox2.Items.Add(item);
            }
        }

        public void GetCommand(out string scriptHash, out string command)
        {
            
            scriptHash = comboBox1.SelectedItem as string;
            command = comboBox2.SelectedItem as string;
            
        }
    }
}
