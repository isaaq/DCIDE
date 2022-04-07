// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.CustomManipulationAdorner
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls.Diagrams.Primitives;

namespace DC.IDE.UI.Diagram.Table
{
  public class CustomManipulationAdorner : ManipulationAdorner
  {
    public static readonly DependencyProperty AdditionalResizeHandlersProperty = DependencyProperty.Register(nameof (AdditionalResizeHandlers), typeof (ObservableCollection<ResizeHandler>), typeof (CustomManipulationAdorner), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty AdditionalHandlersVisibilityProperty = DependencyProperty.Register(nameof (AdditionalHandlersVisibility), typeof (Visibility), typeof (CustomManipulationAdorner), new PropertyMetadata((object) Visibility.Collapsed));

    public ObservableCollection<ResizeHandler> AdditionalResizeHandlers
    {
      get
      {
        return (ObservableCollection<ResizeHandler>) this.GetValue(CustomManipulationAdorner.AdditionalResizeHandlersProperty);
      }
      set
      {
        this.SetValue(CustomManipulationAdorner.AdditionalResizeHandlersProperty, (object) value);
      }
    }

    public Visibility AdditionalHandlersVisibility
    {
      get
      {
        return (Visibility) this.GetValue(CustomManipulationAdorner.AdditionalHandlersVisibilityProperty);
      }
      set
      {
        this.SetValue(CustomManipulationAdorner.AdditionalHandlersVisibilityProperty, (object) value);
      }
    }

    public CustomManipulationAdorner()
    {
      this.AdditionalResizeHandlers = new ObservableCollection<ResizeHandler>();
    }

    internal void UpdateAdditionalHandlers(
      List<SwimlaneShapeBase> containers,
      Point boundsPosition,
      bool isVisible,
      Orientation orientation)
    {
      int index1 = 0;
      if (isVisible && containers != null)
      {
        for (; index1 < containers.Count - 1; ++index1)
        {
          Rect bounds = containers[index1].Bounds;
          bounds.X -= boundsPosition.X;
          bounds.Y -= boundsPosition.Y;
          Point point = new Point(bounds.Left + bounds.Width / 2.0 - 3.0, bounds.Bottom - 3.0);
          if (orientation == Orientation.Horizontal)
            point = new Point(bounds.Right - 3.0, bounds.Top + bounds.Height / 2.0 - 3.0);
          if (!double.IsNaN(point.X) && !double.IsNaN(point.Y) && !double.IsInfinity(point.X) && !double.IsInfinity(point.Y))
          {
            ResizeHandler resizeHandler;
            if (index1 < this.AdditionalResizeHandlers.Count)
            {
              resizeHandler = this.AdditionalResizeHandlers[index1];
            }
            else
            {
              resizeHandler = new ResizeHandler();
              this.AdditionalResizeHandlers.Add(resizeHandler);
            }
            resizeHandler.X = point.X;
            resizeHandler.Y = point.Y;
            resizeHandler.TopShape = containers[index1];
            resizeHandler.BottomShape = containers[index1 + 1];
            resizeHandler.Orientation = orientation;
            resizeHandler.Cursor = orientation == Orientation.Vertical ? Cursors.SizeNS : Cursors.SizeWE;
          }
        }
      }
      for (int index2 = this.AdditionalResizeHandlers.Count - 1; index2 >= index1; --index2)
        this.AdditionalResizeHandlers.RemoveAt(index2);
    }
  }
}
