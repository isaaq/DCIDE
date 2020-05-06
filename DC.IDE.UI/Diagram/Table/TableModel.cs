// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.TableModel
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System.Windows;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;

namespace DC.IDE.UI.Diagram.Table
{
  public class TableModel : ContainerNodeViewModelBase<NodeViewModelBase>
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
        this.OnPropertyChanged(nameof (IsCollapsed));
      }
    }

    public override bool AddItem(object item)
    {
      RowModel rowModel = item as RowModel;
      if (rowModel == null)
        return false;
      rowModel.Position = new Point(this.Position.X, this.Position.Y + 90.0 + (double) (this.InternalItems.Count * 30));
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
