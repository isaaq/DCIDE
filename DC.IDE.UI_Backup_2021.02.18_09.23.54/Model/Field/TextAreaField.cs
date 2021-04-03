using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class TextAreaField : FieldItem
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string DefaultValue { get; set; }
        public bool IsAllowHTML { get; set; }

    }
}
