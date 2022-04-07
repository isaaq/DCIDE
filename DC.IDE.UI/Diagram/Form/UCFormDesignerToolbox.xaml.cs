using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DC.IDE.UI.Diagram.Form.Shape;

using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;
using Telerik.Windows.DragDrop;

namespace DC.IDE.UI.Diagram.Form
{
    /// <summary>
    /// UCFormDesignerToolbox.xaml 的交互逻辑
    /// </summary>
    public partial class UCFormDesignerToolbox : UserControl
    {
        public UCFormDesignerToolbox()
        {
            InitializeComponent();
            DragDropManager.AddDragInitializeHandler(toolbox, OnDragInitialize);
        }

        private DiagramDropInfo GetFormElement(Size dropSize, TextBlock tb)
        {
            var tag = tb.Tag.ToString();
            var text = tb.ToolTip;
            // TODO 不应该放这儿
            var dispatcher = new string[30];
            dispatcher[18] = "CreateContainerShape";
            var ele = dispatcher[int.Parse(tag)];
            RadDiagramShapeBase shape = new FormElementShape().CreateShape();
            if (ele != null)
            {

                //Activator.CreateInstance(Assembly.GetAssembly(typeof(UCFormDesignerToolbox)).FullName, "DC.IDE.UI.Diagram.Form.Shape." + dispatcher[int.Parse(tag)]);
                var method = typeof(FormShapeCreator).GetMethod(ele, BindingFlags.Public | BindingFlags.Static);
                var ret = method.Invoke(null, new object[] { "分组框", null });

                if (ret is RadDiagramShapeBase dsb)
                {
                    shape = dsb;
                }
            } else {
                shape.Content = text;
            }
            var newshape = new RadDiagramShapeBase[1] { shape };
            var diagramDropInfo = new DiagramDropInfo(dropSize, SerializationService.Default.SerializeItems(newshape));

            return diagramDropInfo;
        }

        private void OnDragInitialize(object sender, DragInitializeEventArgs e)
        {
            var old = e.OriginalSource as RadListBoxItem;
            if (old != null)
            {
                var content = old.Content as TextBlock;
                if (content == null)
                    return;
                var dropSize = new Size(content.ActualWidth, content.ActualHeight);
                var dropInfo = GetFormElement(dropSize, content);
                e.Data = dropInfo;
                e.DragVisualOffset = new Point(e.RelativeStartPoint.X - dropSize.Width * 2.0, e.RelativeStartPoint.Y - dropSize.Height * 2.0);

                var area = (int)content.ActualWidth * (int)content.ActualHeight;
                var pixels = new byte[area * 4];
                var bitmapSource = BitmapSource.Create((int)content.ActualWidth, (int)content.ActualHeight, 96, 96, PixelFormats.Pbgra32, null, pixels, (int)content.ActualWidth * 4);

                var visual = new DrawingVisual();
                using (DrawingContext drawingContext = visual.RenderOpen())
                {
                    drawingContext.DrawImage(bitmapSource, new Rect(0, 0, content.ActualWidth, content.ActualHeight));
                    drawingContext.DrawText(
                        new FormattedText(content.Text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                            content.FontFamily.GetTypefaces().ToArray()[0], 32, Brushes.Black, 1.25), new Point(0, 0));
                }
                var image = new DrawingImage(visual.Drawing);
                var newimg = new Image();
                newimg.Width = dropSize.Width * 4;
                newimg.Height = dropSize.Height * 4;
                newimg.Source = image;
                e.DragVisual = (object)newimg;


                e.AllowedEffects = DragDropEffects.All;
                e.Handled = true;
            }
            else
                e.Cancel = true;
        }

    }
}
