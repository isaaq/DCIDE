using System;
using System.Collections.Generic;
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
        [SafeForDependencyAnalysis]
        public PropModel PropertyList
        {
            get {
                var pm = new PropModel();
                foreach(var key in data.Keys)
                {
                    pm[key] = data["key"];
                }
                return pm;
            }
        }
    }
}
