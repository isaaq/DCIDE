// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.MainContainerShapeBase
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;
using Telerik.Windows.DragDrop;

namespace DC.IDE.UI.Diagram.Table
{
  public class MainContainerShapeBase : CustomContainerBase, IContainerShape, IShape, IDiagramItem, ISerializable, ISupportMouseOver, IPropertyChanged, IGroupable, ISupportManipulation, ISupportVirtualization, IContainerChild, ICollapsible, ISupportRotation, IDragDropAware
  {
    private static readonly double childrenMargin = 0.0;
    private static readonly Size newItemSize = new Size(200.0, 110.0);
    private readonly List<IConnection> collapsedItems;
    private List<SwimlaneShapeBase> orderedChildren;
    private bool isInternalResize;

    public List<SwimlaneShapeBase> OrderedChildren
    {
      get
      {
        return this.orderedChildren;
      }
    }

    public System.Windows.Controls.Orientation ChildrenPositioning { get; set; }

    public MainContainerShapeBase()
    {
      this.ChildrenPositioning = System.Windows.Controls.Orientation.Vertical;
      DragDropManager.AddDragEnterHandler((DependencyObject) this, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDragEnterManager));
      this.collapsedItems = new List<IConnection>();
    }

    public void UpdateMinBounds()
    {
      foreach (SwimlaneShapeBase swimlaneShapeBase in this.Children.OfType<SwimlaneShapeBase>())
        swimlaneShapeBase.UpdateMinBounds();
      this.MinBounds = this.CalculateMinShapeBounds();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.UpdateChildContainers(false);
    }

    public void MoveTo(SwimlaneShapeBase dragObject, int endPosition, bool isUndoable = true)
    {
      bool shouldAdd = !this.Items.Contains((object) dragObject);
      bool addToDiagram = !this.Diagram.Items.Contains((object) dragObject);
      Point diagramPosition = dragObject.Position;
      int startPosition = shouldAdd ? this.Items.Count : dragObject.ContainerPosition;
      endPosition = startPosition <= endPosition ? endPosition : endPosition + 1;
      Action<object> execute = (Action<object>) (o =>
      {
        this.MoveContainer(startPosition, endPosition, dragObject);
        if (shouldAdd)
          this.AddItem((object) dragObject, new Point?());
        this.UpdateChildContainers(false);
      });
      if (isUndoable)
        this.Diagram.ServiceLocator.GetService<IUndoRedoService>().ExecuteCommand((ICommand) new UndoableDelegateCommand("Move container", execute, (Action<object>) (o =>
        {
          this.MoveContainer(endPosition, startPosition, dragObject);
          if (shouldAdd)
          {
            this.RemoveItem((object) dragObject);
            dragObject.Position = diagramPosition;
            if (addToDiagram)
              this.Diagram.Items.Remove((object) dragObject);
          }
          this.UpdateChildContainers(false);
        }), (Predicate<object>) null), (object) null);
      else
        execute((object) null);
    }

