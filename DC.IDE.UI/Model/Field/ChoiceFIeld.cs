using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Field
{
    public class ChoiceField : FieldItem
    {
        public int ChoiceType { get; set; }
        public int ColumnWidth { get; set; }
        public string DefaultValue { get; set; }
        public int IsClauseField { get; set; }
        public string ChoiceContent { get; set; }

    }
}
