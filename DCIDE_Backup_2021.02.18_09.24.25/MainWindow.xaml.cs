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
using DC.IDE.UI.UC;
using DC.IDE.UI.Util;
using DC.IDE.UI.VM;
using DCIDE.UI.VM;
using Telerik.Windows.Controls;

namespace DCIDE
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new VMMain();
        }

        private void UCFuncPane_OnFuncChange(object sender, RadPane pane)
        {
            var foundPane = dockMain.FindVisualChild<RadPane>(pane.Name);
            if (foundPane == null)
                dockMain.AddItem(pane, Telerik.Windows.Controls.Docking.DockPosition.Center);
            else
                foundPane.IsSelected = true;
        }
    }
}
