using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telerik.Windows.Controls;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Form.Shape
{
    public static class FormShapeCreator
    {
        public static IContainerShape CreateContainerShape(string title = null, string name = null)
        {
            if (string.IsNullOrEmpty(title)) title = "Container";
            var Rand = new Random();
            if (string.IsNullOrEmpty(name)) name = "Container_" + Rand.Next(665686356);
            var shape = new FormContainerShape
            {
                Width = 250,
                Height = 150,
                Name = name,
                Content = title,
                //Stroke = Brushes.DimGray,
                IsCollapsible = true,
                StrokeThickness = 1,

                CollapsedContent = null,
            };
            return shape;
        }
    }
}
