using System;

namespace Python.Runtime.Interop
{
    using System.Reflection;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal class ExceptionOffset
    {
        static ExceptionOffset()
        {
            Type type = typeof(ExceptionOffset);
            FieldInfo[] fi = type.GetFields();
            int size = IntPtr.Size;
            for (int i = 0; i < fi.Length; i++)
            {
                fi[i].SetValue(null, (i * size) + ObjectOffset.ob_type + size);
            }
        }

        // PyException_HEAD
        // (start after PyObject_HEAD)
        public static int dict = 0;
        public static int args = 0;
#if (PYTHON25 || PYTHON26 || PYTHON27)
        public static int message = 0;
#elif (PYTHON32 || PYTHON33 || PYTHON34 || PYTHON35)
        public static int traceback = 0;
        public static int context = 0;
        public static int cause = 0;
#if !PYTHON32
        public static int suppress_context = 0;
#endif
#endif

        // extra c# data
        public static int ob_dict;
        public static int ob_data;
    }
}
