using System;
using NUnit.Framework;
using Python.Runtime;

namespace Python.EmbeddingTest
{
    [TestFixture]
    public class PyObjectTest
    {
        [Test]
        public void TestUnicode()
        {
            PyObject s = new PyString("foo\u00e9");
            Assert.AreEqual("foo\u00e9", s.ToString());
        }
    }
}