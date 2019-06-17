using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neo.Properties;

namespace Neo.UI
{
    public partial class NetFeeDialog : Form
    {
        Fixed8 SystemFee;
        Fixed8 NetFee;
        Fixed8 PriorityFee;

        public NetFeeDialog(Fixed8 SystemFee, Fixed8 NetFee, Fixed8 PriorityFee)
        {
            this.SystemFee = SystemFee;
            this.NetFee = NetFee;
            this.PriorityFee = PriorityFee;

            InitializeComponent();
            OnResize();
            this.ControlBox = false;
            this.CenterToParent();

            ShowCost(SystemFee + NetFee + PriorityFee);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (IsPriority.Checked)
            {
                ShowCost(SystemFee + NetFee + PriorityFee);
            }
            else
            {
                ShowCost(SystemFee + NetFee);
            }
        }

        private void ShowCost(Fixed8 fee)
        {
            StringBuilder sb = new StringBuilder(32);

            string content = sb.AppendFormat("{0} {1} {2}", fee.ToString(), "Gas", Strings.CostTips).ToString();
            this.CostContext.Text = content;
        }

        private void OnResize()
        {
            int x = (int)(0.5 * (this.Width - this.IsPriority.Width));
            int y = this.IsPriority.Location.Y;
            this.IsPriority.Location = new System.Drawing.Point(x, y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
