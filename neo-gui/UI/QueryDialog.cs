using Neo.Properties;
using Neo.SmartContract;
using Neo.VM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class QueryDialog : Form
    {
        private UInt160 scriptHash;
        private AssetDescriptor asset;

        public QueryDialog()
        {
            InitializeComponent();
            string[] arrScriptHash = Settings.Default.NEP5Watched.OfType<string>().ToArray();
            foreach (string item in arrScriptHash)
            {
                comboBox1.Items.Add(item);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.scriptHash = UInt160.parse(comboBox1.SelectedItem as string);
            this.asset = new AssetDescriptor(this.scriptHash);
        }

        private void Query_Click(object sender, EventArgs e)
        {
            byte[] script;
            using (ScriptBuilder sb = new ScriptBuilder())
            {
                sb.EmitAppCall(scriptHash, "totalSupply");
                sb.EmitAppCall(scriptHash, "queryInflationRate");
                sb.EmitAppCall(scriptHash, "queryInflationStartTime");
                sb.EmitAppCall(scriptHash, "totalIcoNeo");
                sb.EmitAppCall(scriptHash, "icoNeo");
                script = sb.ToArray();
            }
            ApplicationEngine engine = TestEngine.Run(script);
            if (engine != null)
            {
                //icoNeo
                BigInteger _icoNeo = engine.EvaluationStack.Pop().GetBigInteger();
                BigDecimal icoNeo = new BigDecimal(_icoNeo, asset.Precision);
                this.txtbx_icoNeo.Text = icoNeo.ToString();

                //totalIcoNeo
                BigInteger _totalIcoNeo = engine.EvaluationStack.Pop().GetBigInteger();
                BigDecimal totalIcoNeo = new BigDecimal(_totalIcoNeo, asset.Precision);
                this.txtbx_totalIcoNeo.Text = totalIcoNeo.ToString();

                //queryInflationStartTime
                BigInteger startTime = engine.EvaluationStack.Pop().GetBigInteger();
                this.txtbx_infStartTime.Text = ConvertIntDateTime((double)startTime).ToString();

                //queryInflationRate
                BigInteger iRate = engine.EvaluationStack.Pop().GetBigInteger();
                this.txtbx_inflationRate.Text = iRate.ToString();

                //totalSupply
                BigInteger _totalSupply = engine.EvaluationStack.Pop().GetBigInteger();
                BigDecimal totalSupply = new BigDecimal(_totalSupply, asset.Precision);
                this.txtbx_totalSupply.Text = totalSupply.ToString();
            }
            else
            {
                MessageBox.Show("Query Failed");
            }
        }

        public static DateTime ConvertIntDateTime(double d)
        {
            DateTime time = System.DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }

    }
}
