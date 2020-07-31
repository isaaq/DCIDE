using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using DC.IDE.UI.Model;
using DC.IDE.UI.Util;

using PostSharp.Patterns.Model;

using Telerik.Windows.Controls;

namespace DC.IDE.UI.VM
{
    [NotifyPropertyChanged]
    public class VMStruct : ViewModelBase
    {
        public ObservableCollection<StructItem> StructItems { get; set; }
        public DelegateCommand EditCommand { get; set; }

        public VMStruct()
        {
            StructItems = new ObservableCollection<StructItem>();
            EditCommand = new DelegateCommand(Edit);
            BuildData(1);
        }

        private void Edit(object obj)
        {
           
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
                default:
                    BuildModel(); break;
            }


        }

        private void BuildModel()
        {
            var t = M.GetDB("dc_c_" + Application.Current.Properties["Company"]).GetTable("sys_tables");
            var r = t.FindAll();
            StructItems.Clear();
            foreach (var i in r)
            {
                var item = new StructItem();
                item.ID = i["_id"].AsObjectId;
                item.Name = i["name"].ToString();
                item.Fields = i["attribute"].AsBsonArray;
                if (i.Contains("modifytime"))
                    item.ModifyTime = i["modifytime"].ToLocalTime();
                if (i.Contains("type"))
                    item.Type = i["type"].ToString();
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
                StructItems.Add(new StructItem() { ID = i["_id"].AsObjectId, Name = i["name"].ToString(), Fields = i["fields"].AsBsonArray, ModifyTime = i["modifytime"].ToLocalTime(), Type = i["type"].ToString(), Description = i["description"].ToString() });
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
            BuildData(int.Parse(uid));
        }
    }
}