    public void UpdateChildContainers(bool resizeChildren = false)
    {
      Rect contentBounds = this.ContentBounds;
      this.orderedChildren = this.Children.OfType<SwimlaneShapeBase>().OrderBy<SwimlaneShapeBase, int>((Func<SwimlaneShapeBase, int>) (c => c.ContainerPosition)).ToList<SwimlaneShapeBase>();
      if (!this.loaded || !this.templateApplied || this.orderedChildren.Count <= 0)
        return;
      if (this.ChildrenPositioning == System.Windows.Controls.Orientation.Vertical)
      {
        double num1 = contentBounds.Width;
        if (!resizeChildren)
          num1 = Math.Max(contentBounds.Width, this.OrderedChildren.Max<SwimlaneShapeBase>((Func<SwimlaneShapeBase, double>) (c => c.ContentBounds.Width)));
        for (int index = 0; index < this.orderedChildren.Count; ++index)
        {
          SwimlaneShapeBase orderedChild = this.orderedChildren[index];
          Rect rect;
          if (index == 0)
          {
            orderedChild.Position = new Point(contentBounds.X, contentBounds.Y);
          }
          else
          {
            SwimlaneShapeBase swimlaneShapeBase = orderedChild;
            double x = contentBounds.X;
            rect = this.orderedChildren[index - 1].Bounds;
            double y = rect.Bottom - 1.0 + MainContainerShapeBase.childrenMargin;
            Point point = new Point(x, y);
            swimlaneShapeBase.Position = point;
          }
          if (double.IsNaN(orderedChild.Height) || orderedChild.Height == 0.0)
            orderedChild.Height = MainContainerShapeBase.newItemSize.Height;
          orderedChild.Width = num1;
          int num2;
          if (index == this.orderedChildren.Count - 1)
          {
            rect = orderedChild.Bounds;
            num2 = rect.Bottom != contentBounds.Bottom ? 1 : 0;
          }
          else
            num2 = 0;
          if (num2 != 0)
          {
            bool flag = true;
            if (resizeChildren)
            {
              double num3;
              if (!(orderedChild.MinBounds != Rect.Empty))
              {
                rect = orderedChild.Bounds;
                num3 = rect.Top + CustomResizingService.MinShapeHeight;
              }
              else
              {
                rect = orderedChild.MinBounds;
                num3 = rect.Bottom;
              }
              double num4 = num3;
              if (num4 > contentBounds.Bottom)
              {
                orderedChild.Height = num4 - orderedChild.Y;
              }
              else
              {
                orderedChild.Height = contentBounds.Bottom - orderedChild.Y;
                flag = false;
              }
            }
            if (flag)
            {
              this.isInternalResize = true;
              Point location = this.ContentBounds.TopLeft();
              double width = num1;
              rect = orderedChild.Bounds;
              double bottom = rect.Bottom;
              rect = this.ContentBounds;
              double y = rect.Y;
              double height = bottom - y;
              Size size = new Size(width, height);
              this.ContentBounds = new Rect(location, size);
            }
          }
        }
      }
      else
      {
        double height = contentBounds.Height;
        if (!resizeChildren)
          height = Math.Max(contentBounds.Height, this.OrderedChildren.Max<SwimlaneShapeBase>((Func<SwimlaneShapeBase, double>) (c => c.ContentBounds.Height)));
        for (int index = 0; index < this.orderedChildren.Count; ++index)
        {
          SwimlaneShapeBase orderedChild = this.orderedChildren[index];
          Rect rect;
          if (index == 0)
          {
            orderedChild.Position = new Point(contentBounds.X, contentBounds.Y);
          }
          else
          {
            SwimlaneShapeBase swimlaneShapeBase = orderedChild;
            rect = this.orderedChildren[index - 1].Bounds;
            Point point = new Point(rect.Right - 1.0 + MainContainerShapeBase.childrenMargin, contentBounds.Y);
            swimlaneShapeBase.Position = point;
          }
          if (double.IsNaN(orderedChild.Width) || orderedChild.Width == 0.0)
            orderedChild.Width = MainContainerShapeBase.newItemSize.Width;
          orderedChild.Height = height;
          int num1;
          if (index == this.orderedChildren.Count - 1)
          {
            rect = orderedChild.Bounds;
            num1 = rect.Right != contentBounds.Right ? 1 : 0;
          }
          else
            num1 = 0;
          if (num1 != 0)
          {
            bool flag = true;
            if (resizeChildren)
            {
              double num2;
              if (!(orderedChild.MinBounds != Rect.Empty))
              {
                rect = orderedChild.Bounds;
                num2 = rect.Left + CustomResizingService.MinShapeWidth;
              }
              else
              {
                rect = orderedChild.MinBounds;
                num2 = rect.Right;
              }
              double num3 = num2;
              if (num3 > contentBounds.Right)
              {
                orderedChild.Width = num3 - orderedChild.X;
              }
              else
              {
                orderedChild.Width = contentBounds.Right - orderedChild.X;
                flag = false;
              }
            }
            if (flag)
            {
              this.isInternalResize = true;
              Point location = this.ContentBounds.TopLeft();
              rect = orderedChild.Bounds;
              Size size = new Size(rect.Right - contentBounds.X, height);
              this.ContentBounds = new Rect(location, size);
            }
          }
        }
      }
    }

    void IContainerShape.AddItem(object item, Point? position)
    {
      SwimlaneShapeBase dragObject = item as SwimlaneShapeBase;
      if (this.loaded && dragObject != null)
      {
        int containerPosition = dragObject.ContainerPosition;
        dragObject.ContainerPosition = this.Items.Count;
        this.MoveTo(dragObject, containerPosition - 1, false);
        this.TryToSelect();
      }
      else
      {
        if (dragObject == null && !(item is IConnection))
          return;
        this.AddItem(item, new Point?());
        this.TryToSelect();
      }
    }

    void IContainerShape.AddItems(IEnumerable<object> items)
    {
      if (!items.Any<object>((Func<object, bool>) (c => c is SwimlaneShapeBase)))
        return;
      this.AddItems(items, true);
      this.UpdateChildContainers(false);
      this.TryToSelect();
      this.OnDragLeave(new DragItemsEventArgs());
    }

    internal void SwapChildren(SwimlaneShapeBase first, SwimlaneShapeBase second)
    {
      int firstPosition = first.ContainerPosition;
      int secondPosition = second.ContainerPosition;
      this.Diagram.ServiceLocator.GetService<IUndoRedoService>().ExecuteCommand((ICommand) new UndoableDelegateCommand("Swap children", (Action<object>) (o =>
      {
        first.ContainerPosition = secondPosition;
        second.ContainerPosition = firstPosition;
        this.UpdateChildContainers(false);
      }), (Action<object>) (o =>
      {
        first.ContainerPosition = firstPosition;
        second.ContainerPosition = secondPosition;
        this.UpdateChildContainers(false);
      }), (Predicate<object>) null), (object) null);
    }

    protected override void OnSizeChanged(Size newSize, Size oldSize)
    {
      base.OnSizeChanged(newSize, oldSize);
      if (this.loaded && !this.isInternalResize && this.Diagram != null && !this.Diagram.ServiceLocator.GetService<IResizingService>().IsResizing)
        this.UpdateChildContainers(true);
      this.isInternalResize = false;
    }

