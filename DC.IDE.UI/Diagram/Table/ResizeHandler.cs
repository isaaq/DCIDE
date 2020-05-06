// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.ResizeHandler
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DC.IDE.UI.Diagram.Table
{
  public class ResizeHandler : Control
  {
    private Point? startPosition;
    private double topShapeStart;
    private double start;
    private double max;
    private double min;

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);
      this.TopShape.UpdateMinBounds();
      this.BottomShape.UpdateMinBounds();
      if (this.Orientation == Orientation.Vertical)
      {
        Rect rect;
        double num1;
        if (!(this.TopShape.MinBounds != Rect.Empty))
        {
          rect = this.TopShape.Bounds;
          num1 = rect.Top + CustomResizingService.MinShapeHeight;
        }
        else
        {
          rect = this.TopShape.MinBounds;
          num1 = rect.Bottom;
        }
        this.min = num1;
        double num2;
        if (!(this.BottomShape.MinBounds != Rect.Empty))
        {
          rect = this.BottomShape.Bounds;
          num2 = rect.Bottom - CustomResizingService.MinShapeHeight;
        }
        else
        {
          rect = this.BottomShape.MinBounds;
          num2 = rect.Top;
        }
        this.max = num2;
        rect = this.TopShape.Bounds;
        this.topShapeStart = rect.Bottom;
        this.start = this.Y;
      }
      else
      {
        Rect rect;
        double num1;
        if (!(this.TopShape.MinBounds != Rect.Empty))
        {
          rect = this.TopShape.Bounds;
          num1 = rect.Left + CustomResizingService.MinShapeWidth;
        }
        else
        {
          rect = this.TopShape.MinBounds;
          num1 = rect.Right;
        }
        this.min = num1;
        double num2;
        if (!(this.BottomShape.MinBounds != Rect.Empty))
        {
          rect = this.BottomShape.Bounds;
          num2 = rect.Right - CustomResizingService.MinShapeWidth;
        }
        else
        {
          rect = this.BottomShape.MinBounds;
          num2 = rect.Left;
        }
        this.max = num2;
        rect = this.TopShape.Bounds;
        this.topShapeStart = rect.Right;
        this.start = this.X;
      }
      this.CaptureMouse();
      this.startPosition = new Point?(e.GetPosition((IInputElement) null));
      this.TopShape.IsInnerResize = true;
      this.BottomShape.IsInnerResize = true;
      e.Handled = true;
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonUp(e);
      this.ReleaseMouseCapture();
      this.startPosition = new Point?();
      this.TopShape.IsInnerResize = false;
      this.BottomShape.IsInnerResize = false;
      e.Handled = true;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      if (!this.startPosition.HasValue)
        return;
      Point position = e.GetPosition((IInputElement) null);
      if (this.Orientation == Orientation.Vertical)
      {
        double num1 = Math.Min(Math.Max(position.Y - this.startPosition.Value.Y, this.min - this.topShapeStart), this.max - this.topShapeStart);
        double num2 = this.topShapeStart + num1;
        this.TopShape.Height = num2 - this.TopShape.Position.Y;
        this.BottomShape.Height = this.BottomShape.Bounds.Bottom - num2 + this.BottomShape.BorderThickness.Top;
        this.BottomShape.Position = new Point(this.BottomShape.Position.X, num2 - this.BottomShape.BorderThickness.Top);
        this.Y = this.start + num1;
      }
      else
      {
        double num1 = Math.Min(Math.Max(position.X - this.startPosition.Value.X, this.min - this.topShapeStart), this.max - this.topShapeStart);
        double num2 = this.topShapeStart + num1;
        this.TopShape.Width = num2 - this.TopShape.Position.X;
        this.BottomShape.Width = this.BottomShape.Bounds.Right - num2 + this.BottomShape.BorderThickness.Left;
        this.BottomShape.Position = new Point(num2 - this.BottomShape.BorderThickness.Left, this.BottomShape.Position.Y);
        this.X = this.start + num1;
      }
    }

    public double X
    {
      get
      {
        return Canvas.GetLeft((UIElement) this);
      }
      set
      {
        Canvas.SetLeft((UIElement) this, value);
      }
    }

    public double Y
    {
      get
      {
        return Canvas.GetTop((UIElement) this);
      }
      set
      {
        Canvas.SetTop((UIElement) this, value);
      }
    }

    public SwimlaneShapeBase TopShape { get; set; }

    public SwimlaneShapeBase BottomShape { get; set; }

    public Orientation Orientation { get; set; }
  }
}
