// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.CustomContainerBase
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Table
{
  public class CustomContainerBase : RadDiagramContainerShape, IContainerShape, IShape, IDiagramItem, ISerializable, ISupportMouseOver, IPropertyChanged, IGroupable, ISupportManipulation, ISupportVirtualization, IContainerChild, ICollapsible, ISupportRotation, IDragDropAware
  {
    private double restoredMinWidth;
    private Popup editPopup;
    private TextBox editTextBox;
    protected Grid headerElement;
    protected Grid editHeaderElement;
    protected bool templateApplied;
    protected bool loaded;
    protected double restoredWidth;
    protected double actualHeaderWidth;
    private bool isRollback;

    public bool ShouldUpdateChildren { get; set; }

    public System.Windows.Controls.Orientation Orientation { get; set; }

    public CustomContainerBase()
    {
      this.Loaded += new RoutedEventHandler(this.OnCustomContainerLoaded);
      this.ShouldUpdateChildren = true;
    }

    public override void OnApplyTemplate()
    {
      FrameworkElement templateChild = this.GetTemplateChild("NormalContentPresenter") as FrameworkElement;
      if (templateChild != null)
        templateChild.SizeChanged += new SizeChangedEventHandler(this.OnHeaderPresenterSizeChanged);
      base.OnApplyTemplate();
      if (this.editTextBox != null)
        this.editTextBox.KeyDown -= new KeyEventHandler(this.OnEditPopupKeyDown);
      this.headerElement = this.GetTemplateChild("PART_headerElement") as Grid;
      this.editHeaderElement = this.GetTemplateChild("PART_editHeaderElement") as Grid;
      this.editPopup = this.GetTemplateChild("PART_editPopup") as Popup;
      this.editTextBox = this.GetTemplateChild("EditTextBox") as TextBox;
      if (this.editPopup != null)
        this.editPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
      if (this.editTextBox != null)
        this.editTextBox.KeyDown += new KeyEventHandler(this.OnEditPopupKeyDown);
      this.templateApplied = true;
      if (!this.loaded)
        return;
      this.ContentBounds = this.GetNewContentBounds(this.Bounds);
    }

    public Rect GetNewContentBounds(Rect shapeBounds)
    {
      if (this.headerElement == null)
        return shapeBounds;
      double containerMargin = DiagramConstants.ContainerMargin;
      if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
      {
        double num1;
        if (this.headerElement == null)
        {
          num1 = 0.0;
        }
        else
        {
          double actualHeight = this.headerElement.ActualHeight;
          Thickness margin = this.headerElement.Margin;
          double top = margin.Top;
          double num2 = actualHeight + top;
          margin = this.headerElement.Margin;
          double bottom = margin.Bottom;
          num1 = num2 + bottom;
        }
        double num3 = num1;
        double width = Math.Max(0.0, shapeBounds.Width - 2.0 * containerMargin);
        double height = Math.Max(0.0, shapeBounds.Height - 2.0 * containerMargin - num3);
        return new Rect(shapeBounds.X + containerMargin, shapeBounds.Y + containerMargin + num3, width, height);
      }
      double d = double.IsNaN(this.headerElement.Width) ? this.actualHeaderWidth : this.headerElement.Width;
      double num = Math.Round(double.IsNaN(d) ? this.headerElement.ActualWidth : d, 1);
      double width1 = Math.Max(0.0, shapeBounds.Width - 2.0 * containerMargin - num);
      double height1 = Math.Max(0.0, shapeBounds.Height - 2.0 * containerMargin);
      return new Rect(shapeBounds.X + containerMargin + num, shapeBounds.Y + containerMargin, width1, height1);
    }

    Rect IContainerShape.ContentBounds
    {
      get
      {
        return this.ContentBounds;
      }
      set
      {
        if (this.Diagram.ServiceLocator.GetService<IRotationService>().IsRotating)
          return;
        this.ContentBounds = value;
      }
    }

    protected virtual void OnHeaderPresenterSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this.loaded)
      {
        FrameworkElement frameworkElement = sender as FrameworkElement;
        if (frameworkElement != null && this.headerElement != null)
          this.actualHeaderWidth = Math.Round(Math.Max(e.NewSize.Height + frameworkElement.Margin.Top + frameworkElement.Margin.Bottom, this.headerElement.MinWidth));
        else
          this.actualHeaderWidth = e.NewSize.Height;
      }
      else
      {
        if (this.headerElement == null)
          return;
        this.actualHeaderWidth = Math.Max(this.headerElement.ActualWidth, this.headerElement.MinWidth);
      }
    }

    protected virtual void OnCustomContainerLoaded(object sender, RoutedEventArgs e)
    {
      if (this.templateApplied)
        this.ContentBounds = this.GetNewContentBounds(this.Bounds);
      this.loaded = true;
    }

    protected override void OnRotationAngleChanged(double newValue, double oldValue)
    {
      if (this.isRollback)
        return;
      this.isRollback = true;
      this.RotationAngle = oldValue;
      this.isRollback = false;
    }

    protected override void UpdateChildrenPositions(Point oldPosition, Point newPosition)
    {
      if (!this.ShouldUpdateChildren)
        return;
      base.UpdateChildrenPositions(oldPosition, newPosition);
    }

    protected override void OnChildBoundsChanged(IDiagramItem diagramItem)
    {
      if (!this.IsNotUserInteration())
        return;
      base.OnChildBoundsChanged(diagramItem);
    }

    protected override void OnIsInEditModeChanged(bool oldIsInEditMode, bool isInEditMode)
    {
      base.OnIsInEditModeChanged(oldIsInEditMode, isInEditMode);
      if (this.editPopup == null)
        return;
      if (isInEditMode)
        this.editPopup.IsOpen = true;
      else
        this.editPopup.IsOpen = false;
    }

    protected override Rect CalculateContentBounds(Rect shapeBounds)
    {
      return this.GetNewContentBounds(shapeBounds);
    }

    protected override Rect CalculateShapeBounds(Rect contentBounds)
    {
      if (this.headerElement == null)
        return contentBounds;
      Rect shapeBounds = this.GetShapeBounds(contentBounds);
      this.SetEditElementPosition(shapeBounds.Height);
      return shapeBounds;
    }

    protected override void OnIsCollapsedChanged(bool newValue, bool oldValue)
    {
      if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
        base.OnIsCollapsedChanged(newValue, oldValue);
      else if (newValue)
      {
        double minHeight = this.MinHeight;
        double height = this.Height;
        base.OnIsCollapsedChanged(newValue, oldValue);
        this.MinHeight = minHeight;
        this.Height = height;
        if (this.MinWidth > 0.0)
        {
          this.restoredMinWidth = this.MinWidth;
          this.SetValue(FrameworkElement.MinWidthProperty, FrameworkElement.MinWidthProperty.GetMetadata(typeof (FrameworkElement)).DefaultValue);
        }
        if (this.Width > 0.0)
        {
          this.restoredWidth = this.Width;
          this.SetValue(FrameworkElement.WidthProperty, FrameworkElement.WidthProperty.GetMetadata(typeof (FrameworkElement)).DefaultValue);
        }
      }
      else
      {
        base.OnIsCollapsedChanged(newValue, oldValue);
        this.Width = this.restoredWidth;
        this.MinWidth = this.restoredMinWidth;
      }
    }

    protected bool IsNotUserInteration()
    {
      return !this.Diagram.ServiceLocator.GetService<IResizingService>().IsResizing && !this.Diagram.ServiceLocator.GetService<IDraggingService>().IsDragging && !this.Diagram.ServiceLocator.GetService<IRotationService>().IsRotating;
    }

    protected SwimlaneShapeBase GetSwimlane(Telerik.Windows.DragDrop.DragEventArgs e)
    {
      SwimlaneShapeBase data1 = (e.Data as DataObject).GetData(typeof (SwimlaneShapeBase)) as SwimlaneShapeBase;
      if (data1 == null)
      {
        object data2 = (e.Data as DataObject).GetData(typeof (DiagramDropInfo));
        if (data2 != null)
          return SerializationService.Default.DeserializeItems(((DiagramDropInfo) data2).Info, true).FirstOrDefault<IDiagramItem>((Func<IDiagramItem, bool>) (i => i is SwimlaneShapeBase)) as SwimlaneShapeBase;
      }
      return data1;
    }

    protected MainContainerShapeBase GetMainContainer(Telerik.Windows.DragDrop.DragEventArgs e)
    {
      MainContainerShapeBase data1 = (e.Data as DataObject).GetData(typeof (MainContainerShapeBase)) as MainContainerShapeBase;
      if (data1 == null)
      {
        object data2 = (e.Data as DataObject).GetData(typeof (DiagramDropInfo));
        if (data2 != null)
          return SerializationService.Default.DeserializeItems(((DiagramDropInfo) data2).Info, true).FirstOrDefault<IDiagramItem>((Func<IDiagramItem, bool>) (i => i is MainContainerShapeBase)) as MainContainerShapeBase;
      }
      return data1;
    }

    protected Rect GetShapeBounds(Rect contentBounds)
    {
      if (this.headerElement == null)
        return contentBounds;
      double containerMargin = DiagramConstants.ContainerMargin;
      if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
      {
        double num1;
        if (this.headerElement == null)
        {
          num1 = 0.0;
        }
        else
        {
          double actualHeight = this.headerElement.ActualHeight;
          Thickness margin = this.headerElement.Margin;
          double top = margin.Top;
          double num2 = actualHeight + top;
          margin = this.headerElement.Margin;
          double bottom = margin.Bottom;
          num1 = num2 + bottom;
        }
        double num3 = num1;
        double width = Math.Max(0.0, contentBounds.Width + 2.0 * containerMargin);
        double height = Math.Max(0.0, contentBounds.Height + 2.0 * containerMargin + num3);
        return new Rect(contentBounds.X - containerMargin, contentBounds.Y - containerMargin - num3, width, height);
      }
      double d = double.IsNaN(this.headerElement.Width) ? this.actualHeaderWidth : this.headerElement.Width;
      double num = Math.Round(double.IsNaN(d) ? this.headerElement.ActualWidth : d, 1);
      double width1 = contentBounds.Width + 2.0 * containerMargin + num;
      double height1 = contentBounds.Height + 2.0 * containerMargin;
      return new Rect(contentBounds.X - containerMargin - num, contentBounds.Y - containerMargin, width1, height1);
    }

    private void SetEditElementPosition(double height)
    {
    }

    private void OnEditPopupKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return && e.Key != Key.Escape)
        return;
      this.IsInEditMode = false;
    }
  }
}
