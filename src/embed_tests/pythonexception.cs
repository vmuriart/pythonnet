using System;
using NUnit.Framework;
using Python.Runtime;

namespace Python.EmbeddingTest
{
    using Net;

    [TestFixture]
    public class PythonExceptionTest
    {
        [Test]
        public void TestMessage()
        {
            using (PythonWrapper.GIL())
            {
                PyList list = new PyList();
                try
                {
                    PyObject junk = list[0];
                }
                catch (PythonException e)
                {
                    Assert.AreEqual("IndexError : list index out of range", e.Message);
                }
            }
        }

        [Test]
        public void TestNoError()
        {
            using (PythonWrapper.GIL())
            {
                PythonException e = new PythonException(); //There is no PyErr to fetch
                Assert.AreEqual("", e.Message);
            }
        }
    }
}