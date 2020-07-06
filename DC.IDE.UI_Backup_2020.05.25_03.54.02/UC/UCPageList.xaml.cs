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
using DC.IDE.UI.Model;
using DC.IDE.UI.UC.PageList;
using DC.IDE.UI.VM;
using Telerik.Windows.Controls;

namespace DC.IDE.UI.UC
{
    /// <summary>
    /// UCPageList.xaml 的交互逻辑
    /// </summary>
    public partial class UCPageList : UserControl
    {
        public WinPageAdd addwin { get; set; }

        public UCPageList()
        {
            InitializeComponent();
            var vmpage = new VMPage();
            vmpage.RequestClose += Vmpage_RequestClose;
            this.DataContext = vmpage;
        }

        private void Vmpage_RequestClose(object sender, EventArgs e)
        {
            addwin.Close();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (((RadRibbonButton)sender).Tag.ToString() == "page")
            {
                addwin = new WinPageAdd();
                addwin.InvalidateVisual();
                addwin.ShowDialog();
            }
            else
            {

            }
        }
    }
}
