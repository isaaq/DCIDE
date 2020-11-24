// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.SqlDiagram
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Table
{
  public class SqlDiagram : RadDiagram
  {
        protected override IContainerShape GetShapeContainerForItemOverride(
          IContainerItem item)
        {
            var ts = new TableShape();
            return (IContainerShape)ts;
        }

        protected override IShape GetShapeContainerForItemOverride(object item)
        {
            return (IShape)new RowShape();
        }

        protected override bool IsItemItsOwnShapeContainerOverride(object item)
        {
            return item is RadDiagramShapeBase;
        }

        public override void Paste()
        {
            if (!(this.ContainerGenerator.ContainerFromItem(this.SelectedItem) is TableShape))
                return;
            base.Paste();
        }
    }
}
