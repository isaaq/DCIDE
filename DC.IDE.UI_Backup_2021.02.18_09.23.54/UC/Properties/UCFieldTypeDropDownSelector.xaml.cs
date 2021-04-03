using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using DC.IDE.UI.VM;

using Telerik.Windows.Controls;

namespace DC.IDE.UI.UC.Properties
{

    /// <summary>
    /// UCFieldTypeDropDownSelector.xaml 的交互逻辑
    /// </summary>
    public partial class UCFieldTypeDropDownSelector : UserControl
    {
        public VMFieldTypeDropDownSelector VM { get; set; }

        public static readonly DependencyProperty SelectedItemProperty =
           DependencyProperty.Register("SelectedItem", typeof(string), typeof(UCFieldTypeDropDownSelector), null);

        public string SelectedItem
        {
            get
            {
                return (string)this.GetValue(SelectedItemProperty);
            }
            set
            {
                this.SetValue(SelectedItemProperty, value);
            }
        }

        public UCFieldTypeDropDownSelector()
        {
            InitializeComponent();
            VM = new VMFieldTypeDropDownSelector();
            list.DataContext = VM;
            VM.PropertyChanged += VM_PropertyChanged;
        }

        private void VM_PropertyChanged(object sender, dynamic e)
        {
            SelectedItem = VM.SelectedItem.ToString();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var parentWindow = (sender as ListBox).ParentOfType<Popup>();
            if (parentWindow != null)
            {
                var parent = (FrameworkElement)VisualTreeHelper.GetParent(this);
                if (parent != null)
                {
                    //dynamic parentdc = parent.DataContext;
                    //parent.DataContext = SelectedItem;
                }
                parentWindow.IsOpen = false;
            }
        }
    }
}
