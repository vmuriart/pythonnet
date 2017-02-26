using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using Python.Runtime;
using Python.Runtime.InteropContracts;

namespace Python.Runtime.Interop
{
    public class PythonRuntimeInterop : IPythonRuntimeInterop
    {
#if UCS4
        public const int UCS = 4;
#elif UCS2
        public const int UCS = 2;
#else
#error You must define either UCS2 or UCS4!
#endif

#if PYTHON27
        public const string pyversion = "2.7";
        public const int pyversionnumber = 27;
#elif PYTHON33
        public const string pyversion = "3.3";
        public const int pyversionnumber = 33;
#elif PYTHON34
        public const string pyversion = "3.4";
        public const int pyversionnumber = 34;
#elif PYTHON35
        public const string pyversion = "3.5";
        public const int pyversionnumber = 35;
#elif PYTHON36
        public const string pyversion = "3.6";
        public const int pyversionnumber = 36;
#elif PYTHON37 // TODO: Add interop37 after Python3.7 is released
        public const string pyversion = "3.7";
        public const int pyversionnumber = 37;
#else
#error You must define one of PYTHON33 to PYTHON37 or PYTHON27
#endif

#if MONO_LINUX || MONO_OSX
#if PYTHON27
        internal const string dllBase = "python27";
#elif PYTHON33
        internal const string dllBase = "python3.3";
#elif PYTHON34
        internal const string dllBase = "python3.4";
#elif PYTHON35
        internal const string dllBase = "python3.5";
#elif PYTHON36
        internal const string dllBase = "python3.6";
#elif PYTHON37
        internal const string dllBase = "python3.7";
#endif
#else // Windows
#if PYTHON27
        internal const string dllBase = "python27";
#elif PYTHON33
        internal const string dllBase = "python33";
#elif PYTHON34
        internal const string dllBase = "python34";
#elif PYTHON35
        internal const string dllBase = "python35";
#elif PYTHON36
        internal const string dllBase = "python36";
#elif PYTHON37
        internal const string dllBase = "python37";
#endif
#endif

#if PYTHON_WITH_PYDEBUG
        internal const string dllWithPyDebug = "d";
#else
        internal const string dllWithPyDebug = "";
#endif

#if PYTHON_WITH_PYMALLOC
        internal const string dllWithPyMalloc = "m";
#else
        internal const string dllWithPyMalloc = "";
#endif

#if PYTHON_WITH_WIDE_UNICODE
        internal const string dllWithWideUnicode = "u";
#else
        internal const string dllWithWideUnicode = "";
#endif

#if PYTHON_WITHOUT_ENABLE_SHARED
        public const string dll = "__Internal";
#else
        public const string dll = dllBase + dllWithPyDebug + dllWithPyMalloc + dllWithWideUnicode;
#endif

#if Py_DEBUG
// Py_IncRef and Py_DecRef are taking care of the extra payload
// in Py_DEBUG builds of Python like _Py_RefTotal
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        private unsafe static extern void
            Py_IncRef(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        private unsafe static extern void
            Py_DecRef(IntPtr ob);
#endif

        internal const int Py_LT = 0;
        internal const int Py_LE = 1;
        internal const int Py_EQ = 2;
        internal const int Py_NE = 3;
        internal const int Py_GT = 4;
        internal const int Py_GE = 5;

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern void Py_Initialize();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern int Py_IsInitialized();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern void Py_Finalize();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr Py_NewInterpreter();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern void Py_EndInterpreter(IntPtr threadState);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr PyThreadState_New(IntPtr istate);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr PyThreadState_Get();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr PyThread_get_key_value(IntPtr key);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern int PyThread_get_thread_ident();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern int PyThread_set_key_value(IntPtr key, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr PyThreadState_Swap(IntPtr key);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr PyGILState_Ensure();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern void PyGILState_Release(IntPtr gs);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true,
            CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr PyGILState_GetThisThreadState();

#if PYTHON3
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        public unsafe static extern int
            Py_Main(int argc,
                [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] argv);
#elif PYTHON2
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        public unsafe static extern int
            Py_Main(int argc, string[] argv);
#endif

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyEval_InitThreads();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyEval_ThreadsInitialized();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyEval_AcquireLock();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyEval_ReleaseLock();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyEval_AcquireThread(IntPtr tstate);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyEval_ReleaseThread(IntPtr tstate);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyEval_SaveThread();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyEval_RestoreThread(IntPtr tstate);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyEval_GetBuiltins();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyEval_GetGlobals();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyEval_GetLocals();

#if PYTHON3
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        internal unsafe static extern string
            Py_GetProgramName();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            Py_SetProgramName([MarshalAs(UnmanagedType.LPWStr)] string name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        internal unsafe static extern string
            Py_GetPythonHome();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            Py_SetPythonHome([MarshalAs(UnmanagedType.LPWStr)] string home);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        internal unsafe static extern string
            Py_GetPath();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            Py_SetPath([MarshalAs(UnmanagedType.LPWStr)] string home);
#elif PYTHON2
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern string
            Py_GetProgramName();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            Py_SetProgramName(string name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern string
            Py_GetPythonHome();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            Py_SetPythonHome(string home);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern string
            Py_GetPath();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            Py_SetPath(string home);
#endif

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern string
            Py_GetVersion();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern string
            Py_GetPlatform();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern string
            Py_GetCopyright();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern string
            Py_GetCompiler();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern string
            Py_GetBuildInfo();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyRun_SimpleString(string code);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyRun_String(string code, IntPtr st, IntPtr globals, IntPtr locals);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            Py_CompileString(string code, string file, IntPtr tok);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyImport_ExecCodeModule(string name, IntPtr code);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyCFunction_NewEx(IntPtr ml, IntPtr self, IntPtr mod);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyCFunction_Call(IntPtr func, IntPtr args, IntPtr kw);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyClass_New(IntPtr bases, IntPtr dict, IntPtr name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyInstance_New(IntPtr cls, IntPtr args, IntPtr kw);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyInstance_NewRaw(IntPtr cls, IntPtr dict);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyMethod_New(IntPtr func, IntPtr self, IntPtr cls);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_HasAttrString(IntPtr pointer, string name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_GetAttrString(IntPtr pointer, string name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_SetAttrString(IntPtr pointer, string name, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_HasAttr(IntPtr pointer, IntPtr name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_GetAttr(IntPtr pointer, IntPtr name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_SetAttr(IntPtr pointer, IntPtr name, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_GetItem(IntPtr pointer, IntPtr key);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_SetItem(IntPtr pointer, IntPtr key, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_DelItem(IntPtr pointer, IntPtr key);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_GetIter(IntPtr op);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_Call(IntPtr pointer, IntPtr args, IntPtr kw);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_CallObject(IntPtr pointer, IntPtr args);

#if PYTHON3
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_RichCompareBool(IntPtr value1, IntPtr value2, int opid);

        internal static int PyObject_Compare(IntPtr value1, IntPtr value2)
        {
            int res;
            res = PyObject_RichCompareBool(value1, value2, Py_LT);
            if (-1 == res)
                return -1;
            else if (1 == res)
                return -1;

            res = PyObject_RichCompareBool(value1, value2, Py_EQ);
            if (-1 == res)
                return -1;
            else if (1 == res)
                return 0;

            res = PyObject_RichCompareBool(value1, value2, Py_GT);
            if (-1 == res)
                return -1;
            else if (1 == res)
                return 1;

            Exceptions.SetError(Exceptions.SystemError, "Error comparing objects");
            return -1;
        }
#elif PYTHON2
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_Compare(IntPtr value1, IntPtr value2);
#endif

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_IsInstance(IntPtr ob, IntPtr type);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_IsSubclass(IntPtr ob, IntPtr type);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyCallable_Check(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_IsTrue(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_Not(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_Size(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_Hash(IntPtr op);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_Repr(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_Str(IntPtr pointer);

#if PYTHON3
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyObject_Str",
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_Unicode(IntPtr pointer);
#elif PYTHON2
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_Unicode(IntPtr pointer);
#endif

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_Dir(IntPtr pointer);


        //====================================================================
        // Python number API
        //====================================================================

#if PYTHON3
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyNumber_Long",
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Int(IntPtr ob);
#elif PYTHON2
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Int(IntPtr ob);
#endif

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Long(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Float(IntPtr ob);


        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern bool
            PyNumber_Check(IntPtr ob);

#if PYTHON3
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyLong_FromLong",
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        private unsafe static extern IntPtr
            PyInt_FromLong(IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyLong_AsLong",
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyInt_AsLong(IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyLong_FromString",
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyInt_FromString(string value, IntPtr end, int radix);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyLong_GetMax",
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyInt_GetMax();
#elif PYTHON2
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        private unsafe static extern IntPtr
            PyInt_FromLong(IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyInt_AsLong(IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyInt_FromString(string value, IntPtr end, int radix);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyInt_GetMax();
#endif

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyLong_FromLong(long value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyLong_FromUnsignedLong(uint value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyLong_FromDouble(double value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyLong_FromLongLong(long value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyLong_FromUnsignedLongLong(ulong value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyLong_FromString(string value, IntPtr end, int radix);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyLong_AsLong(IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern uint
            PyLong_AsUnsignedLong(IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern long
            PyLong_AsLongLong(IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern ulong
            PyLong_AsUnsignedLongLong(IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyFloat_FromDouble(double value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyFloat_FromString(IntPtr value, IntPtr junk);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern double
            PyFloat_AsDouble(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Add(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Subtract(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Multiply(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Divide(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_And(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Xor(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Or(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Lshift(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Rshift(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Power(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Remainder(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_InPlaceAdd(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_InPlaceSubtract(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_InPlaceMultiply(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_InPlaceDivide(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_InPlaceAnd(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_InPlaceXor(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_InPlaceOr(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_InPlaceLshift(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_InPlaceRshift(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_InPlacePower(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_InPlaceRemainder(IntPtr o1, IntPtr o2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Negative(IntPtr o1);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Positive(IntPtr o1);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyNumber_Invert(IntPtr o1);


        //====================================================================
        // Python sequence API
        //====================================================================

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern bool
            PySequence_Check(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PySequence_GetItem(IntPtr pointer, int index);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PySequence_SetItem(IntPtr pointer, int index, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PySequence_DelItem(IntPtr pointer, int index);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PySequence_GetSlice(IntPtr pointer, int i1, int i2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PySequence_SetSlice(IntPtr pointer, int i1, int i2, IntPtr v);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PySequence_DelSlice(IntPtr pointer, int i1, int i2);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PySequence_Size(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PySequence_Contains(IntPtr pointer, IntPtr item);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PySequence_Concat(IntPtr pointer, IntPtr other);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PySequence_Repeat(IntPtr pointer, int count);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PySequence_Index(IntPtr pointer, IntPtr item);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PySequence_Count(IntPtr pointer, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PySequence_Tuple(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PySequence_List(IntPtr pointer);


        //====================================================================
        // Python string API
        //====================================================================

#if PYTHON3
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyBytes_FromString(string op);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyBytes_Size(IntPtr op);

        internal static IntPtr PyBytes_AS_STRING(IntPtr ob)
        {
            return ob + BytesOffset.ob_sval;
        }

        internal static IntPtr PyString_FromStringAndSize(string value, int length)
        {
            // copy the string into an unmanaged UTF-8 buffer
            int len = Encoding.UTF8.GetByteCount(value);
            byte[] buffer = new byte[len + 1];
            Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);
            IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            try
            {
                Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
                return PyUnicode_FromStringAndSize(nativeUtf8, length);
            }
            finally
            {
                Marshal.FreeHGlobal(nativeUtf8);
            }
        }

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromStringAndSize(IntPtr value, int size);

#elif PYTHON2

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyString_FromStringAndSize(string value, int size);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyString_AsString",
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyString_AS_STRING(IntPtr op);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyString_Size(IntPtr pointer);
#endif

#if UCS2 && PYTHON3
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromObject(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromEncodedObject(IntPtr ob, IntPtr enc, IntPtr err);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicode_FromKindAndData",
            ExactSpelling = true,
            CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromKindAndString(int kind, string s, int size);

        internal static IntPtr PyUnicode_FromUnicode(string s, int size)
        {
            return PyUnicode_FromKindAndString(2, s, size);
        }

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern int
            PyUnicode_GetSize(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern char*
            PyUnicode_AsUnicode(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicode_AsUnicode",
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_AS_UNICODE(IntPtr op);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromOrdinal(int c);
#elif UCS2 && PYTHON2
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS2_FromObject",
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromObject(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS2_FromEncodedObject",
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromEncodedObject(IntPtr ob, IntPtr enc, IntPtr err);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS2_FromUnicode",
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromUnicode(string s, int size);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS2_GetSize",
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyUnicode_GetSize(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS2_AsUnicode",
            ExactSpelling = true)]
        internal unsafe static extern char*
            PyUnicode_AsUnicode(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS2_AsUnicode",
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_AS_UNICODE(IntPtr op);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS2_FromOrdinal",
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromOrdinal(int c);

#elif UCS4 && PYTHON3
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromObject(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromEncodedObject(IntPtr ob, IntPtr enc, IntPtr err);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicode_FromKindAndData",
            ExactSpelling = true)]
        internal unsafe static extern IntPtr
            PyUnicode_FromKindAndString(int kind,
                IntPtr s,
                int size);

        internal static unsafe IntPtr PyUnicode_FromKindAndString(int kind,
            string s,
            int size)
        {
            var bufLength = Math.Max(s.Length, size) * 4;

            IntPtr mem = Marshal.AllocHGlobal(bufLength);
            try
            {
                fixed (char* ps = s)
                {
                    Encoding.UTF32.GetBytes(ps, s.Length, (byte*) mem, bufLength);
                }

                var result = PyUnicode_FromKindAndString(kind, mem, size);
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(mem);
            }
        }

        internal static IntPtr PyUnicode_FromUnicode(string s, int size)
        {
            return PyUnicode_FromKindAndString(4, s, size);
        }

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyUnicode_GetSize(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true)]
        internal unsafe static extern IntPtr
            PyUnicode_AsUnicode(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicode_AsUnicode",
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_AS_UNICODE(IntPtr op);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromOrdinal(int c);

#elif UCS4 && PYTHON2
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS4_FromObject",
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromObject(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS4_FromEncodedObject",
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromEncodedObject(IntPtr ob, IntPtr enc, IntPtr err);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS4_FromUnicode",
            ExactSpelling = true)]
        internal unsafe static extern IntPtr
            PyUnicode_FromUnicode(IntPtr s, int size);

        internal static unsafe IntPtr PyUnicode_FromUnicode(string s, int size)
        {
            var bufLength = Math.Max(s.Length, size) * 4;

            IntPtr mem = Marshal.AllocHGlobal(bufLength);
            try
            {
                fixed (char* ps = s)
                {
                    Encoding.UTF32.GetBytes(ps, s.Length, (byte*) mem, bufLength);
                }

                var result = PyUnicode_FromUnicode(mem, size);
                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(mem);
            }
        }

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS4_GetSize",
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyUnicode_GetSize(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS4_AsUnicode",
            ExactSpelling = true)]
        internal unsafe static extern IntPtr
            PyUnicode_AsUnicode(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS4_AsUnicode",
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_AS_UNICODE(IntPtr op);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "PyUnicodeUCS4_FromOrdinal",
            ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal unsafe static extern IntPtr
            PyUnicode_FromOrdinal(int c);
#endif

        internal static IntPtr PyUnicode_FromString(string s)
        {
            return PyUnicode_FromUnicode(s, (s.Length));
        }

        internal unsafe static string GetManagedString(IntPtr op)
        {
            IntPtr type = Runtime.PyObject_TYPE(op);

#if PYTHON2 // Python 3 strings are all Unicode
            if (type == Runtime.PyStringType)
            {
                return Marshal.PtrToStringAnsi(
                    PyString_AS_STRING(op),
                    Runtime.PyString_Size(op)
                );
            }
#endif

            if (type == Runtime.PyUnicodeType)
            {
#if UCS4
                IntPtr p = Runtime.PyUnicode_AsUnicode(op);
                int length = Runtime.PyUnicode_GetSize(op);
                int size = length * 4;
                byte[] buffer = new byte[size];
                Marshal.Copy(p, buffer, 0, size);
                return Encoding.UTF32.GetString(buffer, 0, size);
#elif UCS2
                char* p = Runtime.PyUnicode_AsUnicode(op);
                int size = Runtime.PyUnicode_GetSize(op);
                return new String(p, 0, size);
#endif
            }
            return null;
        }


        //====================================================================
        // Python dictionary API
        //====================================================================

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyDict_New();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyDictProxy_New(IntPtr dict);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyDict_GetItem(IntPtr pointer, IntPtr key);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyDict_GetItemString(IntPtr pointer, string key);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyDict_SetItem(IntPtr pointer, IntPtr key, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyDict_SetItemString(IntPtr pointer, string key, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyDict_DelItem(IntPtr pointer, IntPtr key);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyDict_DelItemString(IntPtr pointer, string key);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyMapping_HasKey(IntPtr pointer, IntPtr key);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyDict_Keys(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyDict_Values(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyDict_Items(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyDict_Copy(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyDict_Update(IntPtr pointer, IntPtr other);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyDict_Clear(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyDict_Size(IntPtr pointer);


        //====================================================================
        // Python list API
        //====================================================================

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyList_New(int size);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyList_AsTuple(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyList_GetItem(IntPtr pointer, int index);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyList_SetItem(IntPtr pointer, int index, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyList_Insert(IntPtr pointer, int index, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyList_Append(IntPtr pointer, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyList_Reverse(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyList_Sort(IntPtr pointer);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyList_GetSlice(IntPtr pointer, int start, int end);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyList_SetSlice(IntPtr pointer, int start, int end, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyList_Size(IntPtr pointer);


        //====================================================================
        // Python tuple API
        //====================================================================

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyTuple_New(int size);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyTuple_GetItem(IntPtr pointer, int index);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyTuple_SetItem(IntPtr pointer, int index, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyTuple_GetSlice(IntPtr pointer, int start, int end);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyTuple_Size(IntPtr pointer);


        //====================================================================
        // Python iterator API
        //====================================================================

#if PYTHON2
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern bool
            PyIter_Check(IntPtr pointer);
#elif PYTHON3
        internal static bool
            PyIter_Check(IntPtr pointer)
        {
            IntPtr ob_type = (IntPtr) Marshal.PtrToStructure(pointer + ObjectOffset.ob_type, typeof(IntPtr));
            IntPtr tp_iternext = ob_type + TypeOffset.tp_iternext;
            return tp_iternext != null && tp_iternext != Runtime._PyObject_NextNotImplemented;
        }
#endif

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyIter_Next(IntPtr pointer);


        //====================================================================
        // Python module API
        //====================================================================

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyModule_New(string name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern string
            PyModule_GetName(IntPtr module);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyModule_GetDict(IntPtr module);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern string
            PyModule_GetFilename(IntPtr module);

#if PYTHON3
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyModule_Create2(IntPtr module, int apiver);
#endif

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyImport_Import(IntPtr name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyImport_ImportModule(string name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyImport_ReloadModule(IntPtr module);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyImport_AddModule(string name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyImport_GetModuleDict();

#if PYTHON3
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PySys_SetArgvEx(
                int argc,
                [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] argv,
                int updatepath
            );
#elif PYTHON2
        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PySys_SetArgvEx(
                int argc,
                string[] argv,
                int updatepath
            );
#endif

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PySys_GetObject(string name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PySys_SetObject(string name, IntPtr ob);


        //====================================================================
        // Python type object API
        //====================================================================

        internal static bool PyType_Check(IntPtr ob)
        {
            return PyObject_TypeCheck(ob, Runtime.PyTypeType);
        }

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyType_Modified(IntPtr type);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern bool
            PyType_IsSubtype(IntPtr t1, IntPtr t2);

        internal static bool PyObject_TypeCheck(IntPtr ob, IntPtr tp)
        {
            IntPtr t = Runtime.PyObject_TYPE(ob);
            return (t == tp) || PyType_IsSubtype(t, tp);
        }

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyType_GenericNew(IntPtr type, IntPtr args, IntPtr kw);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyType_GenericAlloc(IntPtr type, int n);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyType_Ready(IntPtr type);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            _PyType_Lookup(IntPtr type, IntPtr name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_GenericGetAttr(IntPtr obj, IntPtr name);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyObject_GenericSetAttr(IntPtr obj, IntPtr name, IntPtr value);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            _PyObject_GetDictPtr(IntPtr obj);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyObject_GC_New(IntPtr tp);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyObject_GC_Del(IntPtr tp);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyObject_GC_Track(IntPtr tp);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyObject_GC_UnTrack(IntPtr tp);


        //====================================================================
        // Python memory API
        //====================================================================

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyMem_Malloc(int size);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyMem_Realloc(IntPtr ptr, int size);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyMem_Free(IntPtr ptr);


        //====================================================================
        // Python exception API
        //====================================================================

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyErr_SetString(IntPtr ob, string message);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyErr_SetObject(IntPtr ob, IntPtr message);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyErr_SetFromErrno(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyErr_SetNone(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyErr_ExceptionMatches(IntPtr exception);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyErr_GivenExceptionMatches(IntPtr ob, IntPtr val);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyErr_NormalizeException(IntPtr ob, IntPtr val, IntPtr tb);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern int
            PyErr_Occurred();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyErr_Fetch(ref IntPtr ob, ref IntPtr val, ref IntPtr tb);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyErr_Restore(IntPtr ob, IntPtr val, IntPtr tb);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyErr_Clear();

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern void
            PyErr_Print();


        //====================================================================
        // Miscellaneous
        //====================================================================

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyMethod_Self(IntPtr ob);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl,
            ExactSpelling = true, CharSet = CharSet.Ansi)]
        internal unsafe static extern IntPtr
            PyMethod_Function(IntPtr ob);


        //=====================================================================
        // InteropContract definitions
        //=====================================================================

        int IPythonRuntimeInterop.UCS => UCS;

        string IPythonRuntimeInterop.PythonDll => dll;

        public string PyVersion { get; } = pyversion;

        public int PyVersionNumber { get; } = pyversionnumber;

#if Py_DEBUG
        void IPythonRuntimeInterop.Py_IncRef(IntPtr ob)
        {
            Py_IncRef(ob);
        }

        void IPythonRuntimeInterop.Py_DecRef(IntPtr ob)
        {
            Py_DecRef(ob);
        }
#else
        public void Py_IncRef(IntPtr ob)
        {
            throw new NotSupportedException();
        }

        public void Py_DecRef(IntPtr ob)
        {
            throw new NotSupportedException();
        }
#endif

        void IPythonRuntimeInterop.Py_Initialize()
        {
            Py_Initialize();
        }

        int IPythonRuntimeInterop.Py_IsInitialized()
        {
            return Py_IsInitialized();
        }

        void IPythonRuntimeInterop.Py_Finalize()
        {
            Py_Finalize();
        }

        IntPtr IPythonRuntimeInterop.Py_NewInterpreter()
        {
            return Py_NewInterpreter();
        }

        void IPythonRuntimeInterop.Py_EndInterpreter(IntPtr threadState)
        {
            Py_EndInterpreter(threadState);
        }

        IntPtr IPythonRuntimeInterop.PyThreadState_New(IntPtr istate)
        {
            return PyThreadState_New(istate);
        }

        IntPtr IPythonRuntimeInterop.PyThreadState_Get()
        {
            return PyThreadState_Get();
        }

        IntPtr IPythonRuntimeInterop.PyThread_get_key_value(IntPtr key)
        {
            return PyThread_get_key_value(key);
        }

        int IPythonRuntimeInterop.PyThread_get_thread_ident()
        {
            return PyThread_get_thread_ident();
        }

        int IPythonRuntimeInterop.PyThread_set_key_value(IntPtr key, IntPtr value)
        {
            return PyThread_set_key_value(key, value);
        }

        IntPtr IPythonRuntimeInterop.PyThreadState_Swap(IntPtr key)
        {
            return PyThreadState_Swap(key);
        }

        IntPtr IPythonRuntimeInterop.PyGILState_Ensure()
        {
            return PyGILState_Ensure();
        }

        void IPythonRuntimeInterop.PyGILState_Release(IntPtr gs)
        {
            PyGILState_Release(gs);
        }

        IntPtr IPythonRuntimeInterop.PyGILState_GetThisThreadState()
        {
            return PyGILState_GetThisThreadState();
        }

        int IPythonRuntimeInterop.Py_Main(int argc, string[] argv)
        {
            return Py_Main(argc, argv);
        }

        void IPythonRuntimeInterop.PyEval_InitThreads()
        {
            PyEval_InitThreads();
        }

        int IPythonRuntimeInterop.PyEval_ThreadsInitialized()
        {
            return PyEval_ThreadsInitialized();
        }

        void IPythonRuntimeInterop.PyEval_AcquireLock()
        {
            PyEval_AcquireLock();
        }

        void IPythonRuntimeInterop.PyEval_ReleaseLock()
        {
            PyEval_ReleaseLock();
        }

        void IPythonRuntimeInterop.PyEval_AcquireThread(IntPtr tstate)
        {
            PyEval_AcquireThread(tstate);
        }

        void IPythonRuntimeInterop.PyEval_ReleaseThread(IntPtr tstate)
        {
            PyEval_ReleaseThread(tstate);
        }

        IntPtr IPythonRuntimeInterop.PyEval_SaveThread()
        {
            return PyEval_SaveThread();
        }

        void IPythonRuntimeInterop.PyEval_RestoreThread(IntPtr tstate)
        {
            PyEval_RestoreThread(tstate);
        }

        IntPtr IPythonRuntimeInterop.PyEval_GetBuiltins()
        {
            return PyEval_GetBuiltins();
        }

        IntPtr IPythonRuntimeInterop.PyEval_GetGlobals()
        {
            return PyEval_GetGlobals();
        }

        IntPtr IPythonRuntimeInterop.PyEval_GetLocals()
        {
            return PyEval_GetLocals();
        }

        string IPythonRuntimeInterop.Py_GetProgramName()
        {
            return Py_GetProgramName();
        }

        void IPythonRuntimeInterop.Py_SetProgramName(string name)
        {
            Py_SetProgramName(name);
        }

        string IPythonRuntimeInterop.Py_GetPythonHome()
        {
            return Py_GetPythonHome();
        }

        void IPythonRuntimeInterop.Py_SetPythonHome(string home)
        {
            Py_SetPythonHome(home);
        }

        string IPythonRuntimeInterop.Py_GetPath()
        {
            return Py_GetPath();
        }

        void IPythonRuntimeInterop.Py_SetPath(string home)
        {
            Py_SetPath(home);
        }

        string IPythonRuntimeInterop.Py_GetVersion()
        {
            return Py_GetVersion();
        }

        string IPythonRuntimeInterop.Py_GetPlatform()
        {
            return Py_GetPlatform();
        }

        string IPythonRuntimeInterop.Py_GetCopyright()
        {
            return Py_GetCopyright();
        }

        string IPythonRuntimeInterop.Py_GetCompiler()
        {
            return Py_GetCompiler();
        }

        string IPythonRuntimeInterop.Py_GetBuildInfo()
        {
            return Py_GetBuildInfo();
        }

        int IPythonRuntimeInterop.PyRun_SimpleString(string code)
        {
            return PyRun_SimpleString(code);
        }

        IntPtr IPythonRuntimeInterop.PyRun_String(string code, IntPtr st, IntPtr globals, IntPtr locals)
        {
            return PyRun_String(code, st, globals, locals);
        }

        IntPtr IPythonRuntimeInterop.Py_CompileString(string code, string file, IntPtr tok)
        {
            return Py_CompileString(code, file, tok);
        }

        IntPtr IPythonRuntimeInterop.PyImport_ExecCodeModule(string name, IntPtr code)
        {
            return PyImport_ExecCodeModule(name, code);
        }

        IntPtr IPythonRuntimeInterop.PyCFunction_NewEx(IntPtr ml, IntPtr self, IntPtr mod)
        {
            return PyCFunction_NewEx(ml, self, mod);
        }

        IntPtr IPythonRuntimeInterop.PyCFunction_Call(IntPtr func, IntPtr args, IntPtr kw)
        {
            return PyCFunction_Call(func, args, kw);
        }

        IntPtr IPythonRuntimeInterop.PyClass_New(IntPtr bases, IntPtr dict, IntPtr name)
        {
            return PyClass_New(bases, dict, name);
        }

        IntPtr IPythonRuntimeInterop.PyInstance_New(IntPtr cls, IntPtr args, IntPtr kw)
        {
            return PyInstance_New(cls, args, kw);
        }

        IntPtr IPythonRuntimeInterop.PyInstance_NewRaw(IntPtr cls, IntPtr dict)
        {
            return PyInstance_NewRaw(cls, dict);
        }

        IntPtr IPythonRuntimeInterop.PyMethod_New(IntPtr func, IntPtr self, IntPtr cls)
        {
            return PyMethod_New(func, self, cls);
        }

        int IPythonRuntimeInterop.PyObject_HasAttrString(IntPtr pointer, string name)
        {
            return PyObject_HasAttrString(pointer, name);
        }

        IntPtr IPythonRuntimeInterop.PyObject_GetAttrString(IntPtr pointer, string name)
        {
            return PyObject_GetAttrString(pointer, name);
        }

        int IPythonRuntimeInterop.PyObject_SetAttrString(IntPtr pointer, string name, IntPtr value)
        {
            return PyObject_SetAttrString(pointer, name, value);
        }

        int IPythonRuntimeInterop.PyObject_HasAttr(IntPtr pointer, IntPtr name)
        {
            return PyObject_HasAttr(pointer, name);
        }

        IntPtr IPythonRuntimeInterop.PyObject_GetAttr(IntPtr pointer, IntPtr name)
        {
            return PyObject_GetAttr(pointer, name);
        }

        int IPythonRuntimeInterop.PyObject_SetAttr(IntPtr pointer, IntPtr name, IntPtr value)
        {
            return PyObject_SetAttr(pointer, name, value);
        }

        IntPtr IPythonRuntimeInterop.PyObject_GetItem(IntPtr pointer, IntPtr key)
        {
            return PyObject_GetItem(pointer, key);
        }

        int IPythonRuntimeInterop.PyObject_SetItem(IntPtr pointer, IntPtr key, IntPtr value)
        {
            return PyObject_SetItem(pointer, key, value);
        }

        int IPythonRuntimeInterop.PyObject_DelItem(IntPtr pointer, IntPtr key)
        {
            return PyObject_DelItem(pointer, key);
        }

        IntPtr IPythonRuntimeInterop.PyObject_GetIter(IntPtr op)
        {
            return PyObject_GetIter(op);
        }

        IntPtr IPythonRuntimeInterop.PyObject_Call(IntPtr pointer, IntPtr args, IntPtr kw)
        {
            return PyObject_Call(pointer, args, kw);
        }

        IntPtr IPythonRuntimeInterop.PyObject_CallObject(IntPtr pointer, IntPtr args)
        {
            return PyObject_CallObject(pointer, args);
        }

        int IPythonRuntimeInterop.PyObject_Compare(IntPtr value1, IntPtr value2)
        {
            return PyObject_Compare(value1, value2);
        }

        int IPythonRuntimeInterop.PyObject_RichCompareBool(IntPtr value1, IntPtr value2, int opid)
        {
#if PYTHON3
            return PyObject_RichCompareBool(value1, value2, opid);
#else
            throw new NotSupportedException("Supported in Python 3.x and higher");
#endif
        }

        int IPythonRuntimeInterop.PyObject_IsInstance(IntPtr ob, IntPtr type)
        {
            return PyObject_IsInstance(ob, type);
        }

        int IPythonRuntimeInterop.PyObject_IsSubclass(IntPtr ob, IntPtr type)
        {
            return PyObject_IsSubclass(ob, type);
        }

        int IPythonRuntimeInterop.PyCallable_Check(IntPtr pointer)
        {
            return PyCallable_Check(pointer);
        }

        int IPythonRuntimeInterop.PyObject_IsTrue(IntPtr pointer)
        {
            return PyObject_IsTrue(pointer);
        }

        int IPythonRuntimeInterop.PyObject_Not(IntPtr pointer)
        {
            return PyObject_Not(pointer);
        }

        int IPythonRuntimeInterop.PyObject_Size(IntPtr pointer)
        {
            return PyObject_Size(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyObject_Hash(IntPtr op)
        {
            return PyObject_Hash(op);
        }

        IntPtr IPythonRuntimeInterop.PyObject_Repr(IntPtr pointer)
        {
            return PyObject_Repr(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyObject_Str(IntPtr pointer)
        {
            return PyObject_Str(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyObject_Unicode(IntPtr pointer)
        {
            return PyObject_Unicode(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyObject_Dir(IntPtr pointer)
        {
            return PyObject_Dir(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Int(IntPtr ob)
        {
            return PyNumber_Int(ob);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Long(IntPtr ob)
        {
            return PyNumber_Long(ob);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Float(IntPtr ob)
        {
            return PyNumber_Float(ob);
        }

        bool IPythonRuntimeInterop.PyNumber_Check(IntPtr ob)
        {
            return PyNumber_Check(ob);
        }

        IntPtr IPythonRuntimeInterop.PyInt_FromLong(IntPtr value)
        {
            return PyInt_FromLong(value);
        }

        int IPythonRuntimeInterop.PyInt_AsLong(IntPtr value)
        {
            return PyInt_AsLong(value);
        }

        IntPtr IPythonRuntimeInterop.PyInt_FromString(string value, IntPtr end, int radix)
        {
            return PyInt_FromString(value, end, radix);
        }

        int IPythonRuntimeInterop.PyInt_GetMax()
        {
            return PyInt_GetMax();
        }

        IntPtr IPythonRuntimeInterop.PyLong_FromLong(long value)
        {
            return PyLong_FromLong(value);
        }

        IntPtr IPythonRuntimeInterop.PyLong_FromUnsignedLong(uint value)
        {
            return PyLong_FromUnsignedLong(value);
        }

        IntPtr IPythonRuntimeInterop.PyLong_FromDouble(double value)
        {
            return PyLong_FromDouble(value);
        }

        IntPtr IPythonRuntimeInterop.PyLong_FromLongLong(long value)
        {
            return PyLong_FromLongLong(value);
        }

        IntPtr IPythonRuntimeInterop.PyLong_FromUnsignedLongLong(ulong value)
        {
            return PyLong_FromUnsignedLongLong(value);
        }

        IntPtr IPythonRuntimeInterop.PyLong_FromString(string value, IntPtr end, int radix)
        {
            return PyLong_FromString(value, end, radix);
        }

        int IPythonRuntimeInterop.PyLong_AsLong(IntPtr value)
        {
            return PyLong_AsLong(value);
        }

        uint IPythonRuntimeInterop.PyLong_AsUnsignedLong(IntPtr value)
        {
            return PyLong_AsUnsignedLong(value);
        }

        long IPythonRuntimeInterop.PyLong_AsLongLong(IntPtr value)
        {
            return PyLong_AsLongLong(value);
        }

        ulong IPythonRuntimeInterop.PyLong_AsUnsignedLongLong(IntPtr value)
        {
            return PyLong_AsUnsignedLongLong(value);
        }

        IntPtr IPythonRuntimeInterop.PyFloat_FromDouble(double value)
        {
            return PyFloat_FromDouble(value);
        }

        IntPtr IPythonRuntimeInterop.PyFloat_FromString(IntPtr value, IntPtr junk)
        {
            return PyFloat_FromString(value, junk);
        }

        double IPythonRuntimeInterop.PyFloat_AsDouble(IntPtr ob)
        {
            return PyFloat_AsDouble(ob);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Add(IntPtr o1, IntPtr o2)
        {
            return PyNumber_Add(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Subtract(IntPtr o1, IntPtr o2)
        {
            return PyNumber_Subtract(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Multiply(IntPtr o1, IntPtr o2)
        {
            return PyNumber_Multiply(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Divide(IntPtr o1, IntPtr o2)
        {
            return PyNumber_Divide(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_And(IntPtr o1, IntPtr o2)
        {
            return PyNumber_And(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Xor(IntPtr o1, IntPtr o2)
        {
            return PyNumber_Xor(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Or(IntPtr o1, IntPtr o2)
        {
            return PyNumber_Or(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Lshift(IntPtr o1, IntPtr o2)
        {
            return PyNumber_Lshift(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Rshift(IntPtr o1, IntPtr o2)
        {
            return PyNumber_Rshift(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Power(IntPtr o1, IntPtr o2)
        {
            return PyNumber_Power(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Remainder(IntPtr o1, IntPtr o2)
        {
            return PyNumber_Remainder(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_InPlaceAdd(IntPtr o1, IntPtr o2)
        {
            return PyNumber_InPlaceAdd(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_InPlaceSubtract(IntPtr o1, IntPtr o2)
        {
            return PyNumber_InPlaceSubtract(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_InPlaceMultiply(IntPtr o1, IntPtr o2)
        {
            return PyNumber_InPlaceMultiply(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_InPlaceDivide(IntPtr o1, IntPtr o2)
        {
            return PyNumber_InPlaceDivide(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_InPlaceAnd(IntPtr o1, IntPtr o2)
        {
            return PyNumber_InPlaceAnd(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_InPlaceXor(IntPtr o1, IntPtr o2)
        {
            return PyNumber_InPlaceXor(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_InPlaceOr(IntPtr o1, IntPtr o2)
        {
            return PyNumber_InPlaceOr(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_InPlaceLshift(IntPtr o1, IntPtr o2)
        {
            return PyNumber_InPlaceLshift(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_InPlaceRshift(IntPtr o1, IntPtr o2)
        {
            return PyNumber_InPlaceRshift(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_InPlacePower(IntPtr o1, IntPtr o2)
        {
            return PyNumber_InPlacePower(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_InPlaceRemainder(IntPtr o1, IntPtr o2)
        {
            return PyNumber_InPlaceRemainder(o1, o2);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Negative(IntPtr o1)
        {
            return PyNumber_Negative(o1);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Positive(IntPtr o1)
        {
            return PyNumber_Positive(o1);
        }

        IntPtr IPythonRuntimeInterop.PyNumber_Invert(IntPtr o1)
        {
            return PyNumber_Invert(o1);
        }

        bool IPythonRuntimeInterop.PySequence_Check(IntPtr pointer)
        {
            return PySequence_Check(pointer);
        }

        IntPtr IPythonRuntimeInterop.PySequence_GetItem(IntPtr pointer, int index)
        {
            return PySequence_GetItem(pointer, index);
        }

        int IPythonRuntimeInterop.PySequence_SetItem(IntPtr pointer, int index, IntPtr value)
        {
            return PySequence_SetItem(pointer, index, value);
        }

        int IPythonRuntimeInterop.PySequence_DelItem(IntPtr pointer, int index)
        {
            return PySequence_DelItem(pointer, index);
        }

        IntPtr IPythonRuntimeInterop.PySequence_GetSlice(IntPtr pointer, int i1, int i2)
        {
            return PySequence_GetSlice(pointer, i1, i2);
        }

        int IPythonRuntimeInterop.PySequence_SetSlice(IntPtr pointer, int i1, int i2, IntPtr v)
        {
            return PySequence_SetSlice(pointer, i1, i2, v);
        }

        int IPythonRuntimeInterop.PySequence_DelSlice(IntPtr pointer, int i1, int i2)
        {
            return PySequence_DelSlice(pointer, i1, i2);
        }

        int IPythonRuntimeInterop.PySequence_Size(IntPtr pointer)
        {
            return PySequence_Size(pointer);
        }

        int IPythonRuntimeInterop.PySequence_Contains(IntPtr pointer, IntPtr item)
        {
            return PySequence_Contains(pointer, item);
        }

        IntPtr IPythonRuntimeInterop.PySequence_Concat(IntPtr pointer, IntPtr other)
        {
            return PySequence_Concat(pointer, other);
        }

        IntPtr IPythonRuntimeInterop.PySequence_Repeat(IntPtr pointer, int count)
        {
            return PySequence_Repeat(pointer, count);
        }

        int IPythonRuntimeInterop.PySequence_Index(IntPtr pointer, IntPtr item)
        {
            return PySequence_Index(pointer, item);
        }

        int IPythonRuntimeInterop.PySequence_Count(IntPtr pointer, IntPtr value)
        {
            return PySequence_Count(pointer, value);
        }

        IntPtr IPythonRuntimeInterop.PySequence_Tuple(IntPtr pointer)
        {
            return PySequence_Tuple(pointer);
        }

        IntPtr IPythonRuntimeInterop.PySequence_List(IntPtr pointer)
        {
            return PySequence_List(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyBytes_FromString(string op)
        {
#if PYTHON3
            return PyBytes_FromString(op);
#elif PYTHON2
            throw new NotSupportedException();
#endif
        }

        int IPythonRuntimeInterop.PyBytes_Size(IntPtr op)
        {
#if PYTHON3
            return PyBytes_Size(op);
#elif PYTHON2
            throw new NotSupportedException();
#endif
        }

        IntPtr IPythonRuntimeInterop.PyBytes_AS_STRING(IntPtr ob)
        {
#if PYTHON3
            return PyBytes_AS_STRING(ob);
#elif PYTHON2
            throw new NotSupportedException();
#endif
        }

        IntPtr IPythonRuntimeInterop.PyUnicode_FromStringAndSize(IntPtr value, int size)
        {
#if PYTHON3
            return PyUnicode_FromStringAndSize(value, size);
#elif PYTHON2
            throw new NotSupportedException();
#endif
        }

        IntPtr IPythonRuntimeInterop.PyString_FromStringAndSize(string value, int size)
        {
            return PyString_FromStringAndSize(value, size);
        }

        IntPtr IPythonRuntimeInterop.PyString_AS_STRING(IntPtr op)
        {
#if PYTHON3
            throw new NotSupportedException();
#elif PYTHON2
            return PyString_AS_STRING(op);
#endif
        }

        int IPythonRuntimeInterop.PyString_Size(IntPtr pointer)
        {
#if PYTHON3
            throw new NotSupportedException();
#elif PYTHON2
            return PyString_Size(pointer);
#endif
        }

        IntPtr IPythonRuntimeInterop.PyUnicode_FromObject(IntPtr ob)
        {
            return PyUnicode_FromObject(ob);
        }

        IntPtr IPythonRuntimeInterop.PyUnicode_FromEncodedObject(IntPtr ob, IntPtr enc, IntPtr err)
        {
            return PyUnicode_FromEncodedObject(ob, enc, err);
        }

        IntPtr IPythonRuntimeInterop.PyUnicode_FromKindAndString(int kind, string s, int size)
        {
#if PYTHON3
            return PyUnicode_FromKindAndString(kind, s, size);
#elif PYTHON2
            throw new NotSupportedException();
#endif
        }

        IntPtr IPythonRuntimeInterop.PyUnicode_FromUnicode(string s, int size)
        {
            return PyUnicode_FromUnicode(s, size);
        }

        int IPythonRuntimeInterop.PyUnicode_GetSize(IntPtr ob)
        {
            return PyUnicode_GetSize(ob);
        }

        unsafe char* IPythonRuntimeInterop.PyUnicode_AsUnicode(IntPtr ob)
        {
            return PyUnicode_AsUnicode(ob);
        }

        IntPtr IPythonRuntimeInterop.PyUnicode_AS_UNICODE(IntPtr op)
        {
            return PyUnicode_AS_UNICODE(op);
        }

        IntPtr IPythonRuntimeInterop.PyUnicode_FromOrdinal(int c)
        {
            return PyUnicode_FromOrdinal(c);
        }

        IntPtr IPythonRuntimeInterop.PyUnicode_FromString(string s)
        {
            return PyUnicode_FromString(s);
        }

        string IPythonRuntimeInterop.GetManagedString(IntPtr op)
        {
            return GetManagedString(op);
        }

        IntPtr IPythonRuntimeInterop.PyUnicode_FromKindAndString(int kind, IntPtr s, int size)
        {
#if PYTHON3 && UCS4
            return PyUnicode_FromKindAndString(kind, s, size);
#else
            throw new NotSupportedException();
#endif
        }

        IntPtr IPythonRuntimeInterop.PyDict_New()
        {
            return PyDict_New();
        }

        IntPtr IPythonRuntimeInterop.PyDictProxy_New(IntPtr dict)
        {
            return PyDictProxy_New(dict);
        }

        IntPtr IPythonRuntimeInterop.PyDict_GetItem(IntPtr pointer, IntPtr key)
        {
            return PyDict_GetItem(pointer, key);
        }

        IntPtr IPythonRuntimeInterop.PyDict_GetItemString(IntPtr pointer, string key)
        {
            return PyDict_GetItemString(pointer, key);
        }

        int IPythonRuntimeInterop.PyDict_SetItem(IntPtr pointer, IntPtr key, IntPtr value)
        {
            return PyDict_SetItem(pointer, key, value);
        }

        int IPythonRuntimeInterop.PyDict_SetItemString(IntPtr pointer, string key, IntPtr value)
        {
            return PyDict_SetItemString(pointer, key, value);
        }

        int IPythonRuntimeInterop.PyDict_DelItem(IntPtr pointer, IntPtr key)
        {
            return PyDict_DelItem(pointer, key);
        }

        int IPythonRuntimeInterop.PyDict_DelItemString(IntPtr pointer, string key)
        {
            return PyDict_DelItemString(pointer, key);
        }

        int IPythonRuntimeInterop.PyMapping_HasKey(IntPtr pointer, IntPtr key)
        {
            return PyMapping_HasKey(pointer, key);
        }

        IntPtr IPythonRuntimeInterop.PyDict_Keys(IntPtr pointer)
        {
            return PyDict_Keys(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyDict_Values(IntPtr pointer)
        {
            return PyDict_Values(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyDict_Items(IntPtr pointer)
        {
            return PyDict_Items(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyDict_Copy(IntPtr pointer)
        {
            return PyDict_Copy(pointer);
        }

        int IPythonRuntimeInterop.PyDict_Update(IntPtr pointer, IntPtr other)
        {
            return PyDict_Update(pointer, other);
        }

        void IPythonRuntimeInterop.PyDict_Clear(IntPtr pointer)
        {
            PyDict_Clear(pointer);
        }

        int IPythonRuntimeInterop.PyDict_Size(IntPtr pointer)
        {
            return PyDict_Size(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyList_New(int size)
        {
            return PyList_New(size);
        }

        IntPtr IPythonRuntimeInterop.PyList_AsTuple(IntPtr pointer)
        {
            return PyList_AsTuple(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyList_GetItem(IntPtr pointer, int index)
        {
            return PyList_GetItem(pointer, index);
        }

        int IPythonRuntimeInterop.PyList_SetItem(IntPtr pointer, int index, IntPtr value)
        {
            return PyList_SetItem(pointer, index, value);
        }

        int IPythonRuntimeInterop.PyList_Insert(IntPtr pointer, int index, IntPtr value)
        {
            return PyList_Insert(pointer, index, value);
        }

        int IPythonRuntimeInterop.PyList_Append(IntPtr pointer, IntPtr value)
        {
            return PyList_Append(pointer, value);
        }

        int IPythonRuntimeInterop.PyList_Reverse(IntPtr pointer)
        {
            return PyList_Reverse(pointer);
        }

        int IPythonRuntimeInterop.PyList_Sort(IntPtr pointer)
        {
            return PyList_Sort(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyList_GetSlice(IntPtr pointer, int start, int end)
        {
            return PyList_GetSlice(pointer, start, end);
        }

        int IPythonRuntimeInterop.PyList_SetSlice(IntPtr pointer, int start, int end, IntPtr value)
        {
            return PyList_SetSlice(pointer, start, end, value);
        }

        int IPythonRuntimeInterop.PyList_Size(IntPtr pointer)
        {
            return PyList_Size(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyTuple_New(int size)
        {
            return PyTuple_New(size);
        }

        IntPtr IPythonRuntimeInterop.PyTuple_GetItem(IntPtr pointer, int index)
        {
            return PyTuple_GetItem(pointer, index);
        }

        int IPythonRuntimeInterop.PyTuple_SetItem(IntPtr pointer, int index, IntPtr value)
        {
            return PyTuple_SetItem(pointer, index, value);
        }

        IntPtr IPythonRuntimeInterop.PyTuple_GetSlice(IntPtr pointer, int start, int end)
        {
            return PyTuple_GetSlice(pointer, start, end);
        }

        int IPythonRuntimeInterop.PyTuple_Size(IntPtr pointer)
        {
            return PyTuple_Size(pointer);
        }

        bool IPythonRuntimeInterop.PyIter_Check(IntPtr pointer)
        {
            return PyIter_Check(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyIter_Next(IntPtr pointer)
        {
            return PyIter_Next(pointer);
        }

        IntPtr IPythonRuntimeInterop.PyModule_New(string name)
        {
            return PyModule_New(name);
        }

        string IPythonRuntimeInterop.PyModule_GetName(IntPtr module)
        {
            return PyModule_GetName(module);
        }

        IntPtr IPythonRuntimeInterop.PyModule_GetDict(IntPtr module)
        {
            return PyModule_GetDict(module);
        }

        string IPythonRuntimeInterop.PyModule_GetFilename(IntPtr module)
        {
            return PyModule_GetFilename(module);
        }

        IntPtr IPythonRuntimeInterop.PyModule_Create2(IntPtr module, int apiver)
        {
#if PYTHON3
            return PyModule_Create2(module, apiver);
#elif PYTHON2
            throw new NotSupportedException();
#endif
        }

        IntPtr IPythonRuntimeInterop.PyImport_Import(IntPtr name)
        {
            return PyImport_Import(name);
        }

        IntPtr IPythonRuntimeInterop.PyImport_ImportModule(string name)
        {
            return PyImport_ImportModule(name);
        }

        IntPtr IPythonRuntimeInterop.PyImport_ReloadModule(IntPtr module)
        {
            return PyImport_ReloadModule(module);
        }

        IntPtr IPythonRuntimeInterop.PyImport_AddModule(string name)
        {
            return PyImport_AddModule(name);
        }

        IntPtr IPythonRuntimeInterop.PyImport_GetModuleDict()
        {
            return PyImport_GetModuleDict();
        }

        void IPythonRuntimeInterop.PySys_SetArgvEx(int argc, string[] argv, int updatepath)
        {
            PySys_SetArgvEx(argc, argv, updatepath);
        }

        IntPtr IPythonRuntimeInterop.PySys_GetObject(string name)
        {
            return PySys_GetObject(name);
        }

        int IPythonRuntimeInterop.PySys_SetObject(string name, IntPtr ob)
        {
            return PySys_SetObject(name, ob);
        }

        void IPythonRuntimeInterop.PyType_Modified(IntPtr type)
        {
            PyType_Modified(type);
        }

        bool IPythonRuntimeInterop.PyType_IsSubtype(IntPtr t1, IntPtr t2)
        {
            return PyType_IsSubtype(t1, t2);
        }

        IntPtr IPythonRuntimeInterop.PyType_GenericNew(IntPtr type, IntPtr args, IntPtr kw)
        {
            return PyType_GenericNew(type, args, kw);
        }

        IntPtr IPythonRuntimeInterop.PyType_GenericAlloc(IntPtr type, int n)
        {
            return PyType_GenericAlloc(type, n);
        }

        int IPythonRuntimeInterop.PyType_Ready(IntPtr type)
        {
            return PyType_Ready(type);
        }

        IntPtr IPythonRuntimeInterop._PyType_Lookup(IntPtr type, IntPtr name)
        {
            return _PyType_Lookup(type, name);
        }

        IntPtr IPythonRuntimeInterop.PyObject_GenericGetAttr(IntPtr obj, IntPtr name)
        {
            return PyObject_GenericGetAttr(obj, name);
        }

        int IPythonRuntimeInterop.PyObject_GenericSetAttr(IntPtr obj, IntPtr name, IntPtr value)
        {
            return PyObject_GenericSetAttr(obj, name, value);
        }

        IntPtr IPythonRuntimeInterop._PyObject_GetDictPtr(IntPtr obj)
        {
            return _PyObject_GetDictPtr(obj);
        }

        IntPtr IPythonRuntimeInterop.PyObject_GC_New(IntPtr tp)
        {
            return PyObject_GC_New(tp);
        }

        void IPythonRuntimeInterop.PyObject_GC_Del(IntPtr tp)
        {
            PyObject_GC_Del(tp);
        }

        void IPythonRuntimeInterop.PyObject_GC_Track(IntPtr tp)
        {
            PyObject_GC_Track(tp);
        }

        void IPythonRuntimeInterop.PyObject_GC_UnTrack(IntPtr tp)
        {
            PyObject_GC_UnTrack(tp);
        }

        IntPtr IPythonRuntimeInterop.PyMem_Malloc(int size)
        {
            return PyMem_Malloc(size);
        }

        IntPtr IPythonRuntimeInterop.PyMem_Realloc(IntPtr ptr, int size)
        {
            return PyMem_Realloc(ptr, size);
        }

        void IPythonRuntimeInterop.PyMem_Free(IntPtr ptr)
        {
            PyMem_Free(ptr);
        }

        void IPythonRuntimeInterop.PyErr_SetString(IntPtr ob, string message)
        {
            PyErr_SetString(ob, message);
        }

        void IPythonRuntimeInterop.PyErr_SetObject(IntPtr ob, IntPtr message)
        {
            PyErr_SetObject(ob, message);
        }

        IntPtr IPythonRuntimeInterop.PyErr_SetFromErrno(IntPtr ob)
        {
            return PyErr_SetFromErrno(ob);
        }

        void IPythonRuntimeInterop.PyErr_SetNone(IntPtr ob)
        {
            PyErr_SetNone(ob);
        }

        int IPythonRuntimeInterop.PyErr_ExceptionMatches(IntPtr exception)
        {
            return PyErr_ExceptionMatches(exception);
        }

        int IPythonRuntimeInterop.PyErr_GivenExceptionMatches(IntPtr ob, IntPtr val)
        {
            return PyErr_GivenExceptionMatches(ob, val);
        }

        void IPythonRuntimeInterop.PyErr_NormalizeException(IntPtr ob, IntPtr val, IntPtr tb)
        {
            PyErr_NormalizeException(ob, val, tb);
        }

        int IPythonRuntimeInterop.PyErr_Occurred()
        {
            return PyErr_Occurred();
        }

        void IPythonRuntimeInterop.PyErr_Fetch(ref IntPtr ob, ref IntPtr val, ref IntPtr tb)
        {
            PyErr_Fetch(ref ob, ref val, ref tb);
        }

        void IPythonRuntimeInterop.PyErr_Restore(IntPtr ob, IntPtr val, IntPtr tb)
        {
            PyErr_Restore(ob, val, tb);
        }

        void IPythonRuntimeInterop.PyErr_Clear()
        {
            PyErr_Clear();
        }

        void IPythonRuntimeInterop.PyErr_Print()
        {
            PyErr_Print();
        }

        IntPtr IPythonRuntimeInterop.PyMethod_Self(IntPtr ob)
        {
            return PyMethod_Self(ob);
        }

        IntPtr IPythonRuntimeInterop.PyMethod_Function(IntPtr ob)
        {
            return PyMethod_Function(ob);
        }
    }
}
