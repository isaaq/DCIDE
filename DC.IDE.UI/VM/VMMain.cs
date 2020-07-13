using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DC.IDE.UI.Model;
using DC.IDE.UI.Util;
using PostSharp.Patterns.Model;
using Telerik.Windows.Controls;

namespace DCIDE.UI.VM
{
    [NotifyPropertyChanged]
    public class VMMain : ViewModelBase
    {
        public ObservableCollection<FuncItem> FuncList { get; set; }
        public object PropList { get; set; }

        public VMMain()
        {
            FuncList = new ObservableCollection<FuncItem>();
            var nodelist = XmlHelper.Load("Data/FuncList.xml");
            foreach(XmlNode node in nodelist)
            {
                FuncList.Add(new FuncItem() { Id = node.Attributes["id"].Value, Text = node.InnerText });
            }


            //var pm = new PropModel();
            //for (int i = 0; i < 20; i++)
            //{
            //    pm[string.Format("Property {0}", i)] = string.Format("Value {0}", i);
            //}
            //PropList = pm;
        }

        public void OnPropChange()
        {
            OnPropertyChanged("PropList");
        }
    }
}
