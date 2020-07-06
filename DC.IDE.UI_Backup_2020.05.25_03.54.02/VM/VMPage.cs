using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DC.IDE.UI.Data;
using DC.IDE.UI.Model;
using DC.IDE.UI.Util;
using MongoDB.Bson;
using MongoDB.Driver;
using PostSharp.Patterns.Model;
using Telerik.Windows.Controls;

namespace DC.IDE.UI.VM
{
    [NotifyPropertyChanged]
    public class VMPage : ViewModelBase, IRequestCloseViewModel
    {
        public ObservableCollection<PageItem> PageList { get; set; }
        public ObservableCollection<PageItem> MasterList { get; set; }

        public DelegateCommand SavePageCommand { get; set; }
        public DelegateCommand AddPageCommand { get; set; }

        public PageItem SelItem { get; set; }

        public VMPage()
        {
            PageList = GetList("sys_pages", 0);
            MasterList = GetList("sys_masterpages", 1);

            SavePageCommand = new DelegateCommand(SavePage);
            AddPageCommand = new DelegateCommand(AddPage);
        }

        public event EventHandler RequestClose;

        private void AddPage(object obj)
        {
            var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var tblname = SelItem.Type == 0 ? "sys_pages" : "sys_masterpages";
            var tbl = db.GetTable(tblname);
            var document = new BsonDocument();
            document["title"] = SelItem.Title;
            document["content"] = SelItem.Content;
            tbl.InsertOne(document);

        }

        private void SavePage(object obj)
        {
            var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var tblname = SelItem.Type == 0 ? "sys_pages" : "sys_masterpages";
            var tbl = db.GetTable(tblname);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", SelItem.Id);
            var update = Builders<BsonDocument>.Update.Set("title", SelItem.Title).Set("content",SelItem.Content);
            var result = tbl.UpdateOne(filter, update);
            if(result.ModifiedCount > 0)
            {
                MessageBox.Show("");
            }
        }

        private ObservableCollection<PageItem> GetList(string tblname, int type)
        {
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var t = m.GetTable(tblname);
            var all = t.FindAll();
            var list = all.Select(s => new PageItem() { Id = s["_id"].AsObjectId, Content = s["content"].AsString, Title = s["title"].AsString, Type = type }).ToList();
            return new ObservableCollection<PageItem>(list);
        }

        public void Close()
        {
            RequestClose?.Invoke(this, new EventArgs());
        }
    }
}
