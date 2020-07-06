using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

using DCIDE.UI.VM;

using Telerik.Windows.Controls;
using Telerik.Windows.Examples.RibbonView.MVVM.ViewModels;

namespace DC.IDE.UI.UC
{
    /// <summary>
    /// UCWebpartList.xaml 的交互逻辑
    /// </summary>
    public partial class UCWebpartList : UserControl
    {
        public WinPageAdd addwin { get; set; }
        public VMWebpart vmwebpart { get; private set; }

        public UCWebpartList()
        {
            InitializeComponent();
            vmwebpart = new VMWebpart();
            this.DataContext = vmwebpart;

            vmwebpart.RequestClose += Vmpage_RequestClose;
            vmwebpart.ElementInserted += Vmpage_ElementInserted;
        }

        private void ChangePropVM()
        {
            var parent = (FrameworkElement)VisualTreeHelper.GetParent(this);
            var parentdc = (VMMain)parent.DataContext;

            parentdc.PropList = vmwebpart.SelItem?.PropertyList;
        }

        private void Vmpage_ElementInserted(object sender, int e)
        {
            txtContent.CaretIndex = e;
        }

        private void Vmpage_RequestClose(object sender, EventArgs e)
        {
            addwin.Close();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
                addwin = new WinPageAdd();
                vmwebpart.SelItem = new WebPartPageModel();
                addwin.DataContext = vmwebpart;
                addwin.ShowDialog();
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            vmwebpart.CurrentPosition = (e.OriginalSource as TextBox).CaretIndex;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ChangePropVM();
        }
    }
}
