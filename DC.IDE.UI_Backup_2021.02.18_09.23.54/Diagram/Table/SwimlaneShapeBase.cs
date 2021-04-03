// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.SwimlaneShapeBase
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using System.Collections.Generic;
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
  public class SwimlaneShapeBase : CustomContainerBase, IContainerShape, IShape, IDiagramItem, ISerializable, ISupportMouseOver, IPropertyChanged, IGroupable, ISupportManipulation, ISupportVirtualization, IContainerChild, ICollapsible, ISupportRotation, IDragDropAware
  {
    public static readonly DependencyProperty ContainerPositionProperty = DependencyProperty.Register(nameof (ContainerPosition), typeof (int), typeof (SwimlaneShapeBase), new PropertyMetadata((object) -1));
    private SwimlaneDiagram swimlaneDiagram;
    private bool canDrag;
    private bool keepBackground;

    public SwimlaneShapeBase()
    {
      this.MinBoundsWithChildren = Rect.Empty;
      DragDropManager.AddDragLeaveHandler((DependencyObject) this, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDragLeaveManager));
      DragDropManager.AddDragEnterHandler((DependencyObject) this, new Telerik.Windows.DragDrop.DragEventHandler(this.OnDragEnterManager));
      DragDropManager.AddDragInitializeHandler((DependencyObject) this, new DragInitializeEventHandler(this.OnDragInit));
      DragDropManager.AddDragDropCompletedHandler((DependencyObject) this, new DragDropCompletedEventHandler(this.OnDragComplete));
    }

    public Rect MinBoundsWithChildren { get; set; }

    public bool IsInnerResize { get; set; }

    public int ContainerPosition
    {
      get
      {
        return (int) this.GetValue(SwimlaneShapeBase.ContainerPositionProperty);
      }
      set
      {
        this.SetValue(SwimlaneShapeBase.ContainerPositionProperty, (object) value);
      }
    }

    public Point DragStartOffset { get; private set; }

    internal MainContainerShapeBase ParentMainContainer
    {
      get
      {
        return this.ParentContainer as MainContainerShapeBase;
      }
    }

    public override void OnApplyTemplate()
    {
      if (this.headerElement != null)
      {
        this.headerElement.MouseEnter -= new MouseEventHandler(this.OnHeaderElementMouseEnter);
        this.headerElement.MouseLeave -= new MouseEventHandler(this.OnHeaderElementMouseLeave);
      }
      base.OnApplyTemplate();
      if (this.headerElement == null)
        return;
      this.headerElement.MouseEnter += new MouseEventHandler(this.OnHeaderElementMouseEnter);
      this.headerElement.MouseLeave += new MouseEventHandler(this.OnHeaderElementMouseLeave);
    }

    void IContainerShape.AddItem(object item, Point? position)
    {
      if (item is SwimlaneShapeBase)
        return;
      this.AddItem(item, position);
    }

    void IContainerShape.AddItems(IEnumerable<object> items)
    {
      if (items.Any<object>((Func<object, bool>) (c =>
      {
        if (!(c is SwimlaneShapeBase))
          return c is MainContainerShapeBase;
        return true;
      })))
        return;
      this.AddItems(items, true);
    }

    public override SerializationInfo Serialize()
    {
      bool[] flagArray1 = new bool[2]
      {
        this.IsResizingEnabled,
        this.IsDraggingEnabled
      };
      bool[] flagArray2 = new bool[2];
      if (this.ReadLocalValue(RadDiagramItem.IsResizingEnabledProperty) != DependencyProperty.UnsetValue)
      {
        flagArray2[0] = true;
        this.ClearValue(RadDiagramItem.IsResizingEnabledProperty);
      }
      if (this.ReadLocalValue(RadDiagramItem.IsDraggingEnabledProperty) != DependencyProperty.UnsetValue)
      {
        flagArray2[1] = true;
        this.ClearValue(RadDiagramItem.IsDraggingEnabledProperty);
      }
      SerializationInfo serializationInfo = base.Serialize();
      serializationInfo["ContainerPosition"] = (object) this.ContainerPosition.ToString();
      if (flagArray2[0])
        this.IsResizingEnabled = flagArray1[0];
      if (flagArray2[1])
        this.IsDraggingEnabled = flagArray1[1];
      return serializationInfo;
    }

    public override void Deserialize(SerializationInfo info)
    {
      base.Deserialize(info);
      if (info["ContainerPosition"] == null)
        return;
      this.ContainerPosition = int.Parse(info["ContainerPosition"].ToString());
    }

    public void UpdateMinBounds()
    {
      this.MinBounds = this.GetMinBounds();
    }

    protected override void Initialize(IGraphServiceLocator serviceLocator, IGraphInternal graph)
    {
      base.Initialize(serviceLocator, graph);
      this.swimlaneDiagram = this.Diagram as SwimlaneDiagram;
    }

    protected override void UpdateChildrenPositions(Point oldPosition, Point newPosition)
    {
      if (this.IsInnerResize)
        return;
      base.UpdateChildrenPositions(oldPosition, newPosition);
    }

    protected override void OnChildBoundsChanged(IDiagramItem diagramItem)
    {
    }

    protected override Rect CalculateMinShapeBounds()
    {
      return this.GetMinBounds();
    }

    protected override void OnManagerDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      this.swimlaneDiagram.HideDragOverVisual();
      SwimlaneShapeBase swimlane = this.GetSwimlane(e);
      if (swimlane != null && this != swimlane)
      {
        if (this.ParentMainContainer == null)
          return;
        Point transformedPoint = (this.Diagram as RadDiagram).GetTransformedPoint(e.GetPosition((IInputElement) (this.Diagram as UIElement)));
        this.ParentMainContainer.MoveTo(swimlane, this.ContainerPosition, true);
        if (swimlane.IsPointOverHeaderElement(transformedPoint))
          swimlane.keepBackground = true;
        e.Handled = true;
      }
      else
      {
        if (this.GetMainContainer(e) != null)
          return;
        base.OnManagerDrop(sender, e);
      }
    }

    protected override void OnDragEnter(DragItemsEventArgs args)
    {
      if (args.Items.Count<IDiagramItem>() <= 0 || args.Items.Any<IDiagramItem>((Func<IDiagramItem, bool>) (c => c is CustomContainerBase)))
        return;
      base.OnDragEnter(args);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (!this.IsPointOverHeaderElement((this.Diagram as RadDiagram).GetTransformedPoint(e.GetPosition((IInputElement) (this.Diagram as UIElement)))))
      {
        if (this.ParentMainContainer == null)
          this.IsDraggingEnabled = true;
        else
          this.ClearValue(RadDiagramItem.IsDraggingEnabledProperty);
        base.OnMouseLeftButtonDown(e);
      }
      else
        this.ClearValue(RadDiagramItem.IsDraggingEnabledProperty);
    }

    private void OnDragEnterManager(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      if (this.GetSwimlane(e) != null)
      {
        this.swimlaneDiagram.ShowDragOverVisual(this);
      }
      else
      {
        if (this.GetMainContainer(e) != null)
          return;
        base.OnDragEnter(new DragItemsEventArgs());
      }
    }

    private void OnDragLeaveManager(object sender, Telerik.Windows.DragDrop.DragEventArgs e)
    {
      if (this.GetSwimlane(e) != null)
      {
        this.swimlaneDiagram.HideDragOverVisual();
      }
      else
      {
        if (this.GetMainContainer(e) != null)
          return;
        this.OnDragLeave(new DragItemsEventArgs());
      }
    }

    private void OnHeaderElementMouseLeave(object sender, MouseEventArgs e)
    {
      this.OnHeaderElementLeave();
    }

    private void OnHeaderElementMouseEnter(object sender, MouseEventArgs e)
    {
      this.OnHeaderElementEnter();
    }

    public bool IsPointOverHeaderElement(Point transformedPoint)
    {
      if (this.headerElement == null)
        return false;
      return new Rect(this.Position, new Size(this.headerElement.ActualWidth, this.headerElement.ActualHeight)).Contains(transformedPoint);
    }

    private void OnHeaderElementLeave()
    {
      this.canDrag = false;
      VisualStateManager.GoToState((FrameworkElement) this, "NormalHeader", false);
    }

    private void OnHeaderElementEnter()
    {
      this.canDrag = true;
      VisualStateManager.GoToState((FrameworkElement) this, "MouseOverHeader", false);
    }

    private new Rect GetShapeBounds(Rect contentBounds)
    {
      if (this.headerElement == null)
        return contentBounds;
      if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
      {
        double width = contentBounds.Width + this.headerElement.ActualWidth;
        double height = contentBounds.Height;
        return new Rect(contentBounds.X - this.headerElement.ActualWidth, contentBounds.Y, width, height);
      }
      double width1 = contentBounds.Width;
      double height1 = contentBounds.Height + this.headerElement.ActualHeight;
      return new Rect(contentBounds.X, contentBounds.Y - this.headerElement.ActualHeight, width1, height1);
    }

    private void OnDragInit(object sender, DragInitializeEventArgs e)
    {
      if (this.canDrag)
      {
        e.Data = (object) new DataObject(typeof (SwimlaneShapeBase), (object) this);
        this.Diagram.ServiceLocator.GetService<IToolService>().IsMouseDown = false;
        this.DragStartOffset = e.RelativeStartPoint;
        Rect bounds = new Rect(this.Position, new Size(this.Bounds.Width, this.Height + 1.0));
        WriteableBitmap image1 = this.swimlaneDiagram.CreateImage(bounds);
        Image image2 = new Image();
        image2.Source = (ImageSource) image1;
        image2.Width = bounds.Width;
        image2.Height = bounds.Height;
        Image image3 = image2;
        DragInitializeEventArgs initializeEventArgs = e;
        Border border = new Border();
        border.Child = (UIElement) image3;
        border.Opacity = 0.5;
        initializeEventArgs.DragVisual = (object) border;
        e.AllowedEffects = DragDropEffects.All;
        e.Handled = true;
      }
      else
        e.Cancel = true;
    }

    private void OnDragComplete(object sender, DragDropCompletedEventArgs e)
    {
      if (!this.keepBackground)
        this.OnHeaderElementLeave();
      this.swimlaneDiagram.HideDragOverVisual();
      this.keepBackground = false;
    }

    private Rect GetMinBounds()
    {
      Rect contentBounds = this.GetChildrenBounds();
      if (contentBounds != Rect.Empty)
      {
        contentBounds = this.GetShapeBounds(contentBounds);
        this.MinBoundsWithChildren = contentBounds;
        contentBounds.Intersect(this.Bounds);
      }
      else
        this.MinBoundsWithChildren = contentBounds;
      return contentBounds;
    }
  }
}
