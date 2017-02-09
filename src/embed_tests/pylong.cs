using System;
using Xunit;
using Python.Runtime;

namespace Python.EmbeddingTest
{
    public class PyLongTest
    {
        [Fact]
        public void TestToInt64()
        {
            using (Py.GIL())
            {
                long largeNumber = 8L * 1024L * 1024L * 1024L; // 8 GB
                var pyLargeNumber = new PyLong(largeNumber);
                Assert.Equal(largeNumber, pyLargeNumber.ToInt64());
            }
        }
    }
}
