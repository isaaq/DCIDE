using System;
using System.Linq;
using DC.IDE.UI.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DC.IDE.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestResource()
        {
            ResHelper.Load("FuncList");
        }

        [TestMethod]
        public void TestMongo()
        {
            var db = M.GetDB();
            var tb = db.GetCollection<BsonDocument>("users");
            var filter = Builders<BsonDocument>.Filter.Eq("username", "test");
            var doc = tb.FindSync(filter).FirstOrDefault();
            var r = doc.ToString();
            Console.WriteLine(r);
        }

        [TestMethod]
        public void TestMongo2()
        {
            var db = M.GetDB();
            var tb = db.GetCollection<BsonDocument>("users");
            var filter = Builders<BsonDocument>.Filter.Eq("username", "test");
            var doc = tb.Find(filter);
            var r = doc[0];
            Console.WriteLine(r);
        }

        [TestMethod]
        public void TestMongo3()
        {
            var db = M.GetDB();
            var tb = db.GetCollection<BsonDocument>("users");
            var doc = tb.FindAll();
            var r = doc;
            Console.WriteLine(r);
        }

        [TestMethod]
        public void TestDiagramGen()
        {
            var _id = new ObjectId("5e9eb7bb8b26d60db108f53f");
            var t = M.GetDB("dc_c_5e955138e90140719b3f719e").GetTable("models");
            var f = t.Filter(x => x.Eq("_id", _id));
            var r = t.Find(f);
            var item = r.FirstOrDefault();
            var tables = item["tables"];
            //foreach(var table in tables.AsBsonArray)
            //{

            //}
            var t2 = M.GetDB("dc_c_5e955138e90140719b3f719e").GetTable("tables");
            var f2 = t2.Filter(x => x.In<ObjectId>("_id", tables.AsBsonArray.ToObjectId()));
            var r2 = t2.Find(f2);
        }

        [TestMethod]
        public void TStr()
        {
            var id = "aaaaaaaaaaa";
            var content = "bbbbbbb";
            var items = "";
            var uniqkey = "";
            var pos = "";
            var tableshape = String.Format(@"<TableShape Type=""DC.IDE.UI.Diagram.Table.TableShape"" Id=""{0}"" ZIndex=""0"" Position="""" IsRotationEnabled=""false"" IsResizingEnabled=""false"" IsConnectorsManipulationEnabled=""false"" Content=""{1}"" Size=""240; 150"" RotationAngle=""0"" MinWidth=""200"" MinHeight=""100"" MaxWidth=""INF"" MaxHeight=""INF"" Connectors=""Auto: 0.5; 0.5,Left: 0; 0.5,Top: 0.5; 0,Right: 1; 0.5,Bottom: 0.5; 1,"" UseDefaultConnectors=""True"" RestoredHeight=""0"" RestoredMinHeight=""0"" RestoredIsResizingEnabled=""false"" QN=""0"" Items=""{2}"" NodeUniqueIdKey=""{3}"" MyIsCollapsed=""False"" MyPosition=""{4}"" />",
                id, content, items, uniqkey, pos);
            Console.WriteLine(tableshape);
        }
    }
}
