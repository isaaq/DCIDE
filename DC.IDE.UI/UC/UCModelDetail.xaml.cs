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
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using Telerik.Windows.Controls.SyntaxEditor.Taggers;
using Telerik.Windows.Controls.SyntaxEditor.Tagging.Taggers;
using DC.IDE.UI.Model.Field;

namespace DC.IDE.UI.UC
{
    /// <summary>
    /// UCModelDetail.xaml 的交互逻辑
    /// </summary>
    public partial class UCModelDetail : UserControl
    {
        private FileManager fileManager;
        public TablesGraphSource dc { get; set; }
        private bool isClear;
        private CSharpFoldingTagger foldingTagger;
        private TextSearchHighlightTagger selectionWordTagger;

        public StructItem StructItem { get; set; }

        public static readonly DependencyProperty ViewNameProperty =
           DependencyProperty.Register("ViewName", typeof(string), typeof(UCModelDetail), new FrameworkPropertyMetadata(
            null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string ViewName
        {
            get
            {
                return (string)this.GetValue(ViewNameProperty);
            }
            set
            {
                this.SetValue(ViewNameProperty, value);
            }
        }

        static UCModelDetail()
        {
            CommandBinding commandBinding1 = new CommandBinding(DiagramCommands.Save, new ExecutedRoutedEventHandler(ExecuteSave), new CanExecuteRoutedEventHandler(CanExecutedSave));
            CommandBinding commandBinding2 = new CommandBinding(DiagramCommands.Open, new ExecutedRoutedEventHandler(ExecuteOpen));
            CommandManager.RegisterClassCommandBinding(typeof(UCModelDetail), commandBinding1);
            CommandManager.RegisterClassCommandBinding(typeof(UCModelDetail), commandBinding2);
        }

        public UCModelDetail(StructItem e)
        {
            DiagramConstants.RoutingGridSize = 40d;
            DiagramConstants.ContainerMargin = 0d;
            InitializeComponent();
            StructItem = e;
            this.DataContext = this;
            this.fileManager = new FileManager(this.diagram);
            this.dc = new TablesGraphSource();
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
            //var shapestr = DiagramXamlGen.Gen(StructItem.ID);

            //diagram.Load(shapestr);
            BindData(StructItem.ID);
            Action action = () => diagram.AutoFit();
            diagram.Dispatcher.BeginInvoke(action, DispatcherPriority.ApplicationIdle, null);
        }

        private void BindData(ObjectId id)
        {
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var t = m.GetTable("sys_views");
            var f = t.Filter(x => x.Eq("_id", id));
            var r = t.Find(f);
            var item = r.FirstOrDefault();
            ViewName = item["name"].AsString;
            var code = Encoding.UTF8.GetString(Convert.FromBase64String(item["code"].AsString));
            editor.Document = new Telerik.Windows.SyntaxEditor.Core.Text.TextDocument(code);

            var hb = new StringBuilder();
            foreach (var i in item["_tables"].AsBsonArray)
            {
                var tm = new DiagramTableModel();
                tm.Content = i["name"].AsString;
                var posarr = i["pos"].AsBsonArray;
                tm.Position = new Point(posarr[0].AsDouble, posarr[1].AsDouble);
                if (i.AsBsonDocument.Contains("attribute"))
                {
                    var count = 0;
                    foreach (var field in i["attribute"].AsBsonArray)
                    {
                        var rm = new RowModel();
                        rm.ColumnName = field["Name"].AsString;
                        rm.DataType = (FieldType)field["Type"].AsInt32;
                        rm.Position = new Point(tm.Position.X, tm.Position.Y + 90.0 + count * 30);
                        tm.InternalItems.Add(rm);
                        count++;
                    }
                }
                dc.AddNode(tm);
            }
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
            RowModel rowModel = e.OldItems.ElementAt(0) as RowModel;
            CompositeCommand command = new CompositeCommand("Remove Connections");
            if (rowModel != null)
            {
                this.RemoveRowModel(rowModel, command);
                if (command.Commands.Count() <= 0)
                    return;
                this.diagram.UndoRedoService.ExecuteCommand(command, null);
            }
            else
            {
                DiagramTableModel tableModel = e.OldItems.ElementAt(0) as DiagramTableModel;
                if (tableModel == null)
                    return;
                foreach (NodeViewModelBase internalItem in tableModel.InternalItems)
                    this.RemoveRowModel(internalItem as RowModel, command);
                if (command.Commands.Count() > 0)
                    this.diagram.UndoRedoService.ExecuteCommand(command);
                tableModel.InternalItems.Clear();
            }
        }

        private void RemoveRowModel(RowModel rowModel, CompositeCommand command)
        {
            IShape shape = this.diagram.ContainerGenerator.ContainerFromItem(rowModel) as IShape;
            if (shape == null)
                return;
            foreach (IConnection connection in this.diagram.GetConnectionsForShape(shape).ToList<IConnection>())
            {
                LinkViewModelBase<NodeViewModelBase> link = this.diagram.ContainerGenerator.ItemFromContainer(connection) as LinkViewModelBase<NodeViewModelBase>;
                command.AddCommand(new UndoableDelegateCommand("Remove link", (o => this.dc.RemoveLink(link)),(o => this.dc.AddLink(link)), (Predicate<object>)null));
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.foldingTagger = new CSharpFoldingTagger(this.editor);
            this.selectionWordTagger = new TextSearchHighlightTagger(this.editor, TextSearchHighlightTagger.SearchFormatDefinition);
            this.editor.TaggersRegistry.RegisterTagger(foldingTagger);
            this.editor.TaggersRegistry.RegisterTagger(this.selectionWordTagger);

        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var t = m.GetTable("sys_views");
            var f = t.Filter(x => x.Eq("_id", StructItem.ID));
            var r = t.Find(f);
            var old = r.FirstOrDefault();
            old["name"] = ViewName;
            old["code"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(editor.Document.CurrentSnapshot.GetText()));

            var newtables = new BsonArray();
            foreach (var item in dc.Items)
            {
                var tbl = new BsonDocument();
                if (item is DiagramTableModel)
                {
                    var tm = (DiagramTableModel)item;
                    tbl["name"] = tm.Content.ToString();
                    var pos = new BsonArray();
                    pos.Add(tm.Position.X);
                    pos.Add(tm.Position.Y);

                    tbl["pos"] = pos;
                    var attrs = new BsonArray();
                    var attritems = tm.InternalItems;
                    foreach (var attritem in attritems)
                    {
                        var row = (RowModel)attritem;
                        var attr = new BsonDocument();
                        attr["Name"] = row.ColumnName;
                        attr["Type"] = row.DataType;
                        attrs.Add(attr);
                    }
                    tbl["attribute"] = attrs;
                    newtables.Add(tbl);
                }
            }
            old["_tables"] = newtables;
            //var update = Builders<BsonDocument>.Update.Set("_tables", newtables);
            t.ReplaceOne(f, old);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var dtm = new DiagramTableModel();
            dtm.Content = "new";
            dc.AddNode(dtm);
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            var obj = this.diagram.SelectedItem;
            dc.RemoveItem((NodeViewModelBase)obj);
        }

        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            RefreshDiagram();
        }
    }
}
