using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DC.IDE.PackageBuilder.Model;

using PostSharp.Patterns.Model;

using Telerik.Windows.Controls;

namespace DC.IDE.PackageBuilder.VM
{
    [NotifyPropertyChanged]
    public class VMMain : ViewModelBase
    {
        public ObservableCollection<PkgItem> PkgItemTree { get; set; }

        public VMMain()
        {
            GetPkgFileList();
        }

        private void GetPkgFileList()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + @"\Samples\Sp1\";
            PkgItemTree = new ObservableCollection<PkgItem>();

            //var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            //foreach (var file in files)
            //{
            //    var f = new FileInfo(file);
            //    var item = new PkgItem();
            //    item.Title = f.Name;
            //    GetFiles(path, item.Items);
            //    PkgItemTree.Add(item);
            //}
            DirSearch_ex3(path, PkgItemTree);
        }

        //private void GetFiles(string path, ObservableCollection<PkgItem> items)
        //{
        //    var files = Directory.GetFiles(path);

        //    foreach (var file in files)
        //    {
        //        var f = new FileInfo(file);
        //        var item = new PkgItem();
        //        item.Title = f.Name;
        //    }
        //}

        static void DirSearch_ex3(string sDir, ObservableCollection<PkgItem> pkgItemTree)
        {
            //Console.WriteLine("DirSearch..(" + sDir + ")");
            try
            {
                //Console.WriteLine(sDir);  

                foreach (string f in Directory.GetFiles(sDir))
                {
                    pkgItemTree.Add(new PkgItem() { Title = new FileInfo(f).Name });
                }

                foreach (string d in Directory.GetDirectories(sDir))
                {
                    var item = new PkgItem() { Title = new FileInfo(d).Name, Items = new ObservableCollection<PkgItem>() };
                    
                    DirSearch_ex3(d, item.Items);
                    pkgItemTree.Add(item);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
    }
}
