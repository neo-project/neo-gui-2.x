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
    public partial class WatchDialog : Form
    {
        public WatchDialog()
        {
            InitializeComponent();
            textBox1.Lines = Settings.Default.NEP5Watched.OfType<string>().ToArray();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            Settings.Default.NEP5Watched.Clear();
            Settings.Default.NEP5Watched.AddRange(textBox1.Lines.Where(p => !string.IsNullOrWhiteSpace(p) && UInt160.TryParse(p, out _)).ToArray());
            Settings.Default.Save();
        }
    }
}
