using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DC.IDE.UI.Diagram.Table;
using DC.IDE.UI.Model;
using DC.IDE.UI.Util;

using MongoDB.Bson;
using PostSharp.Patterns.Model;
using Telerik.Windows.Controls;

namespace DC.IDE.UI.UC.PageList
{
    /// <summary>
    /// InsertFormWindow.xaml 的交互逻辑
    /// </summary>
    [NotifyPropertyChanged]
    public partial class InsertFormWindow
    {
        public ObjectId SelItem { get; set; }
        public ObservableCollection<TableModel> FormList { get; set; }
        
        public string InsertContent { get; set; }

        public InsertFormWindow()
        {
            InitializeComponent();
            this.DataContext = this;
           
            FormList = GetList();
        }

        private ObservableCollection<TableModel> GetList()
        {
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var t = m.GetTable("sys_tables");
            var all = t.FindAll();

            var tmlist = all.Select(s =>
            {
                var fieldlist = new List<FieldModel>();
                foreach (var field in s["attribute"].AsBsonArray)
                {
                    BsonElement value;
                    var pk = field.AsBsonDocument.TryGetElement("is_pk", out value);

                    try
                    {
                        var fieldmodel = new FieldModel()
                        {
                            Name = field["Name"].AsString,
                            Type = field["Type"].AsInt32
                        };
                        fieldlist.Add(fieldmodel);
                    }
                    catch (Exception)
                    {

                    }
                    
                }

                return new TableModel()
                {
                    Name = s["name"].AsString,
                    Description = s["description"].AsString,
                    Fields = fieldlist
                };
            });
            return new ObservableCollection<TableModel>(tmlist);
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            var control = (e.OriginalSource as Control);
            var tag = control.Tag;
            InsertContent = "@form " + tag;

            Close();
        }
    }
}
