using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DC.IDE.UI.Model;
using DC.IDE.UI.Util;
using MongoDB.Bson;
using PostSharp.Patterns.Model;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;
using Telerik.Windows.Diagrams.Core;

namespace DC.IDE.UI.VM
{
    [NotifyPropertyChanged]
    public class VMTable : ViewModelBase
    {
        public ObservableGraphSourceBase<TableItem, TableLink> TableItems { get; set; }
        public StructItem StructItem { get; set; }

        public VMTable(StructItem e)
        {
            TableItems = new ObservableGraphSourceBase<TableItem, TableLink>();
            var tables = LoadDB(e.ID);
            foreach(var tbl in tables)
            {
                var ti = new TableItem()
                { Content = tbl["name"].ToString(), Position = new Point(250, 100) };
                TableItems.AddNode(ti);
            }
            


        }

        private static List<BsonDocument> LoadDB(ObjectId _id)
        {
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var t = m.GetTable("models");
            var f = t.Filter(x => x.Eq("_id", _id));
            var r = t.Find(f);
            var item = r.FirstOrDefault();
            var tables = item["tables"];

            var t2 = m.GetTable("tables");
            var f2 = t2.Filter(x => x.In<ObjectId>("_id", tables.AsBsonArray.ToObjectId()));
            var r2 = t2.Find(f2);
            return r2;
        }
    }
}
