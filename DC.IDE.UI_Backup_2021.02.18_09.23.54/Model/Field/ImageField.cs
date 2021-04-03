using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class ImageField : FieldItem
    {
        public int InputWidth { get; set; }
        public string DefaultValue { get; set; }
        public int MaxSize { get; set; }
        public bool IsWatermark { get; set; }
        public string AllowedTypes { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

    }
}
