using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DC.IDE.UI.Diagram.Form.Model;
using DC.IDE.UI.Model;
using DC.IDE.UI.Util;
using PostSharp.Patterns.Model;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;

namespace DC.IDE.UI.Diagram.Form.VM
{
    [NotifyPropertyChanged]
    public class VMFormDiagram : ViewModelBase
{
        public VMFormDiagramGraphSource DiagramSource { get; set; }
        public ObservableCollection<NodeViewModelBase> Nodes { get; set; }
        public VMFormDiagram(StructItem item)
        {
            this.Nodes = new ObservableCollection<NodeViewModelBase>();
             
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var t_c = m.GetTable("sys_forms");
            var form = t_c.Find(t_c.Filter(f=>f.Eq("_id", item.ID)))[0];
            var attrs = form["attribute"].AsBsonArray;
            foreach(var attr in attrs)
            {
                var fi = new FormItemNode();
                name = attr["Name"].AsString;
            }
        }

       
    }
}
