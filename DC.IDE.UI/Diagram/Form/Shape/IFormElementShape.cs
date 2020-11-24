using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telerik.Windows.Controls;

namespace DC.IDE.UI.Diagram.Form.Shape
{
    interface IFormElementShape
    {
        RadDiagramShape CreateShape();
    }
}
