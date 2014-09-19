using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonTaskTests;

namespace LibTests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void FormatWith_ValidFormat()
        {
            TestClass obja = new TestClass
            {
                FirstProperty = "Hi",
                ReferenceProperty = DateTime.Now
            };
            TestClass objb = new TestClass
            {
                FirstProperty = "Second instance",
                ReferenceProperty = DateTime.Now.AddDays(-1).AddSeconds(-100),
                SelfReferencedProperty = obja
            };
            obja.SelfReferencedProperty = objb;
            string format = "First : {FirstProperty} \n" +
                              "Second : {ReferenceProperty}\n" +
                              "Inner : {SelfReferencedProperty.SomeMethod()} \n";
            string firstLine = "First : Hi ";
            string formattedString = obja.FormatWith(format);
            string actualLIne = formattedString.Substring(0, formattedString.IndexOf('\n'));
            Console.WriteLine(obja.FormatWith(format));
            Console.WriteLine();
            Assert.AreEqual(firstLine, actualLIne, "Format With Failed");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormatWith_NullFormat()
        {
            TestClass obja = new TestClass
            {
                FirstProperty = "Hi",
                ReferenceProperty = DateTime.Now
            };
            TestClass objb = new TestClass
            {
                FirstProperty = "Second instance",
                ReferenceProperty = DateTime.Now.AddDays(-1).AddSeconds(-100),
                SelfReferencedProperty = obja
            };
            obja.SelfReferencedProperty = objb;
            string format = "First : {FirstProperty} \n" +
                              "Second : {ReferenceProperty}\n" +
                              "Inner : {SelfReferencedProperty.SomeMethod()} \n";
            string firstLine = "First : Hi ";
            string formattedString = obja.FormatWith(null);
            string actualLIne = formattedString.Substring(0, formattedString.IndexOf('\n'));
            Console.WriteLine(obja.FormatWith(format));
            Console.WriteLine();
            Assert.AreEqual(firstLine, actualLIne, "Format With Failed");
        }
    }
}
