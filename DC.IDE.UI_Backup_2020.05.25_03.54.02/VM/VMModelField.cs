﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DC.IDE.UI.Model;
using DC.IDE.UI.Model.Field;
using DC.IDE.UI.Util;
using MongoDB.Bson;
using PostSharp.Patterns.Model;
using Telerik.Windows.Controls;

namespace DC.IDE.UI.VM
{
    [NotifyPropertyChanged]
    public class VMModelField : ViewModelBase
    {
        private StructItem StructItem;

        public ObservableCollection<FieldItem> FieldItems { get; set; }

        public VMModelField(StructItem item)
        {
            this.StructItem = item;
            FieldItems = new ObservableCollection<FieldItem>();
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            //var t = m.GetTable("models");
            //var r = t.Find(t.Filter(f => f.Eq("_id", item.ID))).FirstOrDefault();
            //if (r != null)
            //{
            var t2 = m.GetTable("tables");
            var r = t2.Find(t2.Filter(f => f.Eq("_id", item.ID))).FirstOrDefault();
            if (r != null)
            {
                foreach (var i in r["fields"].AsBsonArray)
                {
                    var fi = BuildItem(i["type"].AsInt32);
                    if (fi != null)
                    {
                        fi.FillFieldItem(i.AsBsonDocument);
                        FieldItems.Add(fi);
                    }
                }
            }
            //}

        }

        public FieldItem BuildItem(int typeval)
        {
            var type = (FieldType)typeval;
            var fullName = Assembly.GetExecutingAssembly().GetName().Name;
            var objectHandle = Activator.CreateInstance(fullName, fullName + ".Model.Field." + type.ToString() + "Field");
            if (objectHandle != null)
            {
                var fi = (FieldItem)objectHandle.Unwrap();
                return fi;
            }
            else
            {
                return null;
            }
        }

    }
}