using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class DateTimeField : FieldItem
    {
        public int DateType { get; set; }
        public DateTime DefaultValue { get; set; }
    }
}
