using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC.IDE.UI.Model.Field;
using PostSharp.Patterns.Model;
using Telerik.Windows.Controls;

namespace DC.IDE.UI.VM
{
    [NotifyPropertyChanged]
    public class VMFieldTypeDropDownSelector : ViewModelBase
    {
        public ObservableCollection<FieldType> TypeList { get; set; }
        public FieldType SelectedItem { get; set; }

        public VMFieldTypeDropDownSelector()
        {
            TypeList = new ObservableCollection<FieldType>();
            var names = Enum.GetNames(typeof(FieldType));
            foreach (var name in names)
            {
                var val = Enum.Parse(typeof(FieldType), name);
                TypeList.Add((FieldType)val);
            }
        }

    }
}
