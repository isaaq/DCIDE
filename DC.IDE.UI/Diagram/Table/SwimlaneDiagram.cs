// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.SwimlaneDiagram
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;
using Telerik.Windows.DragDrop;

namespace DC.IDE.UI.Diagram.Table
{
  public class SwimlaneDiagram : RadDiagram
  {
    private CustomManipulationAdorner manipulationAdorner;
    private FrameworkElement horizontalDragVisual;
    private FrameworkElement verticalDragVisual;
    private DiagramSurface itemHost;

    public SwimlaneDiagram()
    {
      DragDropManager.AddDropHandler((DependencyObject) this, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDrop));
      this.ItemsChanging += new EventHandler<DiagramItemsChangingEventArgs>(this.OnSwimlaneDiagramItemsChanging);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.manipulationAdorner = this.GetTemplateChild("ManipulationAdorner") as CustomManipulationAdorner;
      this.horizontalDragVisual = this.GetTemplateChild("PART_horizontalDragOver") as FrameworkElement;
      this.verticalDragVisual = this.GetTemplateChild("PART_verticalDragOver") as FrameworkElement;
      this.itemHost = this.GetTemplateChild("ItemsHost") as DiagramSurface;
      this.SelectionChanged += new SelectionChangedEventHandler(this.OnSelectionChanged);
    }

