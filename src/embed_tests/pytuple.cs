using System;
using Xunit;
using Python.Runtime;

namespace Python.EmbeddingTest
{
    public class PyTupleTest
    {
        /// <summary>
        /// Test IsTupleType without having to Initialize a tuple.
        /// PyTuple constructor use IsTupleType. This decouples the tests.
        /// </summary>
        [Fact]
        public void TestStringIsTupleType()
        {
            using (Py.GIL())
            {
                var s = new PyString("foo");
                Assert.False(PyTuple.IsTupleType(s));
            }
        }

        /// <summary>
        /// Test IsTupleType with Tuple.
        /// </summary>
        [Fact]
        public void TestPyTupleIsTupleType()
        {
            using (Py.GIL())
            {
                var t = new PyTuple();
                Assert.True(PyTuple.IsTupleType(t));
            }
        }

        [Fact]
        public void TestPyTupleEmpty()
        {
            using (Py.GIL())
            {
                var t = new PyTuple();
                Assert.Equal(0, t.Length());
            }
        }

        /// <remarks>
        /// FIXME: Unable to unload AppDomain, Unload thread timed out.
        /// Seen on Travis/AppVeyor on both PY2 and PY3. Causes Embedded_Tests
        /// to hang after they are finished for ~40 seconds until nunit3 forces
        /// a timeout on unloading tests. Doesn't fail the tests though but
        /// greatly slows down CI. nunit2 silently has this issue.
        /// </remarks>
        [Fact(Skip= "GH#397: Travis/AppVeyor: Unable to unload AppDomain, Unload thread timed out")]
        public void TestPyTupleInvalidAppend()
        {
            using (Py.GIL())
            {
                PyObject s = new PyString("foo");
                var t = new PyTuple();

                var ex = Assert.Throws<PythonException>(() => t.Concat(s));

                StringAssert.StartsWith("TypeError : can only concatenate tuple", ex.Message);
                Assert.AreEqual(0, t.Length());
                Assert.IsEmpty(t);
            }
        }

        [Fact]
        public void TestPyTupleValidAppend()
        {
            using (Py.GIL())
            {
                var t0 = new PyTuple();
                var t = new PyTuple();
                t.Concat(t0);
                Assert.NotNull(t);
                Assert.IsType(typeof(PyTuple), t);
            }
        }

        [Fact]
        public void TestPyTupleStringConvert()
        {
            using (Py.GIL())
            {
                PyObject s = new PyString("foo");
                PyTuple t = PyTuple.AsTuple(s);
                Assert.NotNull(t);
                Assert.IsType(typeof(PyTuple), t);
                Assert.Equal("f", t[0].ToString());
                Assert.Equal("o", t[1].ToString());
                Assert.Equal("o", t[2].ToString());
            }
        }

        [Fact]
        public void TestPyTupleValidConvert()
        {
            using (Py.GIL())
            {
                var l = new PyList();
                PyTuple t = PyTuple.AsTuple(l);
                Assert.NotNull(t);
                Assert.IsType(typeof(PyTuple), t);
            }
        }

        /// <remarks>
        /// FIXME: Possible source of intermittent AppVeyor PY27: Unable to unload AppDomain.
        /// </remarks>
        [Fact]
        public void TestNewPyTupleFromPyTuple()
        {
            using (Py.GIL())
            {
                var t0 = new PyTuple();
                var t = new PyTuple(t0);
                Assert.NotNull(t);
                Assert.IsType(typeof(PyTuple), t);
            }
        }

        /// <remarks>
        /// TODO: Should this throw ArgumentError instead?
        /// </remarks>
        [Test]
        public void TestInvalidAsTuple()
        {
            using (Py.GIL())
            {
                var i = new PyInt(5);
                PyTuple t = null;

                var ex = Assert.Throws<PythonException>(() => t = PyTuple.AsTuple(i));

                Assert.AreEqual("TypeError : 'int' object is not iterable", ex.Message);
                Assert.IsNull(t);
            }
        }
    }
}
