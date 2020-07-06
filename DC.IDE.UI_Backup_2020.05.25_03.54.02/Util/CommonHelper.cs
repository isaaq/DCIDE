using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DC.IDE.UI.Model.Field;
using MongoDB.Bson;

namespace DC.IDE.UI.Util
{
    public static class CommonHelper
    {
        public static ObjectId ToObjectId(this object str)
        {
            return new ObjectId(str.ToString());
        }

        public static IEnumerable<ObjectId> ToObjectId(this IEnumerable<object> objs)
        {
            return objs.Select(s => s.ToObjectId());
        }

        public static FieldItem FillFieldItem(this FieldItem fi, BsonDocument doc)
        {
            if (doc == null) return null;
            var type = fi.GetType();

            foreach (var ele in doc)
            {
                var name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(ele.Name);
                var propertyInfo = type.GetProperty(name);
                if(propertyInfo != null)
                    propertyInfo.SetValue(fi, ele.Value, null);
            }
            return fi;
        }
    }
}
