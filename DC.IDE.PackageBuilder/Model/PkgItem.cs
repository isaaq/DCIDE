using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.PackageBuilder.Model
{
    public class PkgItem
    {
        public string Fullpath { get; set; }
        public string Title { get; set; }
        public ObservableCollection<PkgItem> Items { get; set; }
        public string Content { get; set; }
    }
}
