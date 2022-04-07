using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class TextField : FieldItem
    {
        public int InputWidth { get; set; }
        public string DefaultValue { get; set; }
        public bool IsPassword { get; set; }
    }
}
