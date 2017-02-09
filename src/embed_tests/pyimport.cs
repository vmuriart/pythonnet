using System;
using System.IO;
using System.Reflection;
using Xunit;
using Python.Runtime;

namespace Python.EmbeddingTest
{
    /// <summary>
    /// Test Import unittests and regressions
    /// </summary>
    /// <remarks>
    /// Keeping in old-style SetUp/TearDown due to required SetUp.
    /// The required directory structure was added to .\pythonnet\src\embed_tests\fixtures\ directory:
    /// + PyImportTest/
    /// | - __init__.py
    /// | + test/
    /// | | - __init__.py
    /// | | - one.py
    /// </remarks>
    public class PyImportTest : IDisposable
    {
        private IntPtr gs;

        public PyImportTest()
        {
            PythonEngine.Initialize();
            gs = PythonEngine.AcquireLock();

            /* Append the tests directory to sys.path
             * using reflection to circumvent the private
             * modifiers placed on most Runtime methods. */
            const string s = "../fixtures";
            string testPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), s);

            IntPtr str = Runtime.Runtime.PyString_FromString(testPath);
            IntPtr path = Runtime.Runtime.PySys_GetObject("path");
            Runtime.Runtime.PyList_Append(path, str);
        }

        public void Dispose()
        {
            PythonEngine.ReleaseLock(gs);
            PythonEngine.Shutdown();
        }

        /// <summary>
        /// Test subdirectory import
        /// </summary>
        [Fact]
        public void TestDottedName()
        {
            PyObject module = PythonEngine.ImportModule("PyImportTest.test.one");
            Assert.NotNull(module);
        }

        /// <summary>
        /// Tests that sys.args is set. If it wasn't exception would be raised.
        /// </summary>
        [Fact]
        public void TestSysArgsImportException()
        {
            PyObject module = PythonEngine.ImportModule("PyImportTest.sysargv");
            Assert.NotNull(module);
        }
    }
}
