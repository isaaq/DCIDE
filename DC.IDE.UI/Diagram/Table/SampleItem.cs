// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.SampleItem
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using Telerik.Windows.Controls;

namespace DC.IDE.UI.Diagram.Table
{
  public sealed class SampleItem
  {
    public string Title { get; set; }

    public string Icon { get; set; }

    public string Description { get; set; }

    public Action<RadDiagram> Run { get; set; }
  }
}
