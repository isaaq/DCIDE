using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class MapField : FieldItem
    {
        public int MapType { get; set; }
        public string ApiKey { get; set; }
        public string DefaultPosition { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
    }
}
