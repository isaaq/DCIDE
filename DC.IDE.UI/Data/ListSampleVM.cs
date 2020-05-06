using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Data
{
    public class ListItem
    {
        public string Name { get; set; }
        public int Switch { get; set; }
    }

    public class ListSampleVM
    {
        public ObservableCollection<ListItem> ListItems { get; set; }

        public ListSampleVM()
        {
            ListItems = new ObservableCollection<ListItem>();
            ListItems.Add(new ListItem() { Name = "测试9", Switch = 0 });
            ListItems.Add(new ListItem() { Name = "测试8", Switch = 0 });
            ListItems.Add(new ListItem() { Name = "测试1", Switch = 0 });
            ListItems.Add(new ListItem() { Name = "测试2", Switch = 0 });
            ListItems.Add(new ListItem() { Name = "测试3", Switch = 0 });
            ListItems.Add(new ListItem() { Name = "aa", Switch = 0 });
            ListItems.Add(new ListItem() { Name = "bb", Switch = 0 });
            ListItems.Add(new ListItem() { Name = "cc", Switch = 0 });

        }
    }
}
