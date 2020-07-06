// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.RowModel
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;

namespace DC.IDE.UI.Diagram.Table
{
  public class RowModel : NodeViewModelBase
  {
    private string columnName;
    private DataType dataType;

    public DataType DataType
    {
      get
      {
        return this.dataType;
      }
      set
      {
        if (this.dataType == value)
          return;
        this.dataType = value;
        this.OnPropertyChanged(nameof (DataType));
      }
    }

    public string ColumnName
    {
      get
      {
        return this.columnName;
      }
      set
      {
        if (!(this.columnName != value))
          return;
        this.columnName = value;
        this.OnPropertyChanged(nameof (ColumnName));
      }
    }
  }
}
