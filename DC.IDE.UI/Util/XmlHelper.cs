using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DC.IDE.UI.Util
{
    public static class XmlHelper
    {
        public static XmlNodeList Load(string name)
        {
            var xmlstr = ResHelper.Load(name);
            var xml = new XmlDocument();
            xml.LoadXml(xmlstr);
            var nodes = xml.SelectNodes("//item");
            return nodes;
        }
    }
}
