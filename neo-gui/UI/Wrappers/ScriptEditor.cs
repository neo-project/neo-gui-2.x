using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Neo.UI.Wrappers
{
    internal class ScriptEditor : FileNameEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string path = (string)base.EditValue(context, provider, null);
            if (path == null) return null;
            return File.ReadAllBytes(path);
        }

        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            openFileDialog.DefaultExt = "avm";
            openFileDialog.Filter = "NeoContract|*.avm";
        }
    }
}
