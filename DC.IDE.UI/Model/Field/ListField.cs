using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Model.Field
{
    public class ListField : FieldItem
    {
        IList<FieldItem> Children { get; set; }
    }
}
