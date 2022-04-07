using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DC.IDE.UI.Model;
using DC.IDE.UI.UC.Form;
using DC.IDE.UI.UC.ModelDesign;
using DC.IDE.UI.Util;

using MongoDB.Bson;

using PostSharp.Patterns.Model;

using Telerik.Windows.Controls;

namespace DC.IDE.UI.VM
{
    [NotifyPropertyChanged]
    public class VMStruct : ViewModelBase
    {
        public ObservableCollection<StructItem> StructItems { get; set; }
        public DelegateCommand EditCommand { get; set; }
        public DelegateCommand AddModelCommand { get; set; }

        public StructItem SelItem { get; set; }
        public string WindowText { get; set; }
        private int _selId;
        private WinModelAdd _winadd;

        public VMStruct()
        {
            StructItems = new ObservableCollection<StructItem>();
            EditCommand = new DelegateCommand(Edit);
            AddModelCommand = new DelegateCommand(AddModel);

            BuildData(1);
        }

        private void AddModel(object obj)
        {
            _winadd = new WinModelAdd();
            _winadd.DataContext = this;
            _winadd.ShowDialog();
            _winadd.Close();
            var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var tbldict = new List<string>() { "sys_models", "", "", "", "", "", "sys_lists", "", "" };
            var tbl = db.GetTable(tbldict[_selId]);
            var document = new BsonDocument();
            document["name"] = WindowText;
            document["content"] = "";
            tbl.InsertOne(document);
            BuildModel();
        }

        private void Edit(object obj)
        {
            SelItem = (StructItem)obj;
            //_selId;
            RadWindow win = null;
            switch (_selId)
            {
                case 0:
                    break;
                case 6:
                case 7: win = new WinFormDesigner(SelItem); break;

            }
            if (win != null)
            {
                win.ShowDialog();
            }
        }

        private void BuildData(int type)
        {
            switch (type)
            {
                case 0:
                    BuildModel(); break;
                case 1:
                    BuildView(); break;
                case 2:
                    BuildTheme(); break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    BuildList(); break;
                case 7:
                    BuildForm(); break;
                case 8:
                    BuildReport(); break;
                //case 9:
                //    BuildRaw(); break;
                default:
                    BuildModel(); break;
            }


        }

        //private void BuildRaw()
        //{
        //    var t = M.GetDB("dc_c_" + Application.Current.Properties["Company"]).GetTable("sys_forms");
        //    var r = t.FindAll();
        //    StructItems.Clear();
        //    foreach (var i in r)
        //    {
        //        var item = new StructItem();
        //        item.ID = i["_id"].AsObjectId;
        //        item.Name = i["name"].ToString();

        //        if (i.Contains("modifytime"))
        //            item.ModifyTime = i["modifytime"].ToLocalTime();
        //        if (i.Contains("description"))
        //            item.Description = i["description"].ToString();
        //        StructItems.Add(item);
        //    }
        //}

        private void BuildReport()
        {
            throw new NotImplementedException();
        }

        private void BuildForm()
        {
            var t = M.GetDB("dc_c_" + Application.Current.Properties["Company"]).GetTable("sys_forms");
            var r = t.FindAll();
            StructItems.Clear();
            foreach (var i in r)
            {
                var item = new StructItem();
                item.ID = i["_id"].AsObjectId;
                item.Name = i["name"].ToString();

                if (i.Contains("modifytime"))
                    item.ModifyTime = i["modifytime"].ToLocalTime();
                if (i.Contains("description"))
                    item.Description = i["description"].ToString();
                StructItems.Add(item);
            }
        }

        private void BuildList()
        {
            var t = M.GetDB("dc_c_" + Application.Current.Properties["Company"]).GetTable("sys_lists");
            var r = t.FindAll();
            StructItems.Clear();
            foreach (var i in r)
            {
                var item = new StructItem();
                item.ID = i["_id"].AsObjectId;
                item.Name = i["name"].ToString();

                if (i.Contains("modifytime"))
                    item.ModifyTime = i["modifytime"].ToLocalTime();
                if (i.Contains("description"))
                    item.Description = i["description"].ToString();
                StructItems.Add(item);
            }
        }

        private void BuildModel()
        {
            var t = M.GetDB("dc_c_" + Application.Current.Properties["Company"]).GetTable("sys_models");
            var r = t.FindAll();
            StructItems.Clear();
            foreach (var i in r)
            {
                var item = new StructItem();
                item.ID = i["_id"].AsObjectId;
                item.Name = i["name"].ToString();
                if (i.Contains("attribute"))
                    item.Fields = i["attribute"].AsBsonArray;
                if (i.Contains("modifytime"))
                    item.ModifyTime = i["modifytime"].ToLocalTime();
                if (i.Contains("type"))
                    item.Type = i["type"].ToString();
                if (i.Contains("description"))
                    item.Description = i["description"].ToString();
                StructItems.Add(item);
            }
        }

        private void BuildView()
        {
            var t = M.GetDB("dc_c_" + Application.Current.Properties["Company"]).GetTable("sys_views");
            var r = t.FindAll();
            StructItems.Clear();
            foreach (var i in r)
            {
                StructItems.Add(new StructItem() { ID = i["_id"].AsObjectId, Name = i["name"].ToString(), Tables = i["_tables"].AsBsonArray });
                //, ModifyTime = i["modifytime"].ToLocalTime(), Type = i["type"].ToString(), Description = i["description"].ToString() });
            }
        }

        private void BuildTheme()
        {
            var t = M.GetDB("dc_c_" + Application.Current.Properties["Company"]).GetTable("sys_concern_querys");
            var r = t.FindAll();
            StructItems.Clear();
            foreach (var i in r)
            {
                StructItems.Add(new StructItem() { ID = i["_id"].AsObjectId, Name = i["name"].ToString(), Fields = i["Fields"].AsBsonArray, ModifyTime = i["modifytime"].ToLocalTime(), Type = i["type"].ToString(), Description = i["description"].ToString() });
            }
        }

        internal void Select(string uid)
        {
            this._selId = int.Parse(uid);
            BuildData(int.Parse(uid));
        }
    }
}
