using System.Windows.Forms;

namespace AntShares.UI
{
    internal partial class InputBox : Form
    {
        private InputBox(string text, string caption)
        {
            InitializeComponent();
            this.Text = caption;
            groupBox1.Text = text;
        }

        public static string Show(string text, string caption)
        {
            using (InputBox dialog = new InputBox(text, caption))
            {
                if (dialog.ShowDialog() != DialogResult.OK) return null;
                return dialog.textBox1.Text;
            }
        }
    }
}
