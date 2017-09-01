using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace Neo.UI.Wrappers
{
    internal class HexEditor : UITypeEditor
    {
        private readonly MultilineStringEditor ed = new MultilineStringEditor();

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            byte[] array = value as byte[];
            string s = (string)ed.EditValue(context, provider, array?.ToHexString());
            value = s.HexToBytes();
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return ed.GetEditStyle();
        }

        public override bool IsDropDownResizable
        {
            get
            {
                return ed.IsDropDownResizable;
            }
        }
    }
}
