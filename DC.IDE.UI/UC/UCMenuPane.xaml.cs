using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

using DC.IDE.UI.Util;

using MongoDB.Bson;
using MongoDB.Driver;

namespace DC.IDE.UI.UC
{
    /// <summary>
    /// UCMenuPane.xaml 的交互逻辑
    /// </summary>
    public partial class UCMenuPane : UserControl
    {
        public ObservableCollection<Model.MenuItem> MenuList { get; set; }

        public UCMenuPane()
        {
            InitializeComponent();
            this.DataContext = this;

            MenuList = new ObservableCollection<Model.MenuItem>(GetList(null));
        }

        private IEnumerable<Model.MenuItem> GetList(string pid)
        {
            var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var tbl = db.GetTable("sys_menus");
            var filter = Builders<BsonDocument>.Filter.Eq<string>("pid", pid);
            var all = tbl.Find(filter);
            if (all.Count == 0)
                return new ObservableCollection<Model.MenuItem>();
            var list = new ObservableCollection<Model.MenuItem>();
            foreach (var s in all)
            {
                var mi = new Model.MenuItem();
                mi.Name = s["title"].AsString;
                mi.Title = s["title"].AsString;
                mi.Namespace = s["namespace"].AsString;
                if (s.Contains("url"))
                    mi.Url = s["url"].AsString;
                mi.Children = GetList(s["_id"].ToString());
                list.Add(mi);
            }
            return list;
        }
    }
}
