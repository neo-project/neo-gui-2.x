using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms.Design;

namespace Neo.UI.Wrappers
{
    internal class ScriptEditor : FileNameEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string path = (string)base.EditValue(context, provider, null);
            return File.ReadAllBytes(path);
        }
    }
}
