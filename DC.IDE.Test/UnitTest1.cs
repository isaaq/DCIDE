using System;
using DC.IDE.UI.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
