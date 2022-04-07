using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DCIDE
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var company = ConfigurationManager.AppSettings["company"];
            if (company != null)
                App.Current.Properties["Company"] = company;
            else
                App.Current.Properties["Company"] = "5e955138e90140719b3f719e";
            App.Current.Properties["IsDesign"] = false;
        }
    }
}
