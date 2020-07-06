using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DC.IDE.UI.Model
{
    public class PageItem
    {
        public string Title { get; set; }
        public ObjectId Id { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
    }
}
