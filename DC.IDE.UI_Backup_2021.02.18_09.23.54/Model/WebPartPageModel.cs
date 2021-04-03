using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DC.IDE.UI.Model.Webpart;

using MongoDB.Bson;

using PostSharp.Patterns.Model;

namespace DC.IDE.UI.Model
{
    public class WebPartPageModel : BaseWebpartPropModel
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string MainContent { get; set; }
        public bool IsCode { get; set; }
        public CategoryItem Parent { get; set; }
    }
}
