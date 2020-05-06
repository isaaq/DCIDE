// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.DataTypeExtension
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using System.Windows.Markup;

namespace DC.IDE.UI.Diagram.Table
{
  public class DataTypeExtension : MarkupExtension
  {
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return (object) Enum.GetValues(typeof (DataType));
    }
  }
}
