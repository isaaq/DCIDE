using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DC.IDE.UI.Diagram.Table;
using System.Windows.Threading;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;
using Telerik.Windows.Controls.Diagrams.Extensions;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Controls;
using Telerik.Windows.Diagrams.Core;
using Telerik.Windows;
using DC.IDE.UI.Model;
using DC.IDE.UI.Util;

namespace DC.IDE.UI.UC
{
    /// <summary>
    /// UCModelDetail.xaml 的交互逻辑
    /// </summary>
    public partial class UCModelDetail : UserControl
    {
        private FileManager fileManager;
        private TablesGraphSource dc;
        private bool isClear;

        public StructItem StructItem { get; set; }

        static UCModelDetail()
        {
            CommandBinding commandBinding1 = new CommandBinding((System.Windows.Input.ICommand)DiagramCommands.Save, new ExecutedRoutedEventHandler(UCModelDetail.ExecuteSave), new CanExecuteRoutedEventHandler(UCModelDetail.CanExecutedSave));
            CommandBinding commandBinding2 = new CommandBinding((System.Windows.Input.ICommand)DiagramCommands.Open, new ExecutedRoutedEventHandler(UCModelDetail.ExecuteOpen));
            CommandManager.RegisterClassCommandBinding(typeof(UCModelDetail), commandBinding1);
            CommandManager.RegisterClassCommandBinding(typeof(UCModelDetail), commandBinding2);
        }

        public UCModelDetail(StructItem e)
        {
            DiagramConstants.RoutingGridSize = 40d;
            DiagramConstants.ContainerMargin = 0d;
            InitializeComponent();
            StructItem = e;
            this.fileManager = new FileManager(this.diagram);
            this.DataContext = this.dc = new TablesGraphSource();
            var newRouter = new AStarRouter(this.diagram) { WallOptimization = true };
            this.diagram.RoutingService.Router = newRouter;
            this.diagram.RoutingService.FreeRouter = newRouter;
            this.Loaded += this.OnLoaded;
        }

        private static void CanExecutedSave(object sender, CanExecuteRoutedEventArgs e)
        {
            UCModelDetail example = sender as UCModelDetail;
            e.CanExecute = example != null && example.diagram != null && example.diagram.Items.Count > 0;
        }

        private static void ExecuteSave(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as UCModelDetail)?.fileManager.SaveToFile(FileLocation.Disk);
        }

        private static void ExecuteOpen(object sender, ExecutedRoutedEventArgs e)
        {
            UCModelDetail example = sender as UCModelDetail;
            if (example == null)
                return;
            example.ClearDiagram();
            example.fileManager.LoadFromFile(FileLocation.Disk, (Action)null);
        }

