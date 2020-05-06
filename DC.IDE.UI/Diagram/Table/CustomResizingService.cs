// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.CustomResizingService
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Table
{
  public class CustomResizingService : ResizingService
  {
    public static double MinShapeHeight = 12.0;
    public static double MinShapeWidth = 12.0;
    private bool isFirstChildResize = false;
    private bool restrictResize = false;
    private MainContainerShapeBase mainContainer;
    private CompositeAsyncStateCommand compositeCommand;
    private Rect startBounds;
    private double min;
    private double max;

    public CustomResizingService(RadDiagram diagram)
      : base((IGraphInternal) diagram)
    {
    }

    public override void InitializeResize(
      IEnumerable<IDiagramItem> newSelectedItems,
      double adornerAngle,
      Rect adornerBounds,
      ResizeDirection resizingDirection,
      Point startPoint)
    {
      this.mainContainer = newSelectedItems.FirstOrDefault<IDiagramItem>() as MainContainerShapeBase;
      if (this.mainContainer != null)
        this.mainContainer.UpdateMinBounds();
      base.InitializeResize(newSelectedItems, adornerAngle, adornerBounds, resizingDirection, startPoint);
      if (this.mainContainer == null)
        return;
      SwimlaneShapeBase swimlaneShapeBase1 = this.mainContainer.OrderedChildren.FirstOrDefault<SwimlaneShapeBase>();
      SwimlaneShapeBase swimlaneShapeBase2 = this.mainContainer.OrderedChildren.LastOrDefault<SwimlaneShapeBase>();
      if (swimlaneShapeBase1 != null && swimlaneShapeBase2 != null)
      {
        this.restrictResize = true;
        this.startBounds = this.mainContainer.ContentBounds;
        if (this.mainContainer.ChildrenPositioning == System.Windows.Controls.Orientation.Vertical)
        {
          this.isFirstChildResize = resizingDirection == ResizeDirection.NorthEastSouthWest || resizingDirection == ResizeDirection.NorthWestSouthEast;
          Rect rect;
          double num1;
          if (!(swimlaneShapeBase1.MinBoundsWithChildren != Rect.Empty))
          {
            num1 = swimlaneShapeBase1.Bounds.Bottom - CustomResizingService.MinShapeHeight;
          }
          else
          {
            rect = swimlaneShapeBase1.MinBoundsWithChildren;
            num1 = rect.Top;
          }
          this.min = num1;
          double num2;
          if (!(swimlaneShapeBase2.MinBoundsWithChildren != Rect.Empty))
          {
            rect = swimlaneShapeBase2.Bounds;
            num2 = rect.Top + CustomResizingService.MinShapeHeight;
          }
          else
          {
            rect = swimlaneShapeBase2.MinBoundsWithChildren;
            num2 = rect.Bottom;
          }
          this.max = num2;
        }
        else
        {
          this.isFirstChildResize = resizingDirection == ResizeDirection.SouthWestNorthEast || resizingDirection == ResizeDirection.NorthWestSouthEast;
          Rect rect;
          double num1;
          if (!(swimlaneShapeBase1.MinBoundsWithChildren != Rect.Empty))
          {
            num1 = swimlaneShapeBase1.Bounds.Right - CustomResizingService.MinShapeWidth;
          }
          else
          {
            rect = swimlaneShapeBase1.MinBoundsWithChildren;
            num1 = rect.Left;
          }
          this.min = num1;
          double num2;
          if (!(swimlaneShapeBase2.MinBoundsWithChildren != Rect.Empty))
          {
            rect = swimlaneShapeBase2.Bounds;
            num2 = rect.Left + CustomResizingService.MinShapeWidth;
          }
          else
          {
            rect = swimlaneShapeBase2.MinBoundsWithChildren;
            num2 = rect.Right;
          }
          this.max = num2;
        }
        this.CreateResizeCommand();
      }
      else
        this.restrictResize = false;
    }

    public override void Resize(Point newPoint)
    {
      base.Resize(newPoint);
      if (this.mainContainer == null)
        return;
      Rect newContentBounds = this.mainContainer.GetNewContentBounds(this.mainContainer.Bounds);
      if (this.mainContainer.ChildrenPositioning == System.Windows.Controls.Orientation.Vertical)
      {
        for (int index = 0; index < this.mainContainer.OrderedChildren.Count; ++index)
        {
          SwimlaneShapeBase orderedChild = this.mainContainer.OrderedChildren[index];
          if (!orderedChild.IsCollapsed)
            orderedChild.Width = newContentBounds.Width;
          if (index == 0)
          {
            orderedChild.Height = Math.Max(0.0, orderedChild.Bounds.Bottom - newContentBounds.Top);
            orderedChild.Position = new Point(newContentBounds.X, newContentBounds.Y);
          }
          else
            orderedChild.Position = new Point(newContentBounds.X, orderedChild.Position.Y);
          if (index == this.mainContainer.OrderedChildren.Count - 1)
            orderedChild.Height = Math.Max(0.0, newContentBounds.Bottom - orderedChild.Position.Y);
        }
      }
      else
      {
        for (int index = 0; index < this.mainContainer.OrderedChildren.Count; ++index)
        {
          SwimlaneShapeBase orderedChild = this.mainContainer.OrderedChildren[index];
          if (!orderedChild.IsCollapsed)
            orderedChild.Height = newContentBounds.Height;
          if (index == 0)
          {
            orderedChild.Width = Math.Max(0.0, orderedChild.Bounds.Right - newContentBounds.Left);
            orderedChild.Position = new Point(newContentBounds.X, newContentBounds.Y);
          }
          else
            orderedChild.Position = new Point(orderedChild.Position.X, newContentBounds.Y);
          if (index == this.mainContainer.OrderedChildren.Count - 1)
            orderedChild.Width = Math.Max(0.0, newContentBounds.Right - orderedChild.Position.X);
        }
      }
    }

    public override void CompleteResize(Rect finalBounds, Point mousePosition)
    {
      CompositeAsyncStateCommand asyncStateCommand = this.Graph.ServiceLocator.GetService<IUndoRedoService>().UndoStack.ElementAt<ICommand>(0) as CompositeAsyncStateCommand;
      if (asyncStateCommand != null)
      {
        asyncStateCommand.InsertCommand(0, (ICommand) new UndoableDelegateCommand("Set property", (Action<object>) (o => this.mainContainer.ShouldUpdateChildren = false), (Action<object>) (o => this.mainContainer.ShouldUpdateChildren = true), (Predicate<object>) null), (object) null);
        asyncStateCommand.AddCommand((ICommand) this.compositeCommand);
      }
      base.CompleteResize(finalBounds, mousePosition);
      if (this.mainContainer == null)
        return;
      IEnumerable<object> objects = this.mainContainer.Children.OfType<IShape>().Select<IShape, ShapeInfo>((Func<IShape, ShapeInfo>) (s => new ShapeInfo(s.Position, s.Bounds.ToSize(), s.RotationAngle))).OfType<object>();
      if (this.compositeCommand != null)
        this.compositeCommand.Complete(true, (object) objects);
    }

    protected override Point CalculateNewDelta(Point newPoint)
    {
      Point point = base.CalculateNewDelta(newPoint);
      if (this.mainContainer != null && this.restrictResize)
      {
        if (this.mainContainer.ChildrenPositioning == System.Windows.Controls.Orientation.Vertical)
        {
          if (this.isFirstChildResize)
          {
            if (this.startBounds.Top - point.Y > this.min)
              point = new Point(point.X, Math.Min(0.0, this.startBounds.Top - this.min));
          }
          else if (this.startBounds.Bottom + point.Y < this.max)
            point = new Point(point.X, Math.Min(0.0, this.max - this.startBounds.Bottom));
        }
        else if (this.isFirstChildResize)
        {
          if (this.startBounds.Left - point.X > this.min)
            point = new Point(Math.Min(0.0, this.startBounds.Left - this.min), point.Y);
        }
        else if (this.startBounds.Right + point.X < this.max)
          point = new Point(Math.Min(0.0, this.max - this.startBounds.Right), point.Y);
      }
      return point;
    }

    protected override void UpdateContainers()
    {
    }

    private void CreateResizeCommand()
    {
      IEnumerable<SwimlaneShapeBase> source1 = this.mainContainer.Children.OfType<SwimlaneShapeBase>();
      this.compositeCommand = new CompositeAsyncStateCommand("Resize horizontal containers", (Action<object>) null, (Action<object>) null, (Predicate<object>) null);
      this.compositeCommand.AddCommand((ICommand) new UndoableDelegateCommand("Set property", (Action<object>) (o =>
      {
        if (this.mainContainer == null)
          return;
        this.mainContainer.ShouldUpdateChildren = true;
      }), (Action<object>) (o =>
      {
        if (this.mainContainer == null)
          return;
        this.mainContainer.ShouldUpdateChildren = false;
      }), (Predicate<object>) null), (object) null);
      IEnumerable<AsyncStateCommand> source2 = source1.Select<SwimlaneShapeBase, AsyncStateCommand>((Func<SwimlaneShapeBase, AsyncStateCommand>) (shape => new AsyncStateCommand("Resize horizontal container", (Action<object>) (state =>
      {
        shape.ShouldUpdateChildren = false;
        ShapeInfo shapeInfo = (ShapeInfo) state;
        shape.Width = shapeInfo.Size.Width;
        shape.Height = shapeInfo.Size.Height;
        shape.Position = shapeInfo.Position;
        shape.ShouldUpdateChildren = true;
      }), (Action<object>) (state =>
      {
        shape.ShouldUpdateChildren = false;
        ShapeInfo shapeInfo = (ShapeInfo) state;
        shape.Width = shapeInfo.Size.Width;
        shape.Height = shapeInfo.Size.Height;
        shape.Position = shapeInfo.Position;
        shape.ShouldUpdateChildren = true;
      }), (Predicate<object>) null)));
      IEnumerable<ShapeInfo> source3 = source1.Select<SwimlaneShapeBase, ShapeInfo>((Func<SwimlaneShapeBase, ShapeInfo>) (s => new ShapeInfo(s.Position, s.Bounds.ToSize(), s.RotationAngle)));
      for (int index = 0; index < source2.Count<AsyncStateCommand>(); ++index)
      {
        if (index < source3.Count<ShapeInfo>())
          this.compositeCommand.AddCommand((ICommand) source2.ElementAt<AsyncStateCommand>(index), (object) source3.ElementAt<ShapeInfo>(index));
        else
          this.compositeCommand.AddCommand((ICommand) source2.ElementAt<AsyncStateCommand>(index), (object) null);
      }
    }
  }
}
