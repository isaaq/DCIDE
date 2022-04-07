using System;
using System.Collections.Generic;
using System.IO;
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

using DC.IDE.PackageBuilder.VM;
using DC.IDE.UI.Util;

namespace DC.IDE.PackageBuilder.UC
{
    /// <summary>
    /// UCPkgStruct.xaml 的交互逻辑
    /// </summary>
    public partial class UCPkgStruct : UserControl
    {
        public UCPkgStruct()
        {
            InitializeComponent();
        }

        private void RadTreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (VMMain)this.DataContext;
            var sel = vm.TreeSelItem;
            try
            {
                var sr = File.OpenText(sel.Fullpath);
                var cnt = sr.ReadToEnd();
                sr.Close();
                vm.TreeSelItem.Content = cnt;
                vm.OnPropVM("TreeSelItem");
            }
            catch (Exception)
            {

            }
        }
    }
}