        private void OnConnectionManipulationCompleted(object sender, ManipulationRoutedEventArgs e)
        {
            if (e.ManipulationPoint.Type == ManipulationPointType.Intermediate)
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
            //SamplesFactory.LoadSample((RadDiagram)this.diagram, "tableShape");
            if (diagram.GraphSource == null)
                diagram.Clear();
            var shapestr = DiagramXamlGen.Gen(StructItem.ID);
//            shapestr = @"<?xml version=""1.0"" encoding=""utf-8""?>
//<RadDiagram Version=""2013.3"">
//  <Metadata Type=""DC.IDE.UI.Diagram.Table.SqlDiagram"" Id=""e4416f79-3015-4f11-8fc6-b726c1f7be41"" Zoom=""1.07960855960846"" Position=""73;91"" IsSnapEnabled=""false"" RoundedCorners=""True"" Routing=""True"" IsBackgroundSurfaceVisible=""false"">
//    <Title>Diagram [10/4/2013 4:43:42 PM]</Title>
//    <Description></Description>
//  </Metadata>
//  <Groups />
//  <Shapes QNs=""DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"">
//    <RowShape Type=""DC.IDE.UI.Diagram.Table.RowShape"" Id=""cf26a666-a08a-49a9-9e2e-d998d41d3c52"" ZIndex=""5"" Position="""" IsDraggingEnabled=""false"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""DC.IDE.UI.Diagram.Table.RowModel"" Size=""240;30"" RotationAngle=""0"" MinWidth=""0"" MinHeight=""0"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Left:0;0.5,Right:1;0.5,Auto:0.5;0.5,"" UseDefaultConnectors=""True"" QN=""0"" NodeUniqueIdKey=""32422212"" ColumnName=""ProductID"" DataType=""Integer"" MyPosition=""156.193214416504;228.824108123779"" />
//    <RowShape Type=""DC.IDE.UI.Diagram.Table.RowShape"" Id=""ce13187c-6bdd-443d-8272-b2f9aa2cfdcc"" ZIndex=""6"" Position="""" IsDraggingEnabled=""false"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""DC.IDE.UI.Diagram.Table.RowModel"" Size=""240;30"" RotationAngle=""0"" MinWidth=""0"" MinHeight=""0"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Left:0;0.5,Right:1;0.5,Auto:0.5;0.5,"" UseDefaultConnectors=""True"" QN=""0"" NodeUniqueIdKey=""23692060"" ColumnName=""DateOrdered"" DataType=""DateTime"" MyPosition=""156.193214416504;258.824108123779"" />
//    <RowShape Type=""DC.IDE.UI.Diagram.Table.RowShape"" Id=""d01948c2-83c9-41c5-9c28-55469bfe05bd"" ZIndex=""1"" Position="""" IsDraggingEnabled=""false"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""DC.IDE.UI.Diagram.Table.RowModel"" Size=""240;30"" RotationAngle=""0"" MinWidth=""0"" MinHeight=""0"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Left:0;0.5,Right:1;0.5,Auto:0.5;0.5,"" UseDefaultConnectors=""True"" QN=""0"" NodeUniqueIdKey=""4031841"" ColumnName=""OrderID"" DataType=""Integer"" MyPosition=""498.160461425781;103.460567474365"" />
//    <RowShape Type=""DC.IDE.UI.Diagram.Table.RowShape"" Id=""1b98273d-6f88-4d97-9485-4dc88bd7eedd"" ZIndex=""2"" Position="""" IsDraggingEnabled=""false"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""DC.IDE.UI.Diagram.Table.RowModel"" Size=""240;30"" RotationAngle=""0"" MinWidth=""0"" MinHeight=""0"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Left:0;0.5,Right:1;0.5,Auto:0.5;0.5,"" UseDefaultConnectors=""True"" QN=""0"" NodeUniqueIdKey=""33895468"" ColumnName=""CustomerID"" DataType=""String"" MyPosition=""498.160461425781;133.460567474365"" />
//    <RowShape Type=""DC.IDE.UI.Diagram.Table.RowShape"" Id=""ae3aab87-c4c6-43f5-8926-b776200b6fcd"" ZIndex=""2"" Position="""" IsDraggingEnabled=""false"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""DC.IDE.UI.Diagram.Table.RowModel"" Size=""240;30"" RotationAngle=""0"" MinWidth=""0"" MinHeight=""0"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Left:0;0.5,Right:1;0.5,Auto:0.5;0.5,"" UseDefaultConnectors=""True"" QN=""0"" NodeUniqueIdKey=""13234276"" ColumnName=""SalesPersonID"" DataType=""String"" MyPosition=""498.160461425781;163.460567474365"" />
//    <RowShape Type=""DC.IDE.UI.Diagram.Table.RowShape"" Id=""34b0e35a-5ab3-4281-a5ad-55b16acd48e6"" ZIndex=""1"" Position="""" IsDraggingEnabled=""false"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""DC.IDE.UI.Diagram.Table.RowModel"" Size=""240;30"" RotationAngle=""0"" MinWidth=""0"" MinHeight=""0"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Left:0;0.5,Right:1;0.5,Auto:0.5;0.5,"" UseDefaultConnectors=""True"" QN=""0"" NodeUniqueIdKey=""55597689"" ColumnName=""CustomerID"" DataType=""Integer"" MyPosition=""855.422546386719;232.650978088379"" />
//    <RowShape Type=""DC.IDE.UI.Diagram.Table.RowShape"" Id=""b84c2cc5-4315-425a-b360-29bfcdf87ad3"" ZIndex=""2"" Position="""" IsDraggingEnabled=""false"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""DC.IDE.UI.Diagram.Table.RowModel"" Size=""240;30"" RotationAngle=""0"" MinWidth=""0"" MinHeight=""0"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Left:0;0.5,Right:1;0.5,Auto:0.5;0.5,"" UseDefaultConnectors=""True"" QN=""0"" NodeUniqueIdKey=""66606150"" ColumnName=""Name"" DataType=""String"" MyPosition=""855.422546386719;262.650978088379"" />
//    <RowShape Type=""DC.IDE.UI.Diagram.Table.RowShape"" Id=""c9c750ea-a8b9-4f8c-93a7-845b85f3fe86"" ZIndex=""1"" Position="""" IsDraggingEnabled=""false"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""DC.IDE.UI.Diagram.Table.RowModel"" Size=""240;30"" RotationAngle=""0"" MinWidth=""0"" MinHeight=""0"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Left:0;0.5,Right:1;0.5,Auto:0.5;0.5,"" UseDefaultConnectors=""True"" QN=""0"" NodeUniqueIdKey=""15299288"" ColumnName=""EmployeeID"" DataType=""Integer"" MyPosition=""499.667633056641;376.368743896484"" />
//    <RowShape Type=""DC.IDE.UI.Diagram.Table.RowShape"" Id=""7e846955-8da8-4e5b-96ab-8cc434d29dc8"" ZIndex=""2"" Position="""" IsDraggingEnabled=""false"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""DC.IDE.UI.Diagram.Table.RowModel"" Size=""240;30"" RotationAngle=""0"" MinWidth=""0"" MinHeight=""0"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Left:0;0.5,Right:1;0.5,Auto:0.5;0.5,"" UseDefaultConnectors=""True"" QN=""0"" NodeUniqueIdKey=""27878184"" ColumnName=""Name"" DataType=""String"" MyPosition=""499.667633056641;406.368743896484"" />
//    <TableShape Type=""DC.IDE.UI.Diagram.Table.TableShape"" Id=""f75f8599-c79f-4923-8310-5e149005aefc"" ZIndex=""0"" Position="""" IsRotationEnabled=""false"" IsResizingEnabled=""false"" IsConnectorsManipulationEnabled=""false"" Content=""PRODUCTS"" Size=""240;150"" RotationAngle=""0"" MinWidth=""200"" MinHeight=""100"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Auto:0.5;0.5,Left:0;0.5,Top:0.5;0,Right:1;0.5,Bottom:0.5;1,"" UseDefaultConnectors=""True"" RestoredHeight=""0"" RestoredMinHeight=""0"" RestoredIsResizingEnabled=""false"" QN=""0"" Items=""cf26a666-a08a-49a9-9e2e-d998d41d3c52;ce13187c-6bdd-443d-8272-b2f9aa2cfdcc"" NodeUniqueIdKey=""9600450"" MyIsCollapsed=""False"" MyPosition=""156.193214416504;138.824108123779"" />
//    <TableShape Type=""DC.IDE.UI.Diagram.Table.TableShape"" Id=""dce6389d-cd58-48d0-8e98-95f30e381ca6"" ZIndex=""0"" Position="""" IsRotationEnabled=""false"" IsResizingEnabled=""false"" IsConnectorsManipulationEnabled=""false"" Content=""ORDERS"" Size=""240;180"" RotationAngle=""0"" MinWidth=""200"" MinHeight=""100"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Auto:0.5;0.5,Left:0;0.5,Top:0.5;0,Right:1;0.5,Bottom:0.5;1,"" UseDefaultConnectors=""True"" RestoredHeight=""0"" RestoredMinHeight=""0"" RestoredIsResizingEnabled=""false"" QN=""0"" Items=""d01948c2-83c9-41c5-9c28-55469bfe05bd;1b98273d-6f88-4d97-9485-4dc88bd7eedd;ae3aab87-c4c6-43f5-8926-b776200b6fcd"" NodeUniqueIdKey=""22120394"" MyIsCollapsed=""False"" MyPosition=""498.160461425781;13.4605674743652"" />
//    <TableShape Type=""DC.IDE.UI.Diagram.Table.TableShape"" Id=""a5485174-7deb-467d-9aa1-4880565fad1c"" ZIndex=""0"" Position="""" IsRotationEnabled=""false"" IsResizingEnabled=""false"" IsConnectorsManipulationEnabled=""false"" Content=""CUSTOMERS"" Size=""240;150"" RotationAngle=""0"" MinWidth=""200"" MinHeight=""100"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Auto:0.5;0.5,Left:0;0.5,Top:0.5;0,Right:1;0.5,Bottom:0.5;1,"" UseDefaultConnectors=""True"" RestoredHeight=""180"" RestoredMinHeight=""100"" RestoredIsResizingEnabled=""false"" QN=""0"" Items=""34b0e35a-5ab3-4281-a5ad-55b16acd48e6;b84c2cc5-4315-425a-b360-29bfcdf87ad3"" NodeUniqueIdKey=""10468038"" MyIsCollapsed=""False"" MyPosition=""855.422546386719;142.650978088379"" />
//    <TableShape Type=""DC.IDE.UI.Diagram.Table.TableShape"" Id=""0fb57f13-6223-4e4e-97d7-bdfe7b6dd6f6"" ZIndex=""0"" Position="""" IsRotationEnabled=""false"" IsResizingEnabled=""false"" IsConnectorsManipulationEnabled=""false"" Content=""EMPLOYEES"" Size=""240;150"" RotationAngle=""0"" MinWidth=""200"" MinHeight=""100"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Auto:0.5;0.5,Left:0;0.5,Top:0.5;0,Right:1;0.5,Bottom:0.5;1,"" UseDefaultConnectors=""True"" RestoredHeight=""0"" RestoredMinHeight=""0"" RestoredIsResizingEnabled=""false"" QN=""0"" Items=""c9c750ea-a8b9-4f8c-93a7-845b85f3fe86;7e846955-8da8-4e5b-96ab-8cc434d29dc8"" NodeUniqueIdKey=""57189509"" MyIsCollapsed=""False"" MyPosition=""499.667633056641;286.368743896484"" />
//  </Shapes>
//  <Connections QNs=""Telerik.Windows.Controls.Diagrams, Version=2013.3.1008.1050, Culture=neutral, PublicKeyToken=5803cfa389c90ce7;"">
//    <RadDiagramConnection Type=""Telerik.Windows.Controls.RadDiagramConnection"" Id=""933e22e7-334a-4383-bbda-40ebd5604ed8"" ZIndex=""1"" Position=""738.160461425781;178.460567474365"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""Telerik.Windows.Controls.Diagrams.Extensions.ViewModels.LinkViewModelBase`1[Telerik.Windows.Controls.Diagrams.Extensions.ViewModels.NodeViewModelBase]"" SourceConnectorPosition=""Right"" TargetConnectorPosition=""Auto"" SourceCapType=""Arrow5Filled"" TargetCapType=""Arrow2Filled"" SourceCapSize=""5;5"" TargetCapSize=""7;7"" StartPoint=""738.160461425781;178.460567474365"" Source=""ae3aab87-c4c6-43f5-8926-b776200b6fcd"" EndPoint=""739.667633056641;391.368743896484"" Target=""c9c750ea-a8b9-4f8c-93a7-845b85f3fe86"" ConnectionType=""Polyline"" ConnectionPoints=""759.667633056641;178.460567474365;759.667633056641;391.368743896484"" IsModified=""False"" QN=""0"" SourceUniqueId=""13234276"" TargetUniqueId=""15299288"" />
//    <RadDiagramConnection Type=""Telerik.Windows.Controls.RadDiagramConnection"" Id=""45bbef8a-f057-4c34-be22-0885f0bab7ba"" ZIndex=""4"" Position=""396.193214416504;118.460567474365"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""Telerik.Windows.Controls.Diagrams.Extensions.ViewModels.LinkViewModelBase`1[Telerik.Windows.Controls.Diagrams.Extensions.ViewModels.NodeViewModelBase]"" SourceConnectorPosition=""Right"" TargetConnectorPosition=""Auto"" SourceCapType=""Arrow5Filled"" TargetCapType=""Arrow2Filled"" SourceCapSize=""5;5"" TargetCapSize=""7;7"" StartPoint=""396.193214416504;243.824108123779"" Source=""cf26a666-a08a-49a9-9e2e-d998d41d3c52"" EndPoint=""498.160461425781;118.460567474365"" Target=""d01948c2-83c9-41c5-9c28-55469bfe05bd"" ConnectionType=""Polyline"" ConnectionPoints=""478.160461425781;243.824108123779;478.160461425781;118.460567474365"" IsModified=""False"" QN=""0"" SourceUniqueId=""32422212"" TargetUniqueId=""4031841"" />
//    <RadDiagramConnection Type=""Telerik.Windows.Controls.RadDiagramConnection"" Id=""b84c7fa0-18fd-436f-8419-82beef610871"" ZIndex=""1"" Position=""396.193214416504;273.824108123779"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""Telerik.Windows.Controls.Diagrams.Extensions.ViewModels.LinkViewModelBase`1[Telerik.Windows.Controls.Diagrams.Extensions.ViewModels.NodeViewModelBase]"" SourceConnectorPosition=""Right"" TargetConnectorPosition=""Left"" SourceCapType=""Arrow5Filled"" TargetCapType=""Arrow2Filled"" SourceCapSize=""5;5"" TargetCapSize=""7;7"" StartPoint=""396.193214416504;273.824108123779"" Source=""ce13187c-6bdd-443d-8272-b2f9aa2cfdcc"" EndPoint=""499.667633056641;391.368743896484"" Target=""c9c750ea-a8b9-4f8c-93a7-845b85f3fe86"" ConnectionType=""Polyline"" ConnectionPoints=""479.667633056641;273.824108123779;479.667633056641;391.368743896484"" IsModified=""False"" QN=""0"" SourceUniqueId=""23692060"" TargetUniqueId=""15299288"" />
//    <RadDiagramConnection Type=""Telerik.Windows.Controls.RadDiagramConnection"" Id=""81d70c4f-e16d-47d1-a14f-4eb7f5475840"" ZIndex=""0"" Position=""738.160461425781;148.460567474365"" IsRotationEnabled=""false"" IsResizingEnabled=""false"" Content=""Telerik.Windows.Controls.Diagrams.Extensions.ViewModels.LinkViewModelBase`1[Telerik.Windows.Controls.Diagrams.Extensions.ViewModels.NodeViewModelBase]"" SourceConnectorPosition=""Right"" TargetConnectorPosition=""Left"" SourceCapType=""Arrow5Filled"" TargetCapType=""Arrow2Filled"" SourceCapSize=""5;5"" TargetCapSize=""7;7"" StartPoint=""738.160461425781;148.460567474365"" Source=""1b98273d-6f88-4d97-9485-4dc88bd7eedd"" EndPoint=""855.422546386719;247.650978088379"" Target=""34b0e35a-5ab3-4281-a5ad-55b16acd48e6"" ConnectionType=""Polyline"" ConnectionPoints=""835.422546386719;148.460567474365;835.422546386719;247.650978088379"" IsModified=""False"" QN=""0"" SourceUniqueId=""33895468"" TargetUniqueId=""55597689"" />
//  </Connections>
//</RadDiagram>
//";

            diagram.Load(shapestr);
            Action action = () => diagram.AutoFit();
            diagram.Dispatcher.BeginInvoke(action, DispatcherPriority.ApplicationIdle, null);
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
                if (command.Commands.Count() <= 0)
                    return;
                this.diagram.UndoRedoService.ExecuteCommand(command, (object)null);
            }
            else
            {
                DiagramTableModel tableModel = e.OldItems.ElementAt<object>(0) as DiagramTableModel;
                if (tableModel == null)
                    return;
                foreach (NodeViewModelBase internalItem in (Collection<NodeViewModelBase>)tableModel.InternalItems)
                    this.RemoveRowModel(internalItem as RowModel, command);
                if (command.Commands.Count() > 0)
                    this.diagram.UndoRedoService.ExecuteCommand(command, (object)null);
                tableModel.InternalItems.Clear();
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
                command.AddCommand(new UndoableDelegateCommand("Remove link", (Action<object>)(o => this.dc.RemoveLink(link)), (Action<object>)(o => this.dc.AddLink(link)), (Predicate<object>)null));
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
