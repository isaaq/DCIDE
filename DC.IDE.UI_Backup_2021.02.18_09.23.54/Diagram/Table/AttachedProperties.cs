// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.AttachedProperties
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Table
{
  public static class AttachedProperties
  {
    public static readonly DependencyProperty CustomConnectorsProperty = DependencyProperty.RegisterAttached("CustomConnectors", typeof (ConnectorCollection), typeof (AttachedProperties), new PropertyMetadata((object) null, new PropertyChangedCallback(AttachedProperties.OnCustomConnectorsChanged)));

    public static ConnectorCollection GetCustomConnectors(DependencyObject obj)
    {
      return (ConnectorCollection) obj.GetValue(AttachedProperties.CustomConnectorsProperty);
    }

    public static void SetCustomConnectors(DependencyObject obj, ConnectorCollection value)
    {
      obj.SetValue(AttachedProperties.CustomConnectorsProperty, (object) value);
    }

    private static void OnCustomConnectorsChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      IShape shape = d as IShape;
      if (shape == null || e.NewValue == null)
        return;
      shape.Connectors.Clear();
      foreach (RadDiagramConnector diagramConnector1 in (Collection<IConnector>) (e.NewValue as ConnectorCollection))
      {
        ConnectorCollection connectors = shape.Connectors;
        RadDiagramConnector diagramConnector2 = new RadDiagramConnector();
        diagramConnector2.Offset = diagramConnector1.Offset;
        diagramConnector2.Name = diagramConnector1.Name;
        diagramConnector2.Opacity = diagramConnector1.Opacity;
        connectors.Add((IConnector) diagramConnector2);
      }
    }
  }
}
