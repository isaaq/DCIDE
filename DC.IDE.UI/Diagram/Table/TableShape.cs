// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.TableShape
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

using Telerik.Windows.Controls;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Table
{
    public class TableShape : RadDiagramContainerShape
    {
        protected override bool IsDropPossible
        {
            get
            {
                return false;
            }
        }

        protected override void OnItemsCollectionChanged(
          object sender,
          NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsCollectionChanged(sender, e);
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                RowModel oldItem = e.OldItems[0] as RowModel;
                if (oldItem == null)
                    return;
                foreach (RowModel rowModel1 in this.Items.OfType<RowModel>())
                {
                    Point position = rowModel1.Position;
                    double y1 = position.Y;
                    position = oldItem.Position;
                    double y2 = position.Y;
                    if (y1 >= y2)
                    {
                        RowModel rowModel2 = rowModel1;
                        position = rowModel1.Position;
                        double x = position.X;
                        position = rowModel1.Position;
                        double y3 = position.Y - 30.0;
                        Point point = new Point(x, y3);
                        rowModel2.Position = point;
                    }
                }
                this.Height -= 30.0;
            }
            else
            {
                if (e.Action != NotifyCollectionChangedAction.Add)
                    return;
                RowModel newItem = e.NewItems[0] as RowModel;
                if (newItem == null)
                    return;
                if (this.Diagram != null)
                    if (this.Bounds.Bottom - 25.0 < newItem.Position.Y && (this.Diagram as RadDiagram).UndoRedoService.IsActive)
                        this.ContentBounds = this.CalculateContentBounds(this.Bounds);
                this.RefreshConnections();
            }
        }

        private void RefreshConnections()
        {
            if (this.Diagram != null)
                foreach (IConnection connection in (ReadOnlyCollection<IConnection>)this.Diagram.Connections)
                    connection.Update(false);
        }
    }
}
