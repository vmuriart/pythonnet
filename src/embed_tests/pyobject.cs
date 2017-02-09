using System;
using Xunit;
using Python.Runtime;

namespace Python.EmbeddingTest
{
    public class PyObjectTest
    {
        [Fact]
        public void TestUnicode()
        {
            using (Py.GIL())
            {
                PyObject s = new PyString("foo\u00e9");
                Assert.Equal("foo\u00e9", s.ToString());
            }
        }
    }
}
