using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Form.Shape
{
    public class FormContainerShape : RadDiagramContainerShape
    {
        public FormContainerShape()
        {

        }
        protected override System.Windows.Rect CalculateContentBounds(System.Windows.Rect newShapeBounds)
        {
            return this.ContentBounds;
        }
        protected override void OnSizeChanged(Size newSize, Size oldSize)
        {
            base.OnSizeChanged(newSize, oldSize);
            InvokeCalcPosition();
        }

        protected override void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var shapelist = (DiagramItemCollection)sender;
            InvokeCalcPosition();
            base.OnItemsCollectionChanged(sender, e);
        }

        private void InvokeCalcPosition()
        {
            Task.Factory.StartNew(() => Thread.Sleep(100))
                                .ContinueWith((t) =>
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        CalcPosition();
                                    });

                                });
        }

        private void CalcPosition()
        {
            var container = this;
            var cwidth = container.Width;
            var cheight = container.Height;

            var cursorx = container.X + 30;
            var cursory = container.Y + 30;

            foreach (var obj in container.Items)
            {
                var shape = (RadDiagramShapeBase)obj;
                var swidth = shape.ActualWidth;
                var sheight = shape.ActualHeight;
                shape.Width = (cwidth - 60 - container.Items.Count % 3 * 10) / 3;
                shape.Position = new Point(cursorx, cursory);
                cursorx += swidth + 10;
                if (cursorx > cwidth - 60)
                {
                    cursorx = container.X + 30;
                    cursory += sheight + 10;
                }
            }
        }
    }
}
