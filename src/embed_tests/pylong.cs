using System;
using NUnit.Framework;
using Python.Runtime;

namespace Python.EmbeddingTest
{
    using Net;

    [TestFixture]
    public class PyLongTest
    {
        [Test]
        public void TestToInt64()
        {
            using (PythonWrapper.GIL())
            {
                long largeNumber = 8L * 1024L * 1024L * 1024L; // 8 GB
                PyLong pyLargeNumber = new PyLong(largeNumber);
                Assert.AreEqual(largeNumber, pyLargeNumber.ToInt64());
            }
        }
    }
}
