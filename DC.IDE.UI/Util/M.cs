using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MongoDB.Bson;
using MongoDB.Driver;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DC.IDE.UI.Util
{
    public static class M
    {
        public static IMongoDatabase GetDB(string name = null)
        {
            //获取链接池大小
            //int connectionPool = Convert.ToInt32(ConfigurationManager.AppSettings["connectionPool"]);
            //int minpool = Convert.ToInt32(ConfigurationManager.AppSettings["minpool"]);
            string hostname = ConfigurationManager.AppSettings["hostname"];
            var port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
            string username = ConfigurationManager.AppSettings["username"];
            string password = ConfigurationManager.AppSettings["password"];     
            string database = name == null ? ConfigurationManager.AppSettings["database"] : name;

            if (Application.Current.Properties["IsDesign"] == null)
            {
                if (username == null)
                    username = "cloud";
                if (password == null)
                    password = "cloud123";
                if (database == null)
                    database = "dc_matrix";
                if (hostname == null)
                    hostname = "47.111.174.241";
                if (port == 0)
                    port = 27017;
            }

            var ipaddress = new MongoServerAddress(hostname, port);//设置服务器的ip和端口
            var settingsclient = new MongoClientSettings();//实例化客户端设置类

            settingsclient.Server = ipaddress;//端口赋值
            var credential = MongoCredential.CreateCredential("admin", username, password);
            //settingsclient.MaxConnectionPoolSize = connectionPool;
            //settingsclient.MinConnectionPoolSize = minpool;
            settingsclient.ConnectionMode = 0;// ConnectionMode.ReplicaSet;//链接模式设置
            settingsclient.Credential = credential;
            var client = new MongoClient(settingsclient);//调用客户端类构造函数设置参数
            var db = client.GetDatabase(database);//获取数据库名称
            return db;
        }

        public static IMongoCollection<BsonDocument> GetTable(this IMongoDatabase db, string name)
        {
            return db.GetCollection<BsonDocument>(name);
        }

        public static IMongoCollection<BsonDocument> GetTable(string name)
        {
            return GetDB().GetCollection<BsonDocument>(name);
        }

        public static List<BsonDocument> Find(this IMongoCollection<BsonDocument> tb, FilterDefinition<BsonDocument> filter)
        {
            var list = tb.FindSync(filter).ToList();
            return list;
        }

        public static List<BsonDocument> FindAll(this IMongoCollection<BsonDocument> tb)
        {
            var list = tb.FindSync(new BsonDocument()).ToList();
            return list;
        }



        public static FilterDefinition<BsonDocument> Filter(this IMongoCollection<BsonDocument> tb, Func<FilterDefinitionBuilder<BsonDocument>, FilterDefinition<BsonDocument>> action)
        {
            var filter = Builders<BsonDocument>.Filter;
            return action(filter);
        }
    }
}
