// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.ShapeStyleSelector
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System.Windows;
using System.Windows.Controls;

namespace DC.IDE.UI.Diagram.Table
{
  public class ShapeStyleSelector : StyleSelector
  {
    public Style RowStyle { get; set; }

    public Style TableStyle { get; set; }

    public override Style SelectStyle(object item, DependencyObject container)
    {
      if (item is RowModel)
        return this.RowStyle;
      return this.TableStyle;
    }
  }
}
