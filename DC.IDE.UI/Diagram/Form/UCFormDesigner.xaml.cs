using System;
using System.Collections.Generic;
using System.Linq;
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

using DC.IDE.UI.Diagram.Form.VM;
using DC.IDE.UI.Model;

namespace DC.IDE.UI.Diagram.Form
{
    /// <summary>
    /// UCFormDesigner.xaml 的交互逻辑
    /// </summary>
    public partial class UCFormDesigner : UserControl
    {
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
          "Item",
          typeof(StructItem),
          typeof(UCFormDesigner),
          new PropertyMetadata(new StructItem())
        );

        public StructItem Item
        {
            get { return (StructItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public UCFormDesigner()
        {
            InitializeComponent();
            this.DataContext = new VMFormDiagram(Item);
            //diagram.GraphSource = new VMFormDiagramGraphSource();
        }
    }
}
