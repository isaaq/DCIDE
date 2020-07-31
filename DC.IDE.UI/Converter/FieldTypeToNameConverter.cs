using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.IDE.UI.Model.Field;

namespace DC.IDE.UI.Converter
{
    public class FieldTypeToNameConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {

            var ft = Enum.Parse(typeof(FieldType), value.ToString());
            return ft;

            //return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
