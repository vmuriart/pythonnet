using System;
using NUnit.Framework;
using Python.Runtime;

namespace Python.EmbeddingTest
{
    public class PyTupleTest
    {
        /// <summary>
        /// Test IsTupleType without having to Initialize a tuple.
        /// PyTuple constructor use IsTupleType. This decouples the tests.
        /// </summary>
        [Test]
        public void TestStringIsTupleType()
        {
            using (Py.GIL())
            {
                var s = new PyString("foo");
                Assert.IsFalse(PyTuple.IsTupleType(s));
            }
        }

        /// <summary>
        /// Test IsTupleType with Tuple.
        /// </summary>
        [Test]
        public void TestPyTupleIsTupleType()
        {
            using (Py.GIL())
            {
                var t = new PyTuple();
                Assert.IsTrue(PyTuple.IsTupleType(t));
            }
        }

        [Test]
        public void TestPyTupleEmpty()
        {
            using (Py.GIL())
            {
                var t = new PyTuple();
                Assert.AreEqual(0, t.Length());
            }
        }

        [Test]
        public void TestPyTupleBadCtor()
        {
            using (Py.GIL())
            {
                var i = new PyInt(5);
                PyTuple t = null;

                var ex = Assert.Throws<ArgumentException>(() => t = new PyTuple(i));

                Assert.AreEqual("object is not a tuple", ex.Message);
                Assert.IsNull(t);
            }
        }

        [Test]
        [Ignore("Add GC Tests. This one hangs tests")]
        public void TestPyTupleArrayCtorDisposedValue()
        {
            using (Py.GIL())
            {
                var numbers = new PyObject[2] {new PyInt(0), new PyInt(1)};
                numbers[0].Dispose();

                var t = new PyTuple(numbers);

                Assert.IsNull(t[0]);
                Assert.Equals(1, t[1]);
            }
        }

        /// <summary>
        /// Test PyTuple.Concat(...) doesn't let invalid appends happen
        /// and throws and exception.
        /// </summary>
        /// <remarks>
        /// Test has second purpose. Currently it generated an Exception
        /// that the GC failed to remove often and caused AppDomain unload
        /// errors at the end of tests. See GH#397 for more info.
        /// <para />
        /// Curious, on PY27 it gets a Unicode on the ex.Message. On PY3+ its string.
        /// </remarks>
        [Test]
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

        [Test]
        public void TestPyTupleValidAppend()
        {
            using (Py.GIL())
            {
                var t0 = new PyTuple();
                var t = new PyTuple();
                t.Concat(t0);
                Assert.IsNotNull(t);
                Assert.IsInstanceOf(typeof(PyTuple), t);
            }
        }

        [Test]
        public void TestPyTupleStringConvert()
        {
            using (Py.GIL())
            {
                PyObject s = new PyString("foo");
                PyTuple t = PyTuple.AsTuple(s);
                Assert.IsNotNull(t);
                Assert.IsInstanceOf(typeof(PyTuple), t);
                Assert.AreEqual("f", t[0].ToString());
                Assert.AreEqual("o", t[1].ToString());
                Assert.AreEqual("o", t[2].ToString());
            }
        }

        [Test]
        public void TestPyTupleValidConvert()
        {
            using (Py.GIL())
            {
                var l = new PyList();
                PyTuple t = PyTuple.AsTuple(l);
                Assert.IsNotNull(t);
                Assert.IsInstanceOf(typeof(PyTuple), t);
            }
        }

        [Test]
        public void TestNewPyTupleFromPyTuple()
        {
            using (Py.GIL())
            {
                var t0 = new PyTuple();
                var t = new PyTuple(t0);
                Assert.IsNotNull(t);
                Assert.IsInstanceOf(typeof(PyTuple), t);
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
