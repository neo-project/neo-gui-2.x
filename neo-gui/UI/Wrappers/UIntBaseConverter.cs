using System;
using System.ComponentModel;
using System.Globalization;

namespace Neo.UI.Wrappers
{
    internal class UIntBaseConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s)
                return context.PropertyDescriptor.PropertyType.GetMethod("Parse").Invoke(null, new[] { s });
            throw new NotSupportedException();
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
                throw new NotSupportedException();
            UIntBase i = value as UIntBase;
            if (i == null) return null;
            return i.ToString();
        }
    }
}
