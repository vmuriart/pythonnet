using System;

namespace Python.Runtime
{
    /// <summary>
    /// Represents a generic Python number. The methods of this class are
    /// equivalent to the Python "abstract number API". See
    /// http://www.python.org/doc/current/api/number.html for details.
    /// </summary>
    public class PyNumber : PyObject
    {
        protected PyNumber(IntPtr ptr) : base(ptr)
        {
        }

        protected PyNumber() : base()
        {
        }


        /// <summary>
        /// IsNumberType Method
        /// </summary>
        ///
        /// <remarks>
        /// Returns true if the given object is a Python numeric type.
        /// </remarks>
        public static bool IsNumberType(PyObject value)
        {
            return Runtime.PyNumber_Check(value.obj);
        }


        // TODO: add all of the PyNumber_XXX methods.
    }
}