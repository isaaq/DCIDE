// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.RowShape
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Table
{
  public class RowShape : RadDiagramShapeBase
  {
    private readonly string connectionToolName = "Connection Tool";
    private bool isButtonDown;
    private Point lastDownPosition;

    protected override void OnMouseLeave(MouseEventArgs e)
    {
      base.OnMouseLeave(e);
      this.isButtonDown = false;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);
      this.lastDownPosition = e.GetPosition((IInputElement) this);
      this.isButtonDown = true;
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonUp(e);
      this.isButtonDown = false;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      if (this.isButtonDown && !this.IsInEditMode && !this.connectionToolName.Equals(this.Diagram.ServiceLocator.GetService<IToolService>().ActiveTool.Name))
      {
        if (this.lastDownPosition.AroundPoint(e.GetPosition((IInputElement) this), 2.0))
          return;
        RadDiagram diagram = this.Diagram as RadDiagram;
        if (diagram != null)
          diagram.SelectedItem = diagram.ContainerGenerator.ItemFromContainer((RadDiagramItem) this);
        this.isButtonDown = false;
        IToolService service = this.Diagram.ServiceLocator.GetService<IToolService>();
        service.ActivateTool(this.connectionToolName);
        service.MouseDown(new PointerArgs(new Point(), new Point(), false));
      }
      else
      {
        this.lastDownPosition = new Point(-100.0, -100.0);
        this.isButtonDown = false;
      }
    }
  }
}
