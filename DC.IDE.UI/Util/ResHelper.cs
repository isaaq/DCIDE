using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Util
{
    public static class ResHelper
    {
        public static string Load(string name)
        {
            var assm = Assembly.GetExecutingAssembly();
            var assmname = assm.GetName().Name;
            var istr = assm.GetManifestResourceStream(assmname + "." + name.Replace("/", "."));
            var sr = new System.IO.StreamReader(istr);
            string str = sr.ReadToEnd();
            return str;
        }
    }
}
