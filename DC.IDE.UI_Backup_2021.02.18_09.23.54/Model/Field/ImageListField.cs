using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class ImageListField : FieldItem
    {
        public string AllowType { get; set; }
        public string IsAllowPickFromExists { get; set; }
        public int MaxNum { get; set; }
    }
}
