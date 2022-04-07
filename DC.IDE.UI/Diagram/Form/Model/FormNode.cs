using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;

namespace DC.IDE.UI.Diagram.Form.Model
{
    public class FormNode : ContainerNodeViewModelBase<NodeViewModelBase>
    {
        private bool isCollapsed;

        public bool IsCollapsed
        {
            get
            {
                return this.isCollapsed;
            }
            set
            {
                if (this.isCollapsed == value)
                    return;
                this.isCollapsed = value;
                this.OnPropertyChanged(nameof(IsCollapsed));
            }
        }

        public override bool AddItem(object item)
        {
            FormItemNode rowModel = item as FormItemNode;
            if (rowModel == null)
                return false;
            rowModel.Position = new Point(this.Position.X, this.Position.Y + 90.0 + (double)(this.InternalItems.Count * 30));
            return base.AddItem(item);
        }

        public override bool RemoveItem(object item)
        {
            if (item is NodeViewModelBase)
                return base.RemoveItem(item);
            return false;
        }
    }
}
