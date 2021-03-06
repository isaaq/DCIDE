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

using Telerik.Windows.Controls;

namespace DC.IDE.UI.Diagram.Form
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public List<RadDiagramShape> S { get; set; }
        public UserControl1()
        {
            InitializeComponent();
            this.DataContext = this;
            S = new List<RadDiagramShape>();
            S.Add(new RadDiagramShape());
            S.Add(new RadDiagramShape());
            S.Add(new RadDiagramShape());
        }
    }
}
