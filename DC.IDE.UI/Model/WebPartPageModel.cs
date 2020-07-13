using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;

using PostSharp.Patterns.Model;

namespace DC.IDE.UI.Model
{
    public class WebPartPageModel : BaseWebpartPropModel
    {
        public ObjectId Id { get; set; }
    }
}
