using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Model.Webpart
{
    public class CategoryItem
    {
        public string _Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public IEnumerable<WebPartPageModel> Children { get; set; }
    }
}
