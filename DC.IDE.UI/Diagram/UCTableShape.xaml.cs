// Decompiled with JetBrains decompiler
// Type: DC.IDE.UI.Diagram.UCTableShape
// Assembly: DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0D536A62-DD1E-45F0-ABC8-30BF28EFB831
// Assembly location: Y:\codes\win\DCIDE\TestContainer\bin\Debug\DC.IDE.UI.dll

using DC.IDE.UI.Diagram.Table;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Controls.Diagrams.Extensions;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.Diagram
{
    public partial class UCTableShape : UserControl, IComponentConnector
    {
        private FileManager fileManager;
        private TablesGraphSource dc;
        private bool isClear;

        static UCTableShape()
        {
            CommandBinding commandBinding1 = new CommandBinding((System.Windows.Input.ICommand)DiagramCommands.Save, new ExecutedRoutedEventHandler(UCTableShape.ExecuteSave), new CanExecuteRoutedEventHandler(UCTableShape.CanExecutedSave));
            CommandBinding commandBinding2 = new CommandBinding((System.Windows.Input.ICommand)DiagramCommands.Open, new ExecutedRoutedEventHandler(UCTableShape.ExecuteOpen));
            CommandManager.RegisterClassCommandBinding(typeof(UCTableShape), commandBinding1);
            CommandManager.RegisterClassCommandBinding(typeof(UCTableShape), commandBinding2);
        }

        public UCTableShape()
        {
            DiagramConstants.RoutingGridSize = 40d;
            DiagramConstants.ContainerMargin = 0d;
            InitializeComponent();

            this.fileManager = new FileManager(this.diagram);
            this.DataContext = this.dc = new TablesGraphSource();
            var newRouter = new AStarRouter(this.diagram) { WallOptimization = true };
            this.diagram.RoutingService.Router = newRouter;
            this.diagram.RoutingService.FreeRouter = newRouter;
            this.Loaded += this.OnLoaded;
        }

        private static void CanExecutedSave(object sender, CanExecuteRoutedEventArgs e)
        {
            UCTableShape ucTableShape = sender as UCTableShape;
            e.CanExecute = ucTableShape != null && ucTableShape.diagram != null && ucTableShape.diagram.Items.Count > 0;
        }

        private static void ExecuteSave(object sender, ExecutedRoutedEventArgs e)
        {
            UCTableShape ucTableShape = sender as UCTableShape;
            if (ucTableShape == null)
                return;
            ucTableShape.fileManager.SaveToFile(FileLocation.Disk);
        }

        private static void ExecuteOpen(object sender, ExecutedRoutedEventArgs e)
        {
            UCTableShape ucTableShape = sender as UCTableShape;
            if (ucTableShape == null)
                return;
            ucTableShape.ClearDiagram();
            ucTableShape.fileManager.LoadFromFile(FileLocation.Disk, (Action)null);
        }

        private void OnConnectionManipulationCompleted(object sender, ManipulationRoutedEventArgs e)
        {
            if ((uint)e.ManipulationPoint.Type <= 0U)
                return;
            e.Handled = e.Shape == null;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
                return;
            foreach (RadDiagramContainerShape diagramContainerShape in this.diagram.Shapes.OfType<RadDiagramContainerShape>())
            {
                if (diagramContainerShape.IsSelected)
                    diagramContainerShape.ZIndex = 4;
                else
                    diagramContainerShape.ZIndex = 0;
            }
        }

        private void OnPreviewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 1)
                return;
            foreach (object addedItem in (IEnumerable)e.AddedItems)
            {
                if (!(addedItem is RowModel))
                    this.diagram.ContainerGenerator.ContainerFromItem(addedItem).IsSelected = true;
            }
            e.Handled = true;
        }

        private void OnNewClick(object sender, RoutedEventArgs e)
        {
            this.RefreshDiagram();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.RefreshDiagram();
        }

        private void LoadDefaultTables()
        {
            SamplesFactory.LoadSample((RadDiagram)this.diagram, "tableShape");
        }

        private void ClearDiagram()
        {
            this.diagram.UndoRedoService.Clear();
            TablesGraphSource dataContext = this.DataContext as TablesGraphSource;
            if (dataContext == null)
                return;
            dataContext.ClearCache();
            dataContext.Clear();
        }

        private void OnItemsChanging(object sender, DiagramItemsChangingEventArgs e)
        {
            if (this.isClear || e.Action != NotifyCollectionChangedAction.Remove)
                return;
            RowModel rowModel = e.OldItems.ElementAt<object>(0) as RowModel;
            CompositeCommand command = new CompositeCommand("Remove Connections", (Action<object>)null, (Action<object>)null, (Predicate<object>)null);
            if (rowModel != null)
            {
                this.RemoveRowModel(rowModel, command);
                if (command.Commands.Count<Telerik.Windows.Diagrams.Core.ICommand>() > 0)
                    this.diagram.UndoRedoService.ExecuteCommand((Telerik.Windows.Diagrams.Core.ICommand)command, (object)null);
            }
            else
            {
                TableModel tableModel = e.OldItems.ElementAt<object>(0) as TableModel;
                if (tableModel != null)
                {
                    foreach (NodeViewModelBase internalItem in (Collection<NodeViewModelBase>)tableModel.InternalItems)
                        this.RemoveRowModel(internalItem as RowModel, command);
                    if (command.Commands.Count<Telerik.Windows.Diagrams.Core.ICommand>() > 0)
                        this.diagram.UndoRedoService.ExecuteCommand((Telerik.Windows.Diagrams.Core.ICommand)command, (object)null);
                    tableModel.InternalItems.Clear();
                }
            }
        }

        private void RemoveRowModel(RowModel rowModel, CompositeCommand command)
        {
            IShape shape = this.diagram.ContainerGenerator.ContainerFromItem((object)rowModel) as IShape;
            if (shape == null)
                return;
            foreach (IConnection connection in this.diagram.GetConnectionsForShape(shape).ToList<IConnection>())
            {
                LinkViewModelBase<NodeViewModelBase> link = this.diagram.ContainerGenerator.ItemFromContainer((object)connection) as LinkViewModelBase<NodeViewModelBase>;
                command.AddCommand((Telerik.Windows.Diagrams.Core.ICommand)new UndoableDelegateCommand("Remove link", (Action<object>)(o => this.dc.RemoveLink(link)), (Action<object>)(o => this.dc.AddLink(link)), (Predicate<object>)null));
            }
        }

        private void RefreshDiagram()
        {
            this.isClear = true;
            this.diagram.DeselectAll();
            this.ClearDiagram();
            this.LoadDefaultTables();
            this.isClear = false;
        }

        private void OnDiagramDeserialized(object sender, RadRoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (var connection in this.diagram.Connections)
                {
                    connection.Update();
                }

            }), DispatcherPriority.ApplicationIdle);
        }

       
    }
}
