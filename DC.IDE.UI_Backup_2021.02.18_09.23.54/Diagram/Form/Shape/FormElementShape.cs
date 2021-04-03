using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;

namespace DC.IDE.UI.Diagram.Form.Shape
{
    public class FormElementShape : RadDiagramShape, IElementShape
    {
        public RadDiagramShape CreateShape()
        {
            var radDiagramShape = new RadDiagramShape();
            radDiagramShape.Geometry = ShapeFactory.GetShapeGeometry(CommonShapeType.RectangleShape);
            radDiagramShape.BorderThickness = new Thickness(0.7);
            return radDiagramShape;
        }

        public string Header { get; set; }
    }
}
