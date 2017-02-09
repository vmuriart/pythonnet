using System;
using Xunit;
using Python.Runtime;

namespace Python.EmbeddingTest
{
    /// <summary>
    /// Test Python Exceptions
    /// </summary>
    /// <remarks>
    /// Keeping this in the old-style SetUp/TearDown
    /// to ensure that setup still works.
    /// </remarks>
    public class PythonExceptionTest : IDisposable
    {
        private IntPtr gs;

        public PythonExceptionTest()
        {
            PythonEngine.Initialize();
            gs = PythonEngine.AcquireLock();
        }

        public void Dispose()
        {
            PythonEngine.ReleaseLock(gs);
            PythonEngine.Shutdown();
        }

        [Fact]
        public void TestMessage()
        {
            var list = new PyList();
            try
            {
                PyObject junk = list[0];
            }
            catch (PythonException e)
            {
                Assert.Equal("IndexError : list index out of range", e.Message);
            }
        }

        [Fact]
        public void TestNoError()
        {
            var e = new PythonException(); // There is no PyErr to fetch
            Assert.Equal("", e.Message);
        }
    }
}
