using System;

namespace Python.Runtime
{
    using System.Reflection;
    using System.Runtime.InteropServices;

#if (PYTHON32 || PYTHON33 || PYTHON34 || PYTHON35)
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal class BytesOffset
    {
        static BytesOffset()
        {
            Type type = typeof(BytesOffset);
            FieldInfo[] fi = type.GetFields();
            int size = IntPtr.Size;
            for (int i = 0; i < fi.Length; i++)
            {
                fi[i].SetValue(null, i * size);
            }
        }

        /* The *real* layout of a type object when allocated on the heap */
        //typedef struct _heaptypeobject {
#if (Py_DEBUG)  // #ifdef Py_TRACE_REFS
/* _PyObject_HEAD_EXTRA defines pointers to support a doubly-linked list of all live heap objects. */
        public static int _ob_next = 0;
        public static int _ob_prev = 0;
#endif
        // PyObject_VAR_HEAD {
        //     PyObject_HEAD {
        public static int ob_refcnt = 0;
        public static int ob_type = 0;
        // }
        public static int ob_size = 0;      /* Number of items in _VAR_iable part */
        // }
        public static int ob_shash = 0;
        public static int ob_sval = 0; /* start of data */

        /* Invariants:
         *     ob_sval contains space for 'ob_size+1' elements.
         *     ob_sval[ob_size] == 0.
         *     ob_shash is the hash of the string or -1 if not computed yet.
         */
        //} PyBytesObject;
    }
#endif
}
