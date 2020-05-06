using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class NumberField : FieldItem
    {
        public int DigitsNum { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double DefaultValue { get; set; }

    }
}
