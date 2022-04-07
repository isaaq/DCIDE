using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using PostSharp.Patterns.Model;
using System.Collections.ObjectModel;
using DC.IDE.UI.Util;
using System.Windows;
using Newtonsoft.Json.Linq;
using MongoDB.Bson;
using DC.IDE.UI.Data;

namespace DC.IDE.UI.VM
{
    [NotifyPropertyChanged]
    public class VMSettings : ViewModelBase
    {
        public ObservableCollection<ListItem> ListItems { get; set; }

        public VMSettings()
        {
            ListItems = new ObservableCollection<ListItem>();
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]).GetTable("settings");
            var r = m.FindAll();
            foreach (var i in r)
            {
                ListItems.Add(new ListItem() { Name = i["name"].ToString(), Switch = 0 });
            }

        }


    }
}
