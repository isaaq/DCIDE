using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;

namespace DC.IDE.UI.Model
{
    public class MenuItem
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Namespace { get; set; }
        public string Url { get; set; }
        public DateTime CreateTime { get; set; }
        public IEnumerable<MenuItem> Children { get; set; }
        public ObjectId Id { get; set; }
    }
}
