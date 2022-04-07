using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

using Telerik.Windows.SyntaxEditor.Core.Text;

namespace DC.IDE.PackageBuilder
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public VMMain VM { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            VM = new VMMain();
            this.DataContext = VM;
            VM.OnTreeSelChanged += VM_OnTreeSelChanged;
        }

        private static string GetSection(string content, string sectionName)
        {
            string r = @"(<" + sectionName + @">[\s\S]*?<\/" + sectionName + ">)";
            var m = Regex.Match(content, r, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            return "";
        }

        private void VM_OnTreeSelChanged(object sender, Model.PkgItem e)
        {
            if (e.Content != null)
            {
                codeTemplate.Document = new TextDocument(GetSection(e.Content, "template"));
                scriptTemplate.Document = new TextDocument(GetSection(e.Content, "script"));
                styleTemplate.Document = new TextDocument(GetSection(e.Content, "style"));
            }
        }
    }
}
