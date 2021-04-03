using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;

namespace DC.IDE.UI.Model
{
    public class DatasourceListItem
    {
        public ObjectId _Id { get; set; }
        public string Name { get; set; }
    }
}
