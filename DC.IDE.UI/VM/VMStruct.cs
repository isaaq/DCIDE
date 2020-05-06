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
        public VMStruct()
        {
            StructItems = new ObservableCollection<StructItem>();
            BuildData(1);

        }

        private void BuildData(int type)
        {
            var t = M.GetDB("dc_c_" + Application.Current.Properties["Company"]).GetTable("models");
            var f = t.Filter(x => x.Eq("type", type));
            var r = t.Find(f);
            StructItems.Clear();
            foreach (var i in r)
            {
                StructItems.Add(new StructItem() { ID = i["_id"].AsObjectId, Name = i["name"].ToString(), Count = i["count"].ToString(), ModifyTime = i["modifytime"].ToLocalTime(), Type = i["type"].ToString(), Description = i["description"].ToString() });
            }
        }

        internal void Select(string uid)
        {
            BuildData(int.Parse(uid));
        }
    }
}
