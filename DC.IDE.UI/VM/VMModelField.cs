using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using DC.IDE.UI.Model;
using DC.IDE.UI.Model.Field;
using DC.IDE.UI.Util;

using MongoDB.Bson;
using MongoDB.Driver;

using PostSharp.Patterns.Model;

using Telerik.Windows.Controls;

using ToastNotifications.Messages;

namespace DC.IDE.UI.VM
{
    [NotifyPropertyChanged]
    public class VMModelField : ViewModelBase
    {
        private StructItem StructItem;

        public ObservableCollection<FieldItem> FieldItems { get; set; }
        public ObservableCollection<FieldType> FieldTypes { get; set; }
        public FieldItem SelectedItem { get; set; }
        public DelegateCommand NewCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DelCommand { get; set; }
        public DelegateCommand RestoreCommand { get; set; }

        public VMModelField(StructItem item)
        {
            this.StructItem = item;
            FieldItems = new ObservableCollection<FieldItem>();
            NewCommand = new DelegateCommand(New);
            SaveCommand = new DelegateCommand(Save);
            DelCommand = new DelegateCommand(Del);
            RestoreCommand = new DelegateCommand(Restore);
            GetFieldTypes();
            BindData(item);
        }

        private void GetFieldTypes()
        {
            FieldTypes = new ObservableCollection<FieldType>();
            var names = Enum.GetNames(typeof(FieldType));
            foreach (var name in names)
            {
                var val = Enum.Parse(typeof(FieldType), name);
                FieldTypes.Add((FieldType)val);
            }    
        }

        private void BindData(StructItem item)
        {
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            //var t = m.GetTable("models");
            //var r = t.Find(t.Filter(f => f.Eq("_id", item.ID))).FirstOrDefault();
            //if (r != null)
            //{
            var t2 = m.GetTable("sys_tables");
            var r = t2.Find(t2.Filter(f => f.Eq("_id", item.ID))).FirstOrDefault();
            if (r != null)
            {
                foreach (var i in r["attribute"].AsBsonArray)
                {
                    FieldItem fi;
                    try
                    {
                        fi = BuildItem(i["Type"].AsString);
                    }
                    catch (Exception)
                    {
                        fi = BuildItem(i["Type"].AsInt32);
                    }
                    if (fi != null)
                    {
                        fi.FillFieldItem(i.AsBsonDocument);
                        FieldItems.Add(fi);
                    }
                }
            }
            foreach (var fi in FieldItems)
            {
                fi.PropertyChanged += Fi_PropertyChanged;
            }
        }

        private void Fi_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var ft = (FieldItem)sender;
            var newft = BuildItem((int)ft.Type);
            if (ft.Type == newft.Type) return;

            newft.Name = ft.Name;
            newft.Type = ft.Type;
           
            var index = -1;
            for (var i = 0; i < FieldItems.Count; i++)
            {
                if(FieldItems[i].Name == ft.Name)
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {

                FieldItems[index] = newft;
                Task.Factory.StartNew(() => Thread.Sleep(500))
                    .ContinueWith((t) =>
                    {
                        newft.PropertyChanged += Fi_PropertyChanged;
                    });
            }
        }

        private void Restore(object obj)
        {
            FieldItems.Clear();
            BindData(StructItem);
            CommonHelper.GetNotify().ShowSuccess("已恢复!");
        }

        private void Del(object obj)
        {
            FieldItems.Remove(SelectedItem);
        }

        private void Save(object obj)
        {
            var ba = new BsonArray();
            foreach (var fi in FieldItems)
            {
                var value = fi.ToBsonDocument();
                value["Type"] = (FieldType)(value["Type"].AsInt32);
                ba.Add(value);
            }
            var db = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var tbl = db.GetTable("sys_tables");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", StructItem.ID);
            var update = Builders<BsonDocument>.Update.Set("attribute", ba);
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

        private void New(object obj)
        {
            var tf = new TextField();
            tf.PropertyChanged += Fi_PropertyChanged;
            FieldItems.Add(tf);
        }

        public FieldItem BuildItem(string typestr)
        {
            //var strs = Enum.GetNames(typeof(FieldType));
            //for (var i = 0; i < strs.Length; i++)
            //{
            //    if (strs[i].ToLower() == typestr)
            //        return BuildItem(i);
            //}
            var val = (int)Enum.Parse(typeof(FieldType), typestr, true);
            return BuildItem(val);
        }

        public FieldItem BuildItem(int typeval)
        {
            var type = (FieldType)typeval;
            var fullName = Assembly.GetExecutingAssembly().GetName().Name;
            try
            {
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
            catch (TypeLoadException e)
            {
                return null;
            }
        }

    }
}
