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
using DC.IDE.UI.VM;
using Telerik.Windows.Controls;

namespace DC.IDE.UI.UC
{
    /// <summary>
    /// UCModelDesigner.xaml 的交互逻辑
    /// </summary>
    public partial class UCModelDesigner : UserControl
    {
        private VMStruct _vmStruct = new VMStruct();

        public event EventHandler<StructItem> ListItemClicked;

        public UCModelDesigner()
        {
            InitializeComponent();
            this.AddHandler(Hyperlink.ClickEvent, new RoutedEventHandler(GridViewHyperlinkColumn_Click));
            this.DataContext = _vmStruct;
        }

        private void GridViewHyperlinkColumn_Click(object sender, RoutedEventArgs e)
        {
            var hyperLink = e.OriginalSource as Hyperlink;
            if (this.ListItemClicked == null)
                return;
            var item = hyperLink.DataContext as StructItem;
            this.ListItemClicked(this, item);
        }

        private void RadTreeView_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var item = e.OriginalSource as RadTreeViewItem;
            var uid = item.Uid;
            if (uid != "")
            {
                _vmStruct.Select(uid);
            }
        }
    }
}
 