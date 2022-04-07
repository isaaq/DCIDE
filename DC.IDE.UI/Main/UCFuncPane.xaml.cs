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
using DC.IDE.UI.UC;

using Telerik.Windows.Controls;

namespace DC.IDE.UI.Main
{
    /// <summary>
    /// UCFuncPane.xaml 的交互逻辑
    /// </summary>
    public partial class UCFuncPane : UserControl
    {
        public event EventHandler<RadPane> OnFuncChange;

        public UCFuncPane()
        {
            InitializeComponent();
        }

        private void FuncList_FuncChanged(object sender, Model.FuncItem e)
        {
            UserControl uc = null;
            var pane = new RadPane();
            pane.Name = e.Id;
            pane.Header = e.Text;
            switch (e.Id)
            {
                case "persons": uc = new UCPerson(); break;
                case "structs": uc = new UCModelDesigner(); ((UCModelDesigner)uc).ListItemClicked += MainWindow_ListItemClicked; break;
                case "pages": uc = new UCPageList(); break;
                case "webparts": uc = new UCWebpartList(); break;
            }
            pane.Content = uc;
            OnFuncChange(this, pane);
        }

        private void MainWindow_ListItemClicked(object sender, StructItem e)
        {
            var pane = new RadPane();
            pane.Header = string.Format("[{0}]{1}", e.Type, e.Name);
            UserControl uc = null;
            if (e.Type == null)
                e.Type = "0";
            switch (e.Type)
            {
                case "0": uc = new UCModelField(e); pane.Name = "modelfield"; break;
                case "1": uc = new UCModelDetail(e); pane.Name = "modeldetail"; break;
            }
            if (uc != null)
                pane.Content = uc;
            OnFuncChange(this, pane);
        }
    }
}
