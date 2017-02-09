using System;
using System.Collections.Generic;
using Xunit;
using Python.Runtime;

namespace Python.EmbeddingTest
{
    public class PyIterTest
    {
        [Fact]
        public void TestOnPyList()
        {
            using (Py.GIL())
            {
                var list = new PyList();
                list.Append(new PyString("foo"));
                list.Append(new PyString("bar"));
                list.Append(new PyString("baz"));
                var result = new List<string>();
                foreach (PyObject item in list)
                {
                    result.Add(item.ToString());
                }
                Assert.Equal(3, result.Count);
                Assert.Equal("foo", result[0]);
                Assert.Equal("bar", result[1]);
                Assert.Equal("baz", result[2]);
            }
        }
    }
}
