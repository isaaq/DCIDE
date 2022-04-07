using System;
using System.Collections.Generic;
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

using DC.IDE.UI.Model;
using DC.IDE.UI.Util;

using MongoDB.Bson;
using MongoDB.Driver;

using Telerik.Windows.Controls;
using Telerik.Windows.Controls.TreeView;
using ToastNotifications.Messages;

namespace DC.IDE.UI.UC.MenuPane
{
    /// <summary>
    /// MenuPageBindWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MenuPageBindWindow  
    {
        public PageItem Page { get; set; }
        public string Url { get; set; }
        public Model.MenuItem Menu { get; set; }
        public MenuPageBindWindow(TreeViewDragDropOptions options)
        {
            InitializeComponent();
            this.DataContext = this;
            Page = options.DraggedItems.OfType<PageItem>().ToList()[0];
            Url = "/v/" + Page.Id.ToString();
            Menu = (Model.MenuItem)options.DropTargetItem.DataContext;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var tbl = db.GetTable("sys_menus");
            var filter = tbl.Filter(f => f.Eq("_id", Menu.Id));
            var update = Builders<BsonDocument>.Update.Set("href", Url);
            var result = tbl.UpdateOne(filter, update);
            if (result.ModifiedCount > 0 || result.MatchedCount > 0)
            {
                CommonHelper.GetNotify().ShowSuccess("保存成功!");
            }
            else
            {
                throw new Exception("保存失败");
            }
        }
    }
}
