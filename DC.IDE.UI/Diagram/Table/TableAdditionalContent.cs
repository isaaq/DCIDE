// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.TableAdditionalContent
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Table
{
  public class TableAdditionalContent : AdditionalContent
  {
    protected override void OnContextItemChanged(object newValue, object oldValue)
    {
      if (newValue is TableModel)
      {
        this.Visibility = Visibility.Visible;
        if (this.addRemove != null)
          this.addRemove.Visibility = Visibility.Visible;
        if (this.settingsPane == null)
          return;
        this.settingsPane.Opacity = 0.0;
        this.settingsPane.IsHitTestVisible = false;
      }
      else
        this.Visibility = Visibility.Collapsed;
    }

    protected override void OnAdd()
    {
      TableModel model = this.ContextItem as TableModel;
      if (model == null)
        return;
      RowModel itemToAdd = new RowModel()
      {
        ColumnName = "NewRow",
        DataType = DataType.String
      };
      this.Diagram.UndoRedoService.ExecuteCommand((Telerik.Windows.Diagrams.Core.ICommand) new UndoableDelegateCommand("Add new row", (Action<object>) (o => this.AddNewRow(model, (NodeViewModelBase) itemToAdd)), (Action<object>) (o => this.RemoveRow(model, (NodeViewModelBase) itemToAdd)), (Predicate<object>) null), (object) null);
    }

    protected override void OnCanRemove(CanExecuteRoutedEventArgs e)
    {
      TableModel contextItem = this.ContextItem as TableModel;
      if (contextItem == null || contextItem.InternalItems.Count <= 0)
        return;
      e.CanExecute = true;
    }

    protected override void OnRemove()
    {
      TableModel model = this.ContextItem as TableModel;
      if (model == null || model.InternalItems.Count <= 0)
        return;
      NodeViewModelBase itemToRemove = model.InternalItems.LastOrDefault<NodeViewModelBase>();
      this.Diagram.UndoRedoService.ExecuteCommand((Telerik.Windows.Diagrams.Core.ICommand) new UndoableDelegateCommand("Add new row", (Action<object>) (o => this.RemoveRow(model, itemToRemove)), (Action<object>) (o => this.AddNewRow(model, itemToRemove)), (Predicate<object>) null), (object) null);
    }

    private void AddNewRow(TableModel model, NodeViewModelBase itemToRemove)
    {
      if (model.IsCollapsed)
        model.IsCollapsed = false;
      if (itemToRemove == null)
        model.AddItem((object) new RowModel()
        {
          ColumnName = "NewRow",
          DataType = DataType.String
        });
      else
        model.AddItem((object) itemToRemove);
    }

    private void RemoveRow(TableModel model, NodeViewModelBase itemToRemove)
    {
      TablesGraphSource dataContext = this.DataContext as TablesGraphSource;
      if (dataContext != null && model != null && itemToRemove != null)
      {
        if (model.IsCollapsed)
          model.IsCollapsed = false;
        dataContext.RemoveItem(itemToRemove);
      }
      CommandManager.InvalidateRequerySuggested();
    }
  }
}
