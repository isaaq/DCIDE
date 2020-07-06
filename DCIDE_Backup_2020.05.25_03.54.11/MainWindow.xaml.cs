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
using DC.IDE.UI.VM;
using DCIDE.VM;
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
            this.DataContext = new MainVM();
        }

        private void FuncList_FuncChanged(object sender, DC.IDE.UI.Model.FuncItem e)
        {
            UserControl uc = null;
            var pane = new RadPane();
            pane.Header = e.Text;
            switch (e.Id)
            {
                case "persons": uc = new UCPerson(); break;
                case "structs": uc = new UCModelDesigner(); ((UCModelDesigner)uc).ListItemClicked += MainWindow_ListItemClicked; break;
                case "pages": uc = new UCPageList(); break;
            }
            pane.Content = uc;
            dockMain.AddItem(pane, Telerik.Windows.Controls.Docking.DockPosition.Center);
        }

        private void MainWindow_ListItemClicked(object sender, DC.IDE.UI.Model.StructItem e)
        {
            //var pane = dockMain.FindChildByType<UCModelDesigner>().Parent;
            //pane.
            var pane = new RadPane();
            pane.Header = string.Format("[{0}]{1}", e.Type, e.Name);
            UserControl uc = null;
            switch (e.Type)
            {
                case "0": uc = new UCModelField(e); ; break;
                case "1": uc = new UCModelDetail(e); break;
            }
            if (uc != null)
                pane.Content = uc;
            dockMain.AddItem(pane, Telerik.Windows.Controls.Docking.DockPosition.Center);
        }
    }
}