    protected override void OnHeaderPresenterSizeChanged(object sender, SizeChangedEventArgs e)
    {
      base.OnHeaderPresenterSizeChanged(sender, e);
      if (!this.loaded)
        return;
      this.ShouldUpdateChildren = false;
      Rect rect = this.ContentBounds;
      double x = rect.X - DiagramConstants.ContainerMargin - this.actualHeaderWidth;
      rect = this.Bounds;
      double val2 = rect.Right - x;
      double num = val2 - this.Width;
      this.Position = new Point(x, this.Position.Y);
      this.Width = Math.Max(0.0, val2);
      if (this.IsCollapsed && e.PreviousSize.Height != 0.0)
      {
        if (num.IsNanOrInfinity() || num == 0.0)
          num = e.NewSize.Height - e.PreviousSize.Height;
        this.SetValue(FrameworkElement.WidthProperty, FrameworkElement.WidthProperty.GetMetadata(typeof (FrameworkElement)).DefaultValue);
        this.restoredWidth = Math.Max(0.0, this.restoredWidth + num);
      }
      this.ShouldUpdateChildren = true;
    }

    protected override void OnDrop(DragItemsEventArgs args)
    {
    }

    protected override void OnDragEnter(DragItemsEventArgs args)
    {
      if (args.Items.Count<IDiagramItem>() <= 0 || !args.Items.Any<IDiagramItem>((Func<IDiagramItem, bool>) (c => c is SwimlaneShapeBase)))
        return;
      base.OnDragEnter(args);
    }

    protected override void OnManagerDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      SwimlaneShapeBase swimlane = this.GetSwimlane(e);
      if (swimlane == null)
        return;
      VisualStateManager.GoToState((FrameworkElement) this, "DropNormal", false);
      bool isCollapsed = this.IsCollapsed;
      if (isCollapsed)
        this.IsCollapsed = false;
      if (swimlane != null)
        this.MoveTo(swimlane, this.Items.Count, true);
      this.TryToSelect();
      this.IsCollapsed = isCollapsed;
      e.Handled = true;
    }

    protected override Rect CalculateMinShapeBounds()
    {
      Rect empty = Rect.Empty;
      foreach (SwimlaneShapeBase swimlaneShapeBase in this.Children.OfType<SwimlaneShapeBase>())
        empty.Union(swimlaneShapeBase.MinBounds);
      if (empty != Rect.Empty)
        return this.GetShapeBounds(empty);
      return empty;
    }

    protected override void OnItemsCollectionChanged(
      object sender,
      NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsCollectionChanged(sender, e);
      this.IsDropEnabled = this.Items.Count == 0;
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        SwimlaneShapeBase newItem = e.NewItems[0] as SwimlaneShapeBase;
        if (newItem != null)
          newItem.IsResizingEnabled = false;
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        FrameworkElement oldItem = e.OldItems[0] as FrameworkElement;
        if (oldItem != null)
          oldItem.ClearValue(RadDiagramItem.IsResizingEnabledProperty);
      }
      this.orderedChildren = this.Children.OfType<SwimlaneShapeBase>().OrderBy<SwimlaneShapeBase, int>((Func<SwimlaneShapeBase, int>) (c => c.ContainerPosition)).ToList<SwimlaneShapeBase>();
      this.UpdateChildContainers(false);
    }

    protected override void OnCustomContainerLoaded(object sender, RoutedEventArgs e)
    {
      base.OnCustomContainerLoaded(sender, e);
      this.UpdateChildContainers(false);
    }

    private void TryToSelect()
    {
      RadDiagram diagram = this.Diagram as RadDiagram;
      if (diagram == null)
        return;
      diagram.DeselectAll();
      diagram.SelectedItem = (object) this;
    }

    private void MoveContainer(int startPosition, int endPosition, SwimlaneShapeBase dragObject)
    {
      if (startPosition <= endPosition)
      {
        for (int index = 0; index < this.OrderedChildren.Count; ++index)
        {
          SwimlaneShapeBase orderedChild = this.OrderedChildren[index];
          if (orderedChild.ContainerPosition > startPosition)
          {
            if (orderedChild.ContainerPosition <= endPosition)
              --orderedChild.ContainerPosition;
            else
              break;
          }
        }
        dragObject.ContainerPosition = endPosition;
      }
      else
      {
        for (int index = 0; index < this.OrderedChildren.Count; ++index)
        {
          SwimlaneShapeBase orderedChild = this.OrderedChildren[index];
          if (orderedChild.ContainerPosition >= endPosition)
          {
            if (orderedChild.ContainerPosition < startPosition)
              ++orderedChild.ContainerPosition;
            else
              break;
          }
        }
        dragObject.ContainerPosition = endPosition;
      }
    }

    private void OnDragEnterManager(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      if (this.GetSwimlane(e) == null)
        return;
      base.OnDragEnter(new DragItemsEventArgs());
    }
  }
}
