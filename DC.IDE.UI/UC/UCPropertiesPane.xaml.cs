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

namespace DC.IDE.UI.UC
{
    /// <summary>
    /// UCPropertiesPane.xaml 的交互逻辑
    /// </summary>
    public partial class UCPropertiesPane : UserControl
    {
        public UCPropertiesPane()
        {
            InitializeComponent();

            //dynamic PropList = new PropModel();
            //for (int i = 0; i < 20; i++)
            //{
            //    PropList[string.Format("Property {0}", i)] = string.Format("Value {0}", i);
            //}
            //propgrid.Item = PropList;
        }

        private void RadPropertyGrid_AutoGeneratingPropertyDefinition(object sender, Telerik.Windows.Controls.Data.PropertyGrid.AutoGeneratingPropertyDefinitionEventArgs e)
        {
           // e.PropertyDefinition.GroupName = e.PropertyDefinition.DisplayName; //(Int32.Parse(e.PropertyDefinition.DisplayName.Substring(9)) % 1000).ToString();
           //e.PropertyDefinition.
        }
    }
}
