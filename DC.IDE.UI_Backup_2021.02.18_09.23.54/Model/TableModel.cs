using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model
{
    public class FieldModel
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public int Length { get; set; }
        public string Default { get; set; }
        public bool Nullable { get; set; }
        public bool IsPK { get; set; }
    }

    public class TableModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<FieldModel> Fields { get; set; }
    }
}
