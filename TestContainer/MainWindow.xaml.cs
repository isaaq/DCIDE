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
using Telerik.Windows.Controls;

namespace TestContainer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
                        
        }

        private void RadTreeView_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var item = e.OriginalSource as RadTreeViewItem;
            var uid = item.Uid;
            if(uid != "")
            {

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var uc = new UCModelField(new DC.IDE.UI.Model.StructItem() { ID = "5ea00e678b26d60db109009d".ToObjectId() });
            grid.Children.Add(uc);
        }
    }
}
