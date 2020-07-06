﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DC.IDE.UI.Model;
using DC.IDE.UI.Util;
using PostSharp.Patterns.Model;
using Telerik.Windows.Controls;

namespace DCIDE.VM
{
    [NotifyPropertyChanged]
    public class MainVM : ViewModelBase
    {
        public ObservableCollection<FuncItem> FuncList { get; set; }

        public MainVM()
        {
            FuncList = new ObservableCollection<FuncItem>();
            var nodelist = XmlHelper.Load("Data/FuncList.xml");
            foreach(XmlNode node in nodelist)
            {
                FuncList.Add(new FuncItem() { Id = node.Attributes["id"].Value, Text = node.InnerText });
            }

            
        }

    }
}
