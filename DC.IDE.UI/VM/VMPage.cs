using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DC.IDE.UI.Common;
using DC.IDE.UI.Data;
using DC.IDE.UI.Model;
using DC.IDE.UI.UC.PageList;
using DC.IDE.UI.Util;
using MongoDB.Bson;
using MongoDB.Driver;
using PostSharp.Patterns.Model;
using Telerik.Windows.Controls;
using ToastNotifications.Messages;

namespace DC.IDE.UI.VM
{
    [NotifyPropertyChanged]
    public class VMPage : ViewModelBase, IRequestCloseViewModel
    {
        public ObservableCollection<PageItem> PageList { get; set; }
        public ObservableCollection<PageItem> MasterList { get; set; }
        public ObservableCollection<PageItem> UCList { get; set; }

        public DelegateCommand SavePageCommand { get; set; }
        public DelegateCommand AddPageCommand { get; set; }
        public DelegateCommand DelPageCommand { get; set; }
        
        public DelegateCommand FormCommand { get; set; }

        public PageItem SelItem { get; set; }
        public string SelType { get; set; }
        public int CurrentPosition { get; internal set; }

        public VMPage()
        {
            PageList = GetList("sys_pages");
            MasterList = GetList("sys_masterpages");
            UCList = GetList("sys_usercontrols");

            SavePageCommand = new DelegateCommand(SavePage);
            AddPageCommand = new DelegateCommand(AddPage);
            DelPageCommand = new DelegateCommand(DelPage);

            FormCommand = new DelegateCommand(InsertForm);

        }

        public event EventHandler RequestClose;
        public event EventHandler<int> ElementInserted;

        private void InsertForm(object obj)
        {
            var win = new InsertFormWindow();
            win.ShowDialog();
            var content = win.InsertContent;
            var cntbyte = Convert.FromBase64String(SelItem.Content);
            var cnt = Encoding.UTF8.GetString(cntbyte);
            var pos = CurrentPosition;
            cnt = cnt.Substring(0, CurrentPosition) + content + cnt.Substring(CurrentPosition);
            SelItem.Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(cnt));
            if(content != null)
                ElementInserted(this, pos + content.Length);
        }

        private void DelPage(object obj)
        {
            var result = MessageBox.Show("您确定要删除吗?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
                var tblname = "sys_" + SelItem.Type;
                var tbl = db.GetTable(tblname);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", SelItem.Id);
                var delresult = tbl.DeleteOne(filter);
                if (delresult.DeletedCount > 0)
                {
                    CommonHelper.GetNotify().ShowSuccess("删除成功!");

                    PageList = GetList("sys_pages");
                    MasterList = GetList("sys_masterpages");
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
            var tblname = "sys_" + SelItem.Type;
            var tbl = db.GetTable(tblname);
            var document = new BsonDocument();
            document["title"] = SelItem.Title;
            document["content"] = SelItem.Content ?? "";
            tbl.InsertOne(document);

            Close();
            PageList = GetList("sys_pages");
            MasterList = GetList("sys_masterpages");
            UCList = GetList("sys_usercontrols");
        }

        private void SavePage(object obj)
        {
            if (SelItem != null)
            {
                var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
                var tblname = "sys_" + SelItem.Type;
                var tbl = db.GetTable(tblname);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", SelItem.Id);
                var update = Builders<BsonDocument>.Update.Set("title", SelItem.Title).Set("content", SelItem.Content);
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

        private ObservableCollection<PageItem> GetList(string tblname)
        {
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var t = m.GetTable(tblname);
            var all = t.FindAll();
            var list = all.Select(s => new PageItem() { Id = s["_id"].AsObjectId, Content = s["content"].AsString, Title = s["title"].AsString, Type = tblname.Replace("sys_", "") }).ToList();
            return new ObservableCollection<PageItem>(list);
        }

        public void Close()
        {
            RequestClose?.Invoke(this, new EventArgs());
        }
    }
}
