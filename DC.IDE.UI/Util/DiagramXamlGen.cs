using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MongoDB.Bson;

namespace DC.IDE.UI.Util
{
    public static class DiagramXamlGen
    {
        public static string Gen(ObjectId _id)
        {
            return LoadDB(_id);
        }

        private static string LoadDB(ObjectId _id)
        {
            var m = M.GetDB("dc_c_" + Application.Current.Properties["Company"]);
            var t = m.GetTable("sys_views");
            var f = t.Filter(x => x.Eq("_id", _id));
            var r = t.Find(f);
            var item = r.FirstOrDefault();
           
            var hb = new StringBuilder();
            foreach(var i in item["_tables"].AsBsonArray)
            {
                var tit = TableItemTemplate(i["name"].ToString());
                hb.Append(tit);
            }
            string finalstr = Template(hb.ToString());
            return finalstr;
        }

        private static string TableItemTemplate(string content)
        {
            var id = Guid.NewGuid().ToString();
            var items = "0";
            var uniqkey = "9600450";
            var pos = "498.160461425781;13.4605674743652";
            var tableshape = String.Format(@"<TableShape Type=""DC.IDE.UI.Diagram.Table.TableShape"" Id=""{0}"" ZIndex=""0"" Position="""" IsRotationEnabled=""false"" IsResizingEnabled=""false"" IsConnectorsManipulationEnabled=""false"" Content=""{1}"" Size=""240; 150"" RotationAngle=""0"" MinWidth=""200"" MinHeight=""100"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Auto: 0.5; 0.5,Left: 0; 0.5,Top: 0.5; 0,Right: 1; 0.5,Bottom: 0.5; 1,"" UseDefaultConnectors=""True"" RestoredHeight=""0"" RestoredMinHeight=""0"" RestoredIsResizingEnabled=""false"" QN=""0"" Items=""{2}"" NodeUniqueIdKey=""{3}"" MyIsCollapsed=""False"" MyPosition=""{4}"" />",
                id, content, items, uniqkey, pos);
            return tableshape;
        }

        private static string Template(string content)
        {
            var str = @"<?xml version=""1.0"" encoding=""utf-8""?>
<RadDiagram Version=""2013.3"">
    <Metadata Type=""DC.IDE.UI.Diagram.Table.SqlDiagram"" Id=""e4416f79-3015-4f11-8fc6-b726c1f7be41"" Zoom=""1.07960855960846"" Position=""73;91"" IsSnapEnabled = ""false"" RoundedCorners=""True"" Routing=""True"" IsBackgroundSurfaceVisible=""false"">
    <Title>Diagram [10/4/2013 4:43:42 PM]</Title>
    <Description></Description></Metadata>
    <Groups/> 
    <Shapes QNs=""DC.IDE.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"">";

            var str2 = "</Shapes>";
            var str4 = "</RadDiagram>";
            return str + content + str2 + str4;
        }
    }
}