    protected override void OnDeleteCommandExecutedOverride(
      object sender,
      ExecutedRoutedEventArgs e)
    {
      CompositeCommand compositeCommand = new CompositeCommand("Remove Connections", (Action<object>) null, (Action<object>) null, (Predicate<object>) null);
      foreach (object selectedItem in this.SelectedItems)
      {
        IShape shape = this.ContainerGenerator.ContainerFromItem(selectedItem) as IShape;
        if (shape != null)
        {
          foreach (IConnection connection1 in this.GetConnectionsForShape(shape).ToList<IConnection>())
          {
            IConnection connection = connection1;
            compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) new UndoableDelegateCommand("Remove Connection", (Action<object>) (o => this.RemoveConnection(connection, false)), (Action<object>) (o => this.AddConnection(connection, false)), (Predicate<object>) null));
          }
        }
      }
      base.OnDeleteCommandExecutedOverride(sender, e);
      if (compositeCommand.Commands.Count<Telerik.Windows.Diagrams.Core.ICommand>() <= 0)
        return;
      compositeCommand.Execute((object) null);
      ((this.UndoRedoService.UndoStack.FirstOrDefault<Telerik.Windows.Diagrams.Core.ICommand>() as CompositeCommand).Commands as IList<Telerik.Windows.Diagrams.Core.ICommand>).Add((Telerik.Windows.Diagrams.Core.ICommand) compositeCommand);
    }

    protected override bool PublishDiagramEvent(DiagramEvent diagramEvent, object args)
    {
      if (diagramEvent == DiagramEvent.SelectionBoundsChanged)
        this.UpdateHandlers(this.SelectedItem as MainContainerShapeBase);
      return base.PublishDiagramEvent(diagramEvent, args);
    }

    internal WriteableBitmap CreateImage(Rect bounds)
    {
      return BitmapUtils.CreateWriteableBitmap((UIElement) this.itemHost, bounds, bounds.ToSize(), (Brush) null, new Thickness(), 96.0);
    }

    internal void ShowDragOverVisual(SwimlaneShapeBase shape)
    {
      if (shape.ParentMainContainer == null)
        return;
      Rect bounds = shape.Bounds;
      if (shape.ParentMainContainer.ChildrenPositioning == System.Windows.Controls.Orientation.Vertical)
      {
        if (this.horizontalDragVisual == null)
          return;
        this.horizontalDragVisual.Visibility = Visibility.Visible;
        Canvas.SetLeft((UIElement) this.horizontalDragVisual, bounds.X);
        Canvas.SetTop((UIElement) this.horizontalDragVisual, bounds.Bottom);
        this.horizontalDragVisual.Width = bounds.Width;
      }
      else
      {
        if (this.verticalDragVisual == null)
          return;
        this.verticalDragVisual.Visibility = Visibility.Visible;
        Canvas.SetLeft((UIElement) this.verticalDragVisual, bounds.Right);
        Canvas.SetTop((UIElement) this.verticalDragVisual, bounds.Y);
        this.verticalDragVisual.Height = bounds.Height;
      }
    }

    internal void HideDragOverVisual()
    {
      if (this.horizontalDragVisual != null)
        this.horizontalDragVisual.Visibility = Visibility.Collapsed;
      if (this.verticalDragVisual == null)
        return;
      this.verticalDragVisual.Visibility = Visibility.Collapsed;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      MainContainerShapeBase selectedItem = this.SelectedItem as MainContainerShapeBase;
      if (e.AddedItems.Count > 0 && this.manipulationAdorner != null && selectedItem != null)
      {
        this.manipulationAdorner.AdditionalHandlersVisibility = Visibility.Visible;
        this.UpdateHandlers(selectedItem);
      }
      else
        this.manipulationAdorner.AdditionalHandlersVisibility = Visibility.Collapsed;
    }

    private void OnDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      SwimlaneShapeBase swimlane = (e.Data as DataObject).GetData(typeof (SwimlaneShapeBase)) as SwimlaneShapeBase;
      if (swimlane == null)
        return;
      Point newPosition = this.GetTransformedPoint(e.GetPosition((IInputElement) this)).Substract(swimlane.DragStartOffset);
      Point oldPosition = swimlane.Position;
      MainContainerShapeBase mainParent = swimlane.ParentContainer as MainContainerShapeBase;
      if (mainParent != null)
        this.UndoRedoService.ExecuteCommand((Telerik.Windows.Diagrams.Core.ICommand) new UndoableDelegateCommand("Remove container from main", (Action<object>) (o =>
        {
          swimlane.Position = newPosition;
          mainParent.Items.Remove((object) swimlane);
          mainParent.UpdateChildContainers(false);
        }), (Action<object>) (o =>
        {
          swimlane.Position = oldPosition;
          mainParent.Items.Add((object) swimlane);
          mainParent.UpdateChildContainers(false);
        }), (Predicate<object>) null), (object) null);
      else
        this.UndoRedoService.ExecuteCommand((Telerik.Windows.Diagrams.Core.ICommand) new UndoableDelegateCommand("Remove container from main", (Action<object>) (o => swimlane.Position = newPosition), (Action<object>) (o => swimlane.Position = oldPosition), (Predicate<object>) null), (object) null);
    }

    private void OnSwimlaneDiagramItemsChanging(object sender, DiagramItemsChangingEventArgs e)
    {
      if (e.Action != NotifyCollectionChangedAction.Remove)
        return;
      SwimlaneShapeBase container = e.OldItems.ElementAt<object>(0) as SwimlaneShapeBase;
      if (container != null && container.ParentMainContainer != null)
      {
        foreach (SwimlaneShapeBase swimlaneShapeBase in container.ParentMainContainer.OrderedChildren.Where<SwimlaneShapeBase>((Func<SwimlaneShapeBase, bool>) (c => c.ContainerPosition > container.ContainerPosition)))
          --swimlaneShapeBase.ContainerPosition;
      }
    }

    private void UpdateHandlers(MainContainerShapeBase mainContainer)
    {
      if (mainContainer == null)
        return;
      Point boundsPosition = this.ServiceLocator.GetService<IAdornerService>().InflatedAdornerBounds.TopLeft();
      if (this.manipulationAdorner != null && !boundsPosition.X.IsNanOrInfinity() && !boundsPosition.Y.IsNanOrInfinity())
        this.manipulationAdorner.UpdateAdditionalHandlers(mainContainer.OrderedChildren, boundsPosition, !mainContainer.IsCollapsed, mainContainer.ChildrenPositioning);
    }
  }
}
