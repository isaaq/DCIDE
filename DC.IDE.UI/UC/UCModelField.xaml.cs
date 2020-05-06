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

namespace DC.IDE.UI.UC
{
    /// <summary>
    /// UCModelField.xaml 的交互逻辑
    /// </summary>
    public partial class UCModelField : UserControl
    {
        private StructItem StructItem;

        public UCModelField(StructItem e)
        {
            this.StructItem = e;
            this.DataContext = new VMModelField(e);
        }
    }
}
