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
        public DelegateCommand SavePageCommand { get; set; }
        public DelegateCommand AddPageCommand { get; set; }
        public DelegateCommand DelPageCommand { get; set; }

        public DelegateCommand FormCommand { get; set; }

        public WebPartPageModel SelItem { get; set; }
        public int CurrentPosition { get; internal set; }

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
                var list = result.Select(s => new WebPartPageModel()
                {
                    Name = s["name"].AsString,
                    Id = s["_id"].AsObjectId,
                    Category = s["category"].AsString,
                    DataSources = s["datasources"].AsBsonArray,
                    ManagePageContent = s["managecontent"].AsString,
                    MainContent = s["maincontent"].AsString,
                    
                }).ToList();
                 
                //foreach(var m in list)
                //{
                //    m[]
                //}
                var tempcoll = new ObservableCollection<WebPartPageModel>(list);
                var item = new CategoryItem();
                item.Name = name;
                item.Children = tempcoll;
                CategoryList.Add(item);
            }

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
            var content = win.InsertContent;
            var cntbyte = Convert.FromBase64String(SelItem.MainContent);
            var cnt = Encoding.UTF8.GetString(cntbyte);
            var pos = CurrentPosition;
            cnt = cnt.Substring(0, CurrentPosition) + content + cnt.Substring(CurrentPosition);
            SelItem.MainContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(cnt));
            ElementInserted(this, pos + content.Length);
        }

        private void DelPage(object obj)
        {
            var result = MessageBox.Show("您确定要删除吗?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
                var tbl = db.GetTable("sys_webparts");
                var filter = Builders<BsonDocument>.Filter.Eq("_id", SelItem.Id);
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

        private void AddPage(object obj)
        {
            var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var tbl = db.GetTable("sys_webparts");
            var document = new BsonDocument();
            document["name"] = SelItem.Name;
            document["maincontent"] = SelItem.MainContent ?? "";
            tbl.InsertOne(document);

            Close();
            GetList();
        }

        private void SavePage(object obj)
        {
            if (SelItem != null)
            {
                var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
                var tbl = db.GetTable("sys_webparts");
                var filter = Builders<BsonDocument>.Filter.Eq("_id", SelItem.Id);
                var update = Builders<BsonDocument>.Update.Set("name", SelItem.Name).Set("content", SelItem.MainContent);
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

        public void Close()
        {
            RequestClose?.Invoke(this, new EventArgs());
        }

    }
}
