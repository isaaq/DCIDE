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
using DC.IDE.UI.VM;

namespace DC.IDE.UI.UC.Layout
{
    /// <summary>
    /// LySettings.xaml 的交互逻辑
    /// </summary>
    public partial class LySettings : UserControl
    {
        public LySettings()
        {
            InitializeComponent();
            this.DataContext = new VMSettings();
        }
    }
}
