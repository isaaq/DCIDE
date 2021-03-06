using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

using DC.IDE.UI.Model;
using DC.IDE.UI.Model.Webpart;
using DC.IDE.UI.UC.PageList;
using DC.IDE.UI.Util;

using MongoDB.Bson;
using MongoDB.Driver;

using PostSharp.Patterns.Model;

using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.PropertyGrid;

using ToastNotifications.Messages;

namespace DC.IDE.UI.VM
{
    [NotifyPropertyChanged]
    public class VMWebpart : ViewModelBase, IRequestCloseViewModel
    {
        public ObservableCollection<CategoryItem> CategoryList { get; set; }
        public ObservableCollection<WebPartPageModel> WebpartList { get; set; }
        public ObservableCollection<DatasourceListItem> DatasourceList { get; set; }
        public DelegateCommand SavePageCommand { get; set; }
        public DelegateCommand AddPageCommand { get; set; }
        public DelegateCommand DelPageCommand { get; set; }

        public DelegateCommand FormCommand { get; set; }

        public object SelItem { get; set; }
        public string WindowText { get; set; }
        public int CurrentPosition { get; internal set; }
        public string MainContent { get; private set; }

        public event EventHandler RequestClose;
        public event EventHandler<int> ElementInserted;

        public VMWebpart()
        {
            GetList();
            SavePageCommand = new DelegateCommand(SavePage);
            AddPageCommand = new DelegateCommand(AddPage);
            DelPageCommand = new DelegateCommand(DelPage);

            FormCommand = new DelegateCommand(InsertForm);
        }

        private void GetList()
        {
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var t_c = m.GetTable("sys_webpart_categories");
            var categories = t_c.FindAll();
            CategoryList = new ObservableCollection<CategoryItem>();

            foreach (var category in categories)
            {
                var name = category["name"].AsString;

                var t = m.GetTable("sys_webparts");
                var filter = Builders<BsonDocument>.Filter.Eq("category", name);
                var result = t.Find(filter);

                var item = new CategoryItem();
                item.Name = name;
                var arr = new List<string>();
                var list = result.Select(s => new WebPartPageModel()
                {
                    Name = s["name"].AsString,
                    Id = s["_id"].AsObjectId,
                    Category = s["category"].AsString,
                    DataSources = s.Contains("datasources") ? s["datasources"].AsBsonArray : new BsonArray(arr),
                    ManagePageContent = s.Contains("managecontent") ? s["managecontent"].AsString : "",
                    MainContent = s.Contains("maincontent") ? s["maincontent"].AsString : "",
                    IsCode = s.Contains("iscode") ? s["iscode"].AsBoolean : false,
                    Parent = item
                }).ToList();
                var tempcoll = new ObservableCollection<WebPartPageModel>(list);
                item.Children = tempcoll;
                CategoryList.Add(item);
            }
            // TODO 
            //CategoryList.Add();
        }

        private PropModel BuildPropList()
        {
            var pm = new PropModel();

            return pm;
        }

        private void InsertForm(object obj)
        {
            var win = new InsertFormWindow();
            win.ShowDialog();
            if (SelItem is WebPartPageModel Sel)
            {
                var content = win.InsertContent;
                var cntbyte = Convert.FromBase64String(Sel.MainContent);
                var cnt = Encoding.UTF8.GetString(cntbyte);
                var pos = CurrentPosition;
                cnt = cnt.Substring(0, CurrentPosition) + content + cnt.Substring(CurrentPosition);
                Sel.MainContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(cnt));
                ElementInserted(this, pos + content.Length);
            }
        }

        private void DelPage(object obj)
        {
            var result = MessageBox.Show("您确定要删除吗?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (SelItem is WebPartPageModel Sel)
                {
                    var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
                    var tbl = db.GetTable("sys_webparts");
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", Sel.Id);
                    var delresult = tbl.DeleteOne(filter);
                    if (delresult.DeletedCount > 0)
                    {
                        CommonHelper.GetNotify().ShowSuccess("删除成功!");

                        GetList();
                    }
                    else
                    {
                        throw new Exception("删除失败");
                    }
                }
            }
        }

        private void AddPage(object obj)
        {
            var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var tbl = db.GetTable("sys_webparts");
            var document = new BsonDocument();
            document["name"] = WindowText;
            document["maincontent"] = "";
            document["iscode"] = false;
            if (SelItem is WebPartPageModel Sel)
            {
                document["category"] = Sel.Parent.Name;
            }
            if (SelItem is CategoryItem Sel2)
            {
                document["category"] = Sel2.Name;
            }
            tbl.InsertOne(document);
            Close();
            GetList();
        }

        private void SavePage(object obj)
        {
            if (SelItem != null)
            {
                if (SelItem is WebPartPageModel Sel)
                {
                    var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
                    var tbl = db.GetTable("sys_webparts");
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", Sel.Id);
                    var update = Builders<BsonDocument>.Update
                        .Set("name", Sel.Name)
                        .Set("content", Sel.MainContent)
                        .Set("iscode", Sel.IsCode);
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

        public void Close()
        {
            RequestClose?.Invoke(this, new EventArgs());
        }

    }
}
