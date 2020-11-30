using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PostSharp.Patterns.Model;

using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Form.VM
{
    [NotifyPropertyChanged]
    public class VMFormDiagram : ViewModelBase
{
        public VMFormDiagramGraphSource DiagramSource { get; set; }
        public ObservableCollection<NodeViewModelBase> Nodes { get; set; }
        public VMFormDiagram()
        {
            this.Nodes = new ObservableCollection<NodeViewModelBase>();
            
        }

       
    }
}
