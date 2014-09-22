using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonTasksLib.Data;

namespace LibTests
{
    [TestClass]
    public class ObjectExtensionTests
    {
        [TestMethod]
        public void Transfer_NullDestination()
        {
            TestClass obja = new TestClass
            {
                FirstProperty = "Hi",
                ReferenceProperty = DateTime.Now
            };
            TestClass objb = null;

            obja.Transfer(ref objb);

            Assert.AreEqual(obja.FirstProperty, objb.FirstProperty, "Transfer operation failed.");
        }

        [TestMethod]
        public void Transfer_USingSkip()
        {
            TestClass obja = new TestClass
            {
                FirstProperty = "Hi",
                ReferenceProperty = DateTime.Now
            };
            TestClass objb = null;

            obja.Transfer(ref objb, "FirstProperty, ReferenceProperty");

            Assert.AreEqual(null, objb.FirstProperty, "Transfer operation failed.");
        }
    }
}
