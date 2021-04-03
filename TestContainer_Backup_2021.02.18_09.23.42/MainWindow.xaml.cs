using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using DC.IDE.UI.Model;
using DC.IDE.UI.UC;
using DC.IDE.UI.UC.PageList;
using DC.IDE.UI.Util;

using DCIDE.UI.VM;

using Telerik.Windows.Controls;

namespace TestContainer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public UCModelDetail d { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
             
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
 
    }
}
