using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class LinkageMenuField : FieldItem
    {
        public int MenuId { get; set; }
        public int DisplayType { get; set; }
        public string PathSplitter { get; set; }
        public bool IsClauseField { get; set; }
    }
}
