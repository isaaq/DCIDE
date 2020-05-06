// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.Table.AdditionalContent
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Controls.Diagrams.Extensions;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram.Table
{
  public class AdditionalContent : Control
  {
    public static readonly DependencyProperty ContextItemProperty = DependencyProperty.Register(nameof (ContextItem), typeof (object), typeof (AdditionalContent), new PropertyMetadata((object) null, new PropertyChangedCallback(AdditionalContent.OnContextItemPropertyChanged)));
    public static readonly DependencyProperty DiagramProperty = DependencyProperty.Register(nameof (Diagram), typeof (RadDiagram), typeof (AdditionalContent), new PropertyMetadata((PropertyChangedCallback) null));
    protected SettingsPane settingsPane;
    protected FrameworkElement addRemove;

    public RadDiagram Diagram
    {
      get
      {
        return (RadDiagram) this.GetValue(AdditionalContent.DiagramProperty);
      }
      set
      {
        this.SetValue(AdditionalContent.DiagramProperty, (object) value);
      }
    }

    public object ContextItem
    {
      get
      {
        return this.GetValue(AdditionalContent.ContextItemProperty);
      }
      set
      {
        this.SetValue(AdditionalContent.ContextItemProperty, value);
      }
    }

    static AdditionalContent()
    {
      CommandManager.RegisterClassCommandBinding(typeof (AdditionalContent), new CommandBinding((System.Windows.Input.ICommand) SwimlaneCommands.AddCommand, new ExecutedRoutedEventHandler(AdditionalContent.OnAddCommand)));
      CommandManager.RegisterClassCommandBinding(typeof (AdditionalContent), new CommandBinding((System.Windows.Input.ICommand) SwimlaneCommands.RemoveCommand, new ExecutedRoutedEventHandler(AdditionalContent.OnRemoveCommand), new CanExecuteRoutedEventHandler(AdditionalContent.OnCanExecuteRemove)));
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.settingsPane = this.GetTemplateChild("settingsPane") as SettingsPane;
      this.addRemove = this.GetTemplateChild("addRemoveButtons") as FrameworkElement;
      if (this.ContextItem == null)
        return;
      this.OnContextItemChanged(this.ContextItem, (object) null);
    }

    protected virtual void OnRemove()
    {
      MainContainerShapeBase contextItem = this.ContextItem as MainContainerShapeBase;
      if (contextItem == null || contextItem.OrderedChildren.Count <= 0)
        return;
      SwimlaneShapeBase swimlaneShapeBase = contextItem.OrderedChildren.LastOrDefault<SwimlaneShapeBase>();
      this.RemoveContainer(contextItem, (RadDiagramShapeBase) swimlaneShapeBase);
    }

    protected virtual void OnAdd()
    {
      MainContainerShapeBase contextItem = this.ContextItem as MainContainerShapeBase;
      if (contextItem == null)
        return;
      if (contextItem.ChildrenPositioning == System.Windows.Controls.Orientation.Vertical)
      {
        MainContainerShapeBase container = contextItem;
        HorizontalSwimlaneShape horizontalSwimlaneShape = new HorizontalSwimlaneShape();
        horizontalSwimlaneShape.ContainerPosition = contextItem.Items.Count;
        this.AddContainer(container, (IShape) horizontalSwimlaneShape);
      }
      else
      {
        MainContainerShapeBase container = contextItem;
        VerticalSwimlaneShape verticalSwimlaneShape = new VerticalSwimlaneShape();
        verticalSwimlaneShape.ContainerPosition = contextItem.Items.Count;
        this.AddContainer(container, (IShape) verticalSwimlaneShape);
      }
    }

    protected virtual void OnContextItemChanged(object newValue, object oldValue)
    {
      if (this.addRemove == null || this.settingsPane == null)
        return;
      if (newValue == null)
      {
        this.settingsPane.Visibility = Visibility.Collapsed;
        this.addRemove.Visibility = Visibility.Collapsed;
        this.Visibility = Visibility.Collapsed;
      }
      else
      {
        this.Visibility = Visibility.Visible;
        if (newValue is MainContainerShapeBase)
          this.addRemove.Visibility = Visibility.Visible;
        else
          this.addRemove.Visibility = Visibility.Collapsed;
        this.settingsPane.Visibility = Visibility.Visible;
      }
    }

    protected virtual void OnCanRemove(CanExecuteRoutedEventArgs e)
    {
      MainContainerShapeBase contextItem = this.ContextItem as MainContainerShapeBase;
      if (contextItem == null || contextItem.OrderedChildren == null || contextItem.OrderedChildren.Count <= 0)
        return;
      e.CanExecute = true;
    }

    private static void OnCanExecuteRemove(object sender, CanExecuteRoutedEventArgs e)
    {
      AdditionalContent additionalContent = sender as AdditionalContent;
      if (additionalContent != null && additionalContent.Diagram != null)
        additionalContent.OnCanRemove(e);
      else
        e.CanExecute = false;
    }

    private static void OnRemoveCommand(object sender, ExecutedRoutedEventArgs e)
    {
      AdditionalContent additionalContent = sender as AdditionalContent;
      if (additionalContent == null || additionalContent.Diagram == null)
        return;
      additionalContent.OnRemove();
    }

    private static void OnAddCommand(object sender, ExecutedRoutedEventArgs e)
    {
      AdditionalContent additionalContent = sender as AdditionalContent;
      if (additionalContent == null || additionalContent.Diagram == null)
        return;
      additionalContent.OnAdd();
    }

    private static void OnContextItemPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      AdditionalContent additionalContent = d as AdditionalContent;
      if (additionalContent == null)
        return;
      additionalContent.OnContextItemChanged(e.NewValue, e.OldValue);
    }

    private void AddContainer(MainContainerShapeBase container, IShape itemToAdd)
    {
      if (container == null || itemToAdd == null)
        return;
      CompositeCommand compositeCommand = new CompositeCommand("Add new container", (Action<object>) null, (Action<object>) null, (Predicate<object>) null);
      if (container.IsCollapsed)
        compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) new UndoableDelegateCommand("Update container", (Action<object>) (o => container.IsCollapsed = false), (Action<object>) (o => container.IsCollapsed = true), (Predicate<object>) null));
      compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) new UndoableDelegateCommand("Add Container", (Action<object>) (o =>
      {
        container.Items.Add((object) itemToAdd);
        container.IsCollapsed = false;
      }), (Action<object>) (o =>
      {
        this.Diagram.RemoveShape(itemToAdd, false);
        container.IsCollapsed = false;
      }), (Predicate<object>) null));
      this.Diagram.UndoRedoService.ExecuteCommand((Telerik.Windows.Diagrams.Core.ICommand) compositeCommand, (object) null);
    }

    private void RemoveContainer(MainContainerShapeBase container, RadDiagramShapeBase itemToRemove)
    {
      if (container == null || itemToRemove == null)
        return;
      CompositeCommand compositeCommand = new CompositeCommand("Remove horizontal container", (Action<object>) null, (Action<object>) null, (Predicate<object>) null);
      if (container.IsCollapsed)
        compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) new UndoableDelegateCommand("Update container", (Action<object>) (o => container.IsCollapsed = false), (Action<object>) (o => container.IsCollapsed = true), (Predicate<object>) null));
      compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) this.CreateRemoveShapeCommand(itemToRemove));
      if (container.IsCollapsed)
        compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) new UndoableDelegateCommand("Update container", (Action<object>) (o => {}), (Action<object>) (o => {}), (Predicate<object>) null));
      this.Diagram.UndoRedoService.ExecuteCommand((Telerik.Windows.Diagrams.Core.ICommand) compositeCommand, (object) null);
    }

    private CompositeCommand CreateRemoveShapeCommand(RadDiagramShapeBase shape)
    {
      if (shape == null)
        return (CompositeCommand) null;
      CompositeCommand compositeCommand = new CompositeCommand("Remove Shapes", (Action<object>) null, (Action<object>) null, (Predicate<object>) null);
      UndoableDelegateCommand undoableDelegateCommand1 = new UndoableDelegateCommand("Remove Shape", (Action<object>) (s => this.Diagram.RemoveShape((IShape) shape, false)), (Action<object>) (s => this.Diagram.AddShape((IShape) shape, new Point?(), false)), (Predicate<object>) null);
      IContainerShape parentContainer = shape.ParentContainer;
      if (parentContainer != null)
      {
        Action<object> execute = (Action<object>) (o => parentContainer.RemoveItem((object) shape));
        Action<object> undo = (Action<object>) (o =>
        {
          if (parentContainer.Items.Contains((object) shape))
            return;
          parentContainer.AddItem((object) shape, new Point?());
        });
        compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) new UndoableDelegateCommand("Remove Item From Container", execute, undo, (Predicate<object>) null));
      }
      foreach (UndoableDelegateCommand undoableDelegateCommand2 in shape.OutgoingLinks.Union<IConnection>(shape.IncomingLinks).ToList<IConnection>().Select<IConnection, UndoableDelegateCommand>((Func<IConnection, UndoableDelegateCommand>) (connection => this.CreateRemoveConnectionCommand(connection))))
        compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) undoableDelegateCommand2);
      compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) undoableDelegateCommand1);
      RadDiagramContainerShape diagramContainerShape = shape as RadDiagramContainerShape;
      if (diagramContainerShape != null)
      {
        for (int index = diagramContainerShape.Items.Count - 1; index >= 0; --index)
        {
          RadDiagramShapeBase shape1 = diagramContainerShape.Items[index] as RadDiagramShapeBase;
          if (shape1 != null)
          {
            compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) this.CreateRemoveShapeCommand(shape1));
          }
          else
          {
            IConnection connection = diagramContainerShape.Items[index] as IConnection;
            if (connection != null && connection.Source == null && connection.Target == null)
              compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) this.CreateRemoveConnectionCommand(diagramContainerShape.Items[index] as IConnection));
          }
        }
      }
      return compositeCommand;
    }

    private UndoableDelegateCommand CreateRemoveConnectionCommand(
      IConnection connection)
    {
      CompositeCommand compositeCommand = new CompositeCommand("Remove Connections", (Action<object>) null, (Action<object>) null, (Predicate<object>) null);
      UndoableDelegateCommand undoableDelegateCommand = new UndoableDelegateCommand("Remove Connection", (Action<object>) (s => this.Diagram.RemoveConnection(connection, false)), (Action<object>) (s => this.Diagram.AddConnection(connection, false)), (Predicate<object>) null);
      compositeCommand.AddCommand((Telerik.Windows.Diagrams.Core.ICommand) undoableDelegateCommand);
      return (UndoableDelegateCommand) compositeCommand;
    }
  }
}
