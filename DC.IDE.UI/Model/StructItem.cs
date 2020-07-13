using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DC.IDE.UI.Model
{
    public class StructItem
    {
        public ObjectId ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public BsonArray Fields { get; set; }
        public DateTime ModifyTime { get; set; }
        public string Description { get; set; }
    }
}
