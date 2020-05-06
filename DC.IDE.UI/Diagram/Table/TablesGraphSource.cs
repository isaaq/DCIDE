// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.TablesGraphSource
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Table
{
  public class TablesGraphSource : SerializableGraphSourceBase<NodeViewModelBase, LinkViewModelBase<NodeViewModelBase>>
  {
    public override string GetNodeUniqueId(NodeViewModelBase node)
    {
      if (node != null)
        return node.GetHashCode().ToString();
      return string.Empty;
    }

    public override void SerializeNode(NodeViewModelBase node, SerializationInfo info)
    {
      base.SerializeNode(node, info);
      RowModel rowModel = node as RowModel;
      if (rowModel != null)
      {
        info["ColumnName"] = (object) rowModel.ColumnName.ToString();
        info["DataType"] = (object) rowModel.DataType.ToString();
      }
      else
      {
        TableModel tableModel = node as TableModel;
        if (tableModel != null)
        {
          info["Content"] = (object) tableModel.Content.ToString();
          info["IsCollapsed"] = (object) null;
          info["MyIsCollapsed"] = (object) tableModel.IsCollapsed.ToString();
        }
      }
      info["Position"] = (object) string.Empty;
      info["MyPosition"] = (object) node.Position.ToInvariant();
    }

    public override NodeViewModelBase DeserializeNode(
      IShape shape,
      SerializationInfo info)
    {
      NodeViewModelBase nodeViewModelBase;
      if (shape is TableShape)
      {
        TableModel tableModel = new TableModel();
        if (info["Content"] != null)
          tableModel.Content = (object) info["Content"].ToString();
        if (info["MyIsCollapsed"] != null)
          tableModel.IsCollapsed = bool.Parse(info["MyIsCollapsed"].ToString());
        nodeViewModelBase = (NodeViewModelBase) tableModel;
      }
      else
      {
        RowModel rowModel = new RowModel();
        if (info["ColumnName"] != null)
          rowModel.ColumnName = info["ColumnName"].ToString();
        if (info["DataType"] != null)
          rowModel.DataType = (DataType) Enum.Parse(typeof (DataType), info["DataType"].ToString(), true);
        nodeViewModelBase = (NodeViewModelBase) rowModel;
      }
      if (info["MyPosition"] != null)
        nodeViewModelBase.Position = Utils.ToPoint(info["MyPosition"].ToString()).Value;
      if (info[this.NodeUniqueIdKey] != null)
      {
        string key = info[this.NodeUniqueIdKey].ToString();
        if (!this.CachedNodes.ContainsKey(key))
          this.CachedNodes.Add(key, nodeViewModelBase);
      }
      return nodeViewModelBase;
    }

    public override void AddNode(NodeViewModelBase node)
    {
      if (this.InternalItems.Contains(node))
        return;
      base.AddNode(node);
    }
  }
}
