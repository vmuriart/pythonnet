using System;
using System.Runtime.InteropServices;

using ReflectionBridge.Extensions;

namespace Python.Runtime
{
    using Python.Runtime.InteropContracts;

    static partial class NativeMethods
    {
        public static IntPtr LoadLibrary(string dllToLoad)
        {
            return _interop.LoadLibrary(dllToLoad);
        }

        public static IntPtr GetProcAddress(IntPtr hModule, string procedureName)
        {
            return _interop.GetProcAddress(hModule, procedureName);
        }

        public static bool FreeLibrary(IntPtr hModule)
        {
            return _interop.FreeLibrary(hModule);
        }
    }

    /// <summary>
    /// Encapsulates the low-level Python C API. Note that it is
    /// the responsibility of the caller to have acquired the GIL
    /// before calling any of these methods.
    /// </summary>
    public partial class Runtime
    {
        private static IPythonRuntimeInterop _interop;

        public static int UCS;

        public static string pyversion;

        public static int pyversionnumber;

        public static bool IsPython3;

        public static bool IsPython2;

        public static bool IsWindows;

        public static bool IsPosix;

        public static bool IsLinux;

        public static bool IsOSX;

        public static bool IsPyDebug;

        public static string dll;

        // set to true when python is finalizing
        internal static Object IsFinalizingLock = new Object();
        internal static bool IsFinalizing = false;

        internal static bool is32bit;

        /// <summary>
        /// Intitialize the runtime...
        /// </summary>
        internal static void Initialize()
        {
            is32bit = IntPtr.Size == 4;

            if (0 == Runtime.Py_IsInitialized())
            {
                Runtime.Py_Initialize();
            }

            if (0 == Runtime.PyEval_ThreadsInitialized())
            {
                Runtime.PyEval_InitThreads();
            }

            IntPtr op;
            IntPtr dict;
            if (IsPython3)
            {
                op = Runtime.PyImport_ImportModule("builtins");
                dict = Runtime.PyObject_GetAttrString(op, "__dict__");
            }
            else
            {
                dict = Runtime.PyImport_GetModuleDict();
                op = Runtime.PyDict_GetItemString(dict, "__builtin__");
            }

            PyNotImplemented = Runtime.PyObject_GetAttrString(op, "NotImplemented");
            PyBaseObjectType = Runtime.PyObject_GetAttrString(op, "object");

            PyModuleType = Runtime.PyObject_Type(op);
            PyNone = Runtime.PyObject_GetAttrString(op, "None");
            PyTrue = Runtime.PyObject_GetAttrString(op, "True");
            PyFalse = Runtime.PyObject_GetAttrString(op, "False");

            PyBoolType = Runtime.PyObject_Type(PyTrue);
            PyNoneType = Runtime.PyObject_Type(PyNone);
            PyTypeType = Runtime.PyObject_Type(PyNoneType);

            op = Runtime.PyObject_GetAttrString(dict, "keys");
            PyMethodType = Runtime.PyObject_Type(op);
            Runtime.XDecref(op);

            // For some arcane reason, builtins.__dict__.__setitem__ is *not*
            // a wrapper_descriptor, even though dict.__setitem__ is.
            //
            // object.__init__ seems safe, though.
            op = Runtime.PyObject_GetAttrString(PyBaseObjectType, "__init__");
            PyWrapperDescriptorType = Runtime.PyObject_Type(op);
            Runtime.XDecref(op);

            if (IsPython3)
            {
                Runtime.XDecref(dict);
            }

            op = Runtime.PyString_FromString("string");
            PyStringType = Runtime.PyObject_Type(op);
            Runtime.XDecref(op);

            op = Runtime.PyUnicode_FromString("unicode");
            PyUnicodeType = Runtime.PyObject_Type(op);
            Runtime.XDecref(op);

            if (IsPython3)
            {
                op = Runtime.PyBytes_FromString("bytes");
                PyBytesType = Runtime.PyObject_Type(op);
                Runtime.XDecref(op);
            }

            op = Runtime.PyTuple_New(0);
            PyTupleType = Runtime.PyObject_Type(op);
            Runtime.XDecref(op);

            op = Runtime.PyList_New(0);
            PyListType = Runtime.PyObject_Type(op);
            Runtime.XDecref(op);

            op = Runtime.PyDict_New();
            PyDictType = Runtime.PyObject_Type(op);
            Runtime.XDecref(op);

            op = Runtime.PyInt_FromInt32(0);
            PyIntType = Runtime.PyObject_Type(op);
            Runtime.XDecref(op);

            op = Runtime.PyLong_FromLong(0);
            PyLongType = Runtime.PyObject_Type(op);
            Runtime.XDecref(op);

            op = Runtime.PyFloat_FromDouble(0);
            PyFloatType = Runtime.PyObject_Type(op);
            Runtime.XDecref(op);

            if (IsPython3)
            {
                PyClassType = IntPtr.Zero;
                PyInstanceType = IntPtr.Zero;
            }
            else
            {
                IntPtr s = Runtime.PyString_FromString("_temp");
                IntPtr d = Runtime.PyDict_New();

                IntPtr c = Runtime.PyClass_New(IntPtr.Zero, d, s);
                PyClassType = Runtime.PyObject_Type(c);

                IntPtr i = Runtime.PyInstance_New(c, IntPtr.Zero, IntPtr.Zero);
                PyInstanceType = Runtime.PyObject_Type(i);

                Runtime.XDecref(s);
                Runtime.XDecref(i);
                Runtime.XDecref(c);
                Runtime.XDecref(d);
            }

            Error = new IntPtr(-1);
            if (IsPython3)
            {
                IntPtr dll = IntPtr.Zero;
                if ("__Internal" != Runtime.dll)
                {
                    NativeMethods.LoadLibrary(Runtime.dll);
                }

                _PyObject_NextNotImplemented = NativeMethods.GetProcAddress(dll, "_PyObject_NextNotImplemented");
                if (IsPosix)
                {
                    if (IntPtr.Zero != dll)
                    {
                        NativeMethods.FreeLibrary(dll);
                    }
                }
            }

            // Initialize modules that depend on the runtime class.
            AssemblyManager.Initialize();
            PyCLRMetaType = MetaType.Initialize();
            Exceptions.Initialize();
            ImportHook.Initialize();

            // Need to add the runtime directory to sys.path so that we
            // can find built-in assemblies like System.Data, et. al.
            string rtdir = typeof(string).GetAssembly().Location;
            IntPtr path = Runtime.PySys_GetObject("path");
            IntPtr item = Runtime.PyString_FromString(rtdir);
            Runtime.PyList_Append(path, item);
            Runtime.XDecref(item);
            AssemblyManager.UpdatePath();
        }

        internal static void Shutdown()
        {
            AssemblyManager.Shutdown();
            Exceptions.Shutdown();
            ImportHook.Shutdown();
            Py_Finalize();
        }

        // called *without* the GIL aquired by clr._AtExit
        internal static int AtExit()
        {
            lock (IsFinalizingLock)
            {
                IsFinalizing = true;
            }
            return 0;
        }

        internal static IntPtr Py_single_input = (IntPtr)256;
        internal static IntPtr Py_file_input = (IntPtr)257;
        internal static IntPtr Py_eval_input = (IntPtr)258;

        internal static IntPtr PyBaseObjectType;
        internal static IntPtr PyModuleType;
        internal static IntPtr PyClassType;
        internal static IntPtr PyInstanceType;
        internal static IntPtr PyCLRMetaType;
        internal static IntPtr PyMethodType;
        internal static IntPtr PyWrapperDescriptorType;

        internal static IntPtr PyUnicodeType;
        internal static IntPtr PyStringType;
        internal static IntPtr PyTupleType;
        internal static IntPtr PyListType;
        internal static IntPtr PyDictType;
        internal static IntPtr PyIntType;
        internal static IntPtr PyLongType;
        internal static IntPtr PyFloatType;
        internal static IntPtr PyBoolType;
        internal static IntPtr PyNoneType;
        internal static IntPtr PyTypeType;

        internal static IntPtr PyBytesType;
        internal static IntPtr _PyObject_NextNotImplemented;

        internal static IntPtr PyNotImplemented;
        internal const int Py_LT = 0;
        internal const int Py_LE = 1;
        internal const int Py_EQ = 2;
        internal const int Py_NE = 3;
        internal const int Py_GT = 4;
        internal const int Py_GE = 5;

        internal static IntPtr PyTrue;
        internal static IntPtr PyFalse;
        internal static IntPtr PyNone;
        internal static IntPtr Error;

        internal static IntPtr GetBoundArgTuple(IntPtr obj, IntPtr args)
        {
            if (Runtime.PyObject_TYPE(args) != Runtime.PyTupleType)
            {
                Exceptions.SetError(Exceptions.TypeError, "tuple expected");
                return IntPtr.Zero;
            }
            int size = Runtime.PyTuple_Size(args);
            IntPtr items = Runtime.PyTuple_New(size + 1);
            Runtime.PyTuple_SetItem(items, 0, obj);
            Runtime.XIncref(obj);

            for (int i = 0; i < size; i++)
            {
                IntPtr item = Runtime.PyTuple_GetItem(args, i);
                Runtime.XIncref(item);
                Runtime.PyTuple_SetItem(items, i + 1, item);
            }

            return items;
        }


        internal static IntPtr ExtendTuple(IntPtr t, params IntPtr[] args)
        {
            int size = Runtime.PyTuple_Size(t);
            int add = args.Length;
            IntPtr item;

            IntPtr items = Runtime.PyTuple_New(size + add);
            for (int i = 0; i < size; i++)
            {
                item = Runtime.PyTuple_GetItem(t, i);
                Runtime.XIncref(item);
                Runtime.PyTuple_SetItem(items, i, item);
            }

            for (int n = 0; n < add; n++)
            {
                item = args[n];
                Runtime.XIncref(item);
                Runtime.PyTuple_SetItem(items, size + n, item);
            }

            return items;
        }

        internal static Type[] PythonArgsToTypeArray(IntPtr arg)
        {
            return PythonArgsToTypeArray(arg, false);
        }

        internal static Type[] PythonArgsToTypeArray(IntPtr arg, bool mangleObjects)
        {
            // Given a PyObject * that is either a single type object or a
            // tuple of (managed or unmanaged) type objects, return a Type[]
            // containing the CLR Type objects that map to those types.
            IntPtr args = arg;
            bool free = false;

            if (!Runtime.PyTuple_Check(arg))
            {
                args = Runtime.PyTuple_New(1);
                Runtime.XIncref(arg);
                Runtime.PyTuple_SetItem(args, 0, arg);
                free = true;
            }

            int n = Runtime.PyTuple_Size(args);
            Type[] types = new Type[n];
            Type t = null;

            for (int i = 0; i < n; i++)
            {
                IntPtr op = Runtime.PyTuple_GetItem(args, i);
                if (mangleObjects && (!Runtime.PyType_Check(op)))
                {
                    op = Runtime.PyObject_TYPE(op);
                }
                ManagedType mt = ManagedType.GetManagedObject(op);

                if (mt is ClassBase)
                {
                    t = ((ClassBase)mt).type;
                }
                else if (mt is CLRObject)
                {
                    object inst = ((CLRObject)mt).inst;
                    if (inst is Type)
                    {
                        t = inst as Type;
                    }
                }
                else
                {
                    t = Converter.GetTypeByAlias(op);
                }

                if (t == null)
                {
                    types = null;
                    break;
                }
                types[i] = t;
            }
            if (free)
            {
                Runtime.XDecref(args);
            }
            return types;
        }

        //===================================================================
        // Managed exports of the Python C API. Where appropriate, we do
        // some optimization to avoid managed <--> unmanaged transitions
        // (mostly for heavily used methods).
        //===================================================================

        internal unsafe static void XIncref(IntPtr op)
        {
            if (IsPyDebug)
            {
                // according to Python doc, Py_IncRef() is Py_XINCREF() 
                Py_IncRef(op);
                return;
            }

            void* p = (void*)op;
            if ((void*)0 != p)
            {
                if (is32bit)
                {
                    (*(int*)p)++;
                }
                else
                {
                    (*(long*)p)++;
                }
            }
        }

        internal static unsafe void XDecref(IntPtr op)
        {
            if (IsPyDebug)
            {
                // Py_DecRef calls Python's Py_DECREF
                // according to Python doc, Py_DecRef() is Py_XDECREF()
                Py_DecRef(op);
                return;
            }

            void* p = (void*)op;
            if ((void*)0 != p)
            {
                if (is32bit)
                {
                    --(*(int*)p);
                }
                else
                {
                    --(*(long*)p);
                }
                if ((*(int*)p) == 0)
                {
                    // PyObject_HEAD: struct _typeobject *ob_type
                    void* t = is32bit
                        ? (void*)(*((uint*)p + 1))
                        : (void*)(*((ulong*)p + 1));
                    // PyTypeObject: destructor tp_dealloc
                    void* f = is32bit
                        ? (void*)(*((uint*)t + 6))
                        : (void*)(*((ulong*)t + 6));
                    if ((void*)0 == f)
                    {
                        return;
                    }
                    NativeCall.Impl.Void_Call_1(new IntPtr(f), op);
                    return;
                }
            }
        }

        internal unsafe static long Refcount(IntPtr op)
        {
            void* p = (void*)op;
            if ((void*)0 != p)
            {
                if (is32bit)
                {
                    return (*(int*)p);
                }
                else
                {
                    return (*(long*)p);
                }
            }
            return 0;
        }

        private static void Py_IncRef(IntPtr ob) => _interop.Py_IncRef(ob);

        private static void Py_DecRef(IntPtr ob) => _interop.Py_DecRef(ob);

        internal static void Py_Initialize() => _interop.Py_Initialize();

        internal static int Py_IsInitialized() => _interop.Py_IsInitialized();

        internal static void Py_Finalize() => _interop.Py_Finalize();

        internal static IntPtr Py_NewInterpreter() => _interop.Py_NewInterpreter();

        internal static void Py_EndInterpreter(IntPtr threadState) => _interop.Py_EndInterpreter(threadState);

        internal static IntPtr PyThreadState_New(IntPtr istate) => _interop.PyThreadState_New(istate);

        internal static IntPtr PyThreadState_Get() => _interop.PyThreadState_Get();

        internal static IntPtr PyThread_get_key_value(IntPtr key) => _interop.PyThread_get_key_value(key);

        internal static int PyThread_get_thread_ident() => _interop.PyThread_get_thread_ident();

        internal static int PyThread_set_key_value(IntPtr key, IntPtr value) => _interop.PyThread_set_key_value(key, value);

        internal static IntPtr PyThreadState_Swap(IntPtr key) => _interop.PyThreadState_Swap(key);


        internal static IntPtr PyGILState_Ensure() => _interop.PyGILState_Ensure();

        internal static void PyGILState_Release(IntPtr gs) => _interop.PyGILState_Release(gs);


        internal static IntPtr PyGILState_GetThisThreadState() => _interop.PyGILState_GetThisThreadState();

        public static int Py_Main(int argc, string[] argv) => _interop.Py_Main(argc, argv);


        internal static void PyEval_InitThreads() => _interop.PyEval_InitThreads();

        internal static int PyEval_ThreadsInitialized() => _interop.PyEval_ThreadsInitialized();

        internal static void PyEval_AcquireLock() => _interop.PyEval_AcquireLock();

        internal static void PyEval_ReleaseLock() => _interop.PyEval_ReleaseLock();

        internal static void PyEval_AcquireThread(IntPtr tstate) => _interop.PyEval_AcquireThread(tstate);

        internal static void PyEval_ReleaseThread(IntPtr tstate) => _interop.PyEval_ReleaseThread(tstate);

        internal static IntPtr PyEval_SaveThread() => _interop.PyEval_SaveThread();

        internal static void PyEval_RestoreThread(IntPtr tstate) => _interop.PyEval_RestoreThread(tstate);

        internal static IntPtr PyEval_GetBuiltins() => _interop.PyEval_GetBuiltins();

        internal static IntPtr PyEval_GetGlobals() => _interop.PyEval_GetGlobals();

        internal static IntPtr PyEval_GetLocals() => _interop.PyEval_GetLocals();

        internal static string Py_GetProgramName() => _interop.Py_GetProgramName();

        internal static void Py_SetProgramName(string name) => _interop.Py_SetProgramName(name);

        internal static string Py_GetPythonHome() => _interop.Py_GetPythonHome();

        internal static void Py_SetPythonHome(string home) => _interop.Py_SetPythonHome(home);

        internal static string Py_GetPath() => _interop.Py_GetPath();

        internal static void Py_SetPath(string home) => _interop.Py_SetPath(home);

        internal static string Py_GetVersion() => _interop.Py_GetVersion();

        internal static string Py_GetPlatform() => _interop.Py_GetPlatform();

        internal static string Py_GetCopyright() => _interop.Py_GetCopyright();

        internal static string Py_GetCompiler() => _interop.Py_GetCompiler();

        internal static string Py_GetBuildInfo() => _interop.Py_GetBuildInfo();

        internal static int PyRun_SimpleString(string code) => _interop.PyRun_SimpleString(code);

        internal static IntPtr PyRun_String(string code, IntPtr st, IntPtr globals, IntPtr locals)
            => _interop.PyRun_String(code, st, globals, locals);

        internal static IntPtr Py_CompileString(string code, string file, IntPtr tok) => _interop.Py_CompileString(code, file, tok);

        internal static IntPtr PyImport_ExecCodeModule(string name, IntPtr code)
            => _interop.PyImport_ExecCodeModule(name, code);

        internal static IntPtr PyCFunction_NewEx(IntPtr ml, IntPtr self, IntPtr mod)
            => _interop.PyCFunction_NewEx(ml, self, mod);

        internal static IntPtr PyCFunction_Call(IntPtr func, IntPtr args, IntPtr kw)
            => _interop.PyCFunction_Call(func, args, kw);

        internal static IntPtr PyClass_New(IntPtr bases, IntPtr dict, IntPtr name)
            => _interop.PyClass_New(bases, dict, name);

        internal static IntPtr PyInstance_New(IntPtr cls, IntPtr args, IntPtr kw)
            => _interop.PyInstance_New(cls, args, kw);

        internal static IntPtr PyInstance_NewRaw(IntPtr cls, IntPtr dict) => _interop.PyInstance_NewRaw(cls, dict);

        internal static IntPtr PyMethod_New(IntPtr func, IntPtr self, IntPtr cls)
            => _interop.PyMethod_New(func, self, cls);


        //====================================================================
        // Python abstract object API
        //====================================================================

        // A macro-like method to get the type of a Python object. This is
        // designed to be lean and mean in IL & avoid managed <-> unmanaged
        // transitions. Note that this does not incref the type object.
        internal unsafe static IntPtr PyObject_TYPE(IntPtr op)
        {
            void* p = (void*)op;
            if ((void*)0 == p)
            {
                return IntPtr.Zero;
            }

            int n = IsPyDebug ? 3 : 1;

            if (is32bit)
            {
                return new IntPtr((void*)(*((uint*)p + n)));
            }
            else
            {
                return new IntPtr((void*)(*((ulong*)p + n)));
            }
        }

        // Managed version of the standard Python C API PyObject_Type call.
        // This version avoids a managed <-> unmanaged transition. This one
        // does incref the returned type object.

        internal unsafe static IntPtr
            PyObject_Type(IntPtr op)
        {
            IntPtr tp = PyObject_TYPE(op);
            Runtime.XIncref(tp);
            return tp;
        }

        internal static string PyObject_GetTypeName(IntPtr op)
        {
            IntPtr pyType = Marshal.ReadIntPtr(op, ObjectOffset.ob_type);
            IntPtr ppName = Marshal.ReadIntPtr(pyType, TypeOffset.tp_name);
            return Marshal.PtrToStringAnsi(ppName);
        }

        internal static int PyObject_HasAttrString(IntPtr pointer, string name)
            => _interop.PyObject_HasAttrString(pointer, name);

        internal static IntPtr PyObject_GetAttrString(IntPtr pointer, string name)
            => _interop.PyObject_GetAttrString(pointer, name);

        internal static int PyObject_SetAttrString(IntPtr pointer, string name, IntPtr value)
            => _interop.PyObject_SetAttrString(pointer, name, value);

        internal static int PyObject_HasAttr(IntPtr pointer, IntPtr name) => _interop.PyObject_HasAttr(pointer, name);

        internal static IntPtr PyObject_GetAttr(IntPtr pointer, IntPtr name) => _interop.PyObject_GetAttr(pointer, name);

        internal static int PyObject_SetAttr(IntPtr pointer, IntPtr name, IntPtr value)
            => _interop.PyObject_SetAttr(pointer, name, value);

        internal static IntPtr PyObject_GetItem(IntPtr pointer, IntPtr key) => _interop.PyObject_GetItem(pointer, key);

        internal static int PyObject_SetItem(IntPtr pointer, IntPtr key, IntPtr value)
            => _interop.PyObject_SetItem(pointer, key, value);

        internal static int PyObject_DelItem(IntPtr pointer, IntPtr key) => _interop.PyObject_DelItem(pointer, key);

        internal static IntPtr PyObject_GetIter(IntPtr op) => _interop.PyObject_GetIter(op);

        internal static IntPtr PyObject_Call(IntPtr pointer, IntPtr args, IntPtr kw)
            => _interop.PyObject_Call(pointer, args, kw);

        internal static IntPtr PyObject_CallObject(IntPtr pointer, IntPtr args)
            => _interop.PyObject_CallObject(pointer, args);

        internal static int PyObject_RichCompareBool(IntPtr value1, IntPtr value2, int opid)
            => _interop.PyObject_RichCompareBool(value1, value2, opid);

        internal static int PyObject_Compare(IntPtr value1, IntPtr value2) => _interop.PyObject_Compare(value1, value2);


        internal static int PyObject_IsInstance(IntPtr ob, IntPtr type) => _interop.PyObject_IsInstance(ob, type);

        internal static int PyObject_IsSubclass(IntPtr ob, IntPtr type) => _interop.PyObject_IsSubclass(ob, type);

        internal static int PyCallable_Check(IntPtr pointer) => _interop.PyCallable_Check(pointer);

        internal static int PyObject_IsTrue(IntPtr pointer) => _interop.PyObject_IsTrue(pointer);

        internal static int PyObject_Not(IntPtr pointer) => _interop.PyObject_Not(pointer);

        internal static int PyObject_Size(IntPtr pointer) => _interop.PyObject_Size(pointer);

        internal static IntPtr PyObject_Hash(IntPtr op) => _interop.PyObject_Hash(op);

        internal static IntPtr PyObject_Repr(IntPtr pointer) => _interop.PyObject_Repr(pointer);

        internal static IntPtr PyObject_Str(IntPtr pointer) => _interop.PyObject_Str(pointer);

        internal static IntPtr PyObject_Unicode(IntPtr pointer) => _interop.PyObject_Unicode(pointer);

        internal static IntPtr PyObject_Dir(IntPtr pointer) => _interop.PyObject_Dir(pointer);


        //====================================================================
        // Python number API
        //====================================================================

        internal static IntPtr PyNumber_Int(IntPtr ob) => _interop.PyNumber_Int(ob);

        internal static IntPtr PyNumber_Long(IntPtr ob) => _interop.PyNumber_Long(ob);

        internal static IntPtr PyNumber_Float(IntPtr ob) => _interop.PyNumber_Float(ob);

        internal static bool PyNumber_Check(IntPtr ob) => _interop.PyNumber_Check(ob);


        internal static bool PyInt_Check(IntPtr ob)
        {
            return PyObject_TypeCheck(ob, Runtime.PyIntType);
        }

        internal static bool PyBool_Check(IntPtr ob)
        {
            return PyObject_TypeCheck(ob, Runtime.PyBoolType);
        }

        internal static IntPtr PyInt_FromInt32(int value)
        {
            IntPtr v = new IntPtr(value);
            return PyInt_FromLong(v);
        }

        internal static IntPtr PyInt_FromInt64(long value)
        {
            IntPtr v = new IntPtr(value);
            return PyInt_FromLong(v);
        }

        private static IntPtr PyInt_FromLong(IntPtr value) => _interop.PyInt_FromLong(value);

        internal static int PyInt_AsLong(IntPtr value) => _interop.PyInt_AsLong(value);

        internal static IntPtr PyInt_FromString(string value, IntPtr end, int radix)
            => _interop.PyInt_FromString(value, end, radix);

        internal static int PyInt_GetMax() => _interop.PyInt_GetMax();
                        
        internal static bool PyLong_Check(IntPtr ob)
        {
            return PyObject_TYPE(ob) == Runtime.PyLongType;
        }

        internal static IntPtr PyLong_FromLong(long value) => _interop.PyLong_FromLong(value);

        internal static IntPtr PyLong_FromUnsignedLong(uint value) => _interop.PyLong_FromUnsignedLong(value);

        internal static IntPtr PyLong_FromDouble(double value) => _interop.PyLong_FromDouble(value);

        internal static IntPtr PyLong_FromLongLong(long value) => _interop.PyLong_FromLongLong(value);

        internal static IntPtr PyLong_FromUnsignedLongLong(ulong value) => _interop.PyLong_FromUnsignedLongLong(value);

        internal static IntPtr PyLong_FromString(string value, IntPtr end, int radix)
            => _interop.PyLong_FromString(value, end, radix);

        internal static int PyLong_AsLong(IntPtr value) => _interop.PyLong_AsLong(value);

        internal static uint PyLong_AsUnsignedLong(IntPtr value) => _interop.PyLong_AsUnsignedLong(value);

        internal static long PyLong_AsLongLong(IntPtr value) => _interop.PyLong_AsLongLong(value);

        internal static ulong PyLong_AsUnsignedLongLong(IntPtr value) => _interop.PyLong_AsUnsignedLongLong(value);


        internal static bool PyFloat_Check(IntPtr ob) 
        {
            return PyObject_TYPE(ob) == Runtime.PyFloatType;
        }

        internal static IntPtr PyFloat_FromDouble(double value) => _interop.PyFloat_FromDouble(value);

        internal static IntPtr PyFloat_FromString(IntPtr value, IntPtr junk) => _interop.PyFloat_FromString(value, junk);

        internal static double PyFloat_AsDouble(IntPtr ob) => _interop.PyFloat_AsDouble(ob);

        internal static IntPtr PyNumber_Add(IntPtr o1, IntPtr o2) => _interop.PyNumber_Add(o1, o2);

        internal static IntPtr PyNumber_Subtract(IntPtr o1, IntPtr o2) => _interop.PyNumber_Subtract(o1, o2);

        internal static IntPtr PyNumber_Multiply(IntPtr o1, IntPtr o2) => _interop.PyNumber_Multiply(o1, o2);

        internal static IntPtr PyNumber_Divide(IntPtr o1, IntPtr o2) => _interop.PyNumber_Divide(o1, o2);

        internal static IntPtr PyNumber_And(IntPtr o1, IntPtr o2) => _interop.PyNumber_And(o1, o2);

        internal static IntPtr PyNumber_Xor(IntPtr o1, IntPtr o2) => _interop.PyNumber_Xor(o1, o2);

        internal static IntPtr PyNumber_Or(IntPtr o1, IntPtr o2) => _interop.PyNumber_Or(o1, o2);

        internal static IntPtr PyNumber_Lshift(IntPtr o1, IntPtr o2) => _interop.PyNumber_Lshift(o1, o2);

        internal static IntPtr PyNumber_Rshift(IntPtr o1, IntPtr o2) => _interop.PyNumber_Rshift(o1, o2);

        internal static IntPtr PyNumber_Power(IntPtr o1, IntPtr o2) => _interop.PyNumber_Power(o1, o2);

        internal static IntPtr PyNumber_Remainder(IntPtr o1, IntPtr o2) => _interop.PyNumber_Remainder(o1, o2);

        internal static IntPtr PyNumber_InPlaceAdd(IntPtr o1, IntPtr o2) => _interop.PyNumber_InPlaceAdd(o1, o2);

        internal static IntPtr PyNumber_InPlaceSubtract(IntPtr o1, IntPtr o2)
            => _interop.PyNumber_InPlaceSubtract(o1, o2);

        internal static IntPtr PyNumber_InPlaceMultiply(IntPtr o1, IntPtr o2)
            => _interop.PyNumber_InPlaceMultiply(o1, o2);

        internal static IntPtr PyNumber_InPlaceDivide(IntPtr o1, IntPtr o2) => _interop.PyNumber_InPlaceDivide(o1, o2);

        internal static IntPtr PyNumber_InPlaceAnd(IntPtr o1, IntPtr o2) => _interop.PyNumber_InPlaceAnd(o1, o2);

        internal static IntPtr PyNumber_InPlaceXor(IntPtr o1, IntPtr o2) => _interop.PyNumber_InPlaceXor(o1, o2);

        internal static IntPtr PyNumber_InPlaceOr(IntPtr o1, IntPtr o2) => _interop.PyNumber_InPlaceOr(o1, o2);

        internal static IntPtr PyNumber_InPlaceLshift(IntPtr o1, IntPtr o2) => _interop.PyNumber_InPlaceLshift(o1, o2);

        internal static IntPtr PyNumber_InPlaceRshift(IntPtr o1, IntPtr o2) => _interop.PyNumber_InPlaceRshift(o1, o2);

        internal static IntPtr PyNumber_InPlacePower(IntPtr o1, IntPtr o2) => _interop.PyNumber_InPlacePower(o1, o2);

        internal static IntPtr PyNumber_InPlaceRemainder(IntPtr o1, IntPtr o2)
            => _interop.PyNumber_InPlaceRemainder(o1, o2);

        internal static IntPtr PyNumber_Negative(IntPtr o1) => _interop.PyNumber_Negative(o1);

        internal static IntPtr PyNumber_Positive(IntPtr o1) => _interop.PyNumber_Positive(o1);

        internal static IntPtr PyNumber_Invert(IntPtr o1) => _interop.PyNumber_Invert(o1);

        //====================================================================
        // Python sequence API
        //====================================================================

        internal static bool PySequence_Check(IntPtr pointer) => _interop.PySequence_Check(pointer);

        internal static IntPtr PySequence_GetItem(IntPtr pointer, int index) => _interop.PySequence_GetItem(pointer, index);

        internal static int PySequence_SetItem(IntPtr pointer, int index, IntPtr value)
            => _interop.PySequence_SetItem(pointer, index, value);

        internal static int PySequence_DelItem(IntPtr pointer, int index) => _interop.PySequence_DelItem(pointer, index);

        internal static IntPtr PySequence_GetSlice(IntPtr pointer, int i1, int i2)
            => _interop.PySequence_GetSlice(pointer, i1, i2);

        internal static int PySequence_SetSlice(IntPtr pointer, int i1, int i2, IntPtr v)
            => _interop.PySequence_SetSlice(pointer, i1, i2, v);

        internal static int PySequence_DelSlice(IntPtr pointer, int i1, int i2)
            => _interop.PySequence_DelSlice(pointer, i1, i2);

        internal static int PySequence_Size(IntPtr pointer) => _interop.PySequence_Size(pointer);

        internal static int PySequence_Contains(IntPtr pointer, IntPtr item)
            => _interop.PySequence_Contains(pointer, item);

        internal static IntPtr PySequence_Concat(IntPtr pointer, IntPtr other)
            => _interop.PySequence_Concat(pointer, other);

        internal static IntPtr PySequence_Repeat(IntPtr pointer, int count)
            => _interop.PySequence_Repeat(pointer, count);

        internal static int PySequence_Index(IntPtr pointer, IntPtr item) => _interop.PySequence_Index(pointer, item);

        internal static int PySequence_Count(IntPtr pointer, IntPtr value) => _interop.PySequence_Count(pointer, value);

        internal static IntPtr PySequence_Tuple(IntPtr pointer) => _interop.PySequence_Tuple(pointer);

        internal static IntPtr PySequence_List(IntPtr pointer) => _interop.PySequence_List(pointer);


        //====================================================================
        // Python string API
        //====================================================================

        internal static bool IsStringType(IntPtr op)
        {
            IntPtr t = PyObject_TYPE(op);
            return (t == PyStringType) || (t == PyUnicodeType);
        }

        internal static bool PyString_Check(IntPtr ob)
        {
            return PyObject_TYPE(ob) == Runtime.PyStringType;
        }

        internal static IntPtr PyString_FromString(string value)
        {
            return PyString_FromStringAndSize(value, value.Length);
        }

        internal static IntPtr PyBytes_FromString(string op) => _interop.PyBytes_FromString(op);

        internal static int PyBytes_Size(IntPtr op) => _interop.PyBytes_Size(op);

        internal static IntPtr PyBytes_AS_STRING(IntPtr ob) => _interop.PyBytes_AS_STRING(ob);

        internal static IntPtr PyUnicode_FromStringAndSize(IntPtr value, int size)
            => _interop.PyUnicode_FromStringAndSize(value, size);


        internal static IntPtr PyString_FromStringAndSize(string value, int size)
            => _interop.PyString_FromStringAndSize(value, size);

        internal static IntPtr PyString_AS_STRING(IntPtr op) => _interop.PyString_AS_STRING(op);

        internal static int PyString_Size(IntPtr pointer) => _interop.PyString_Size(pointer);
                        
        internal static bool PyUnicode_Check(IntPtr ob)
        {
            return PyObject_TYPE(ob) == Runtime.PyUnicodeType;
        }

        internal static IntPtr PyUnicode_FromObject(IntPtr ob) => _interop.PyUnicode_FromObject(ob);

        internal static IntPtr PyUnicode_FromEncodedObject(IntPtr ob, IntPtr enc, IntPtr err)
            => _interop.PyUnicode_FromEncodedObject(ob, enc, err);

        internal static IntPtr PyUnicode_FromKindAndString(int kind, string s, int size)
            => _interop.PyUnicode_FromKindAndString(kind, s, size);

        internal static IntPtr PyUnicode_FromUnicode(string s, int size) => _interop.PyUnicode_FromUnicode(s, size);

        internal static int PyUnicode_GetSize(IntPtr ob) => _interop.PyUnicode_GetSize(ob);

        internal static IntPtr PyUnicode_AS_UNICODE(IntPtr op) => _interop.PyUnicode_AS_UNICODE(op);

        internal static IntPtr PyUnicode_FromOrdinal(int c) => _interop.PyUnicode_FromOrdinal(c);

        internal static IntPtr PyUnicode_FromString(string s) => _interop.PyUnicode_FromString(s);

        internal static string GetManagedString(IntPtr op) => _interop.GetManagedString(op);

        internal static IntPtr PyUnicode_FromKindAndString(int kind, IntPtr s, int size)
            => _interop.PyUnicode_FromKindAndString(kind, s, size);


        //====================================================================
        // Python dictionary API
        //====================================================================

        internal static bool PyDict_Check(IntPtr ob)
        {
            return PyObject_TYPE(ob) == Runtime.PyDictType;
        }

        internal static IntPtr PyDict_New() => _interop.PyDict_New();

        internal static IntPtr PyDictProxy_New(IntPtr dict) => _interop.PyDictProxy_New(dict);

        internal static IntPtr PyDict_GetItem(IntPtr pointer, IntPtr key) => _interop.PyDict_GetItem(pointer, key);

        internal static IntPtr PyDict_GetItemString(IntPtr pointer, string key)
            => _interop.PyDict_GetItemString(pointer, key);

        internal static int PyDict_SetItem(IntPtr pointer, IntPtr key, IntPtr value)
            => _interop.PyDict_SetItem(pointer, key, value);

        internal static int PyDict_SetItemString(IntPtr pointer, string key, IntPtr value)
            => _interop.PyDict_SetItemString(pointer, key, value);

        internal static int PyDict_DelItem(IntPtr pointer, IntPtr key) => _interop.PyDict_DelItem(pointer, key);

        internal static int PyDict_DelItemString(IntPtr pointer, string key)
            => _interop.PyDict_DelItemString(pointer, key);

        internal static int PyMapping_HasKey(IntPtr pointer, IntPtr key) => _interop.PyMapping_HasKey(pointer, key);

        internal static IntPtr PyDict_Keys(IntPtr pointer) => _interop.PyDict_Keys(pointer);

        internal static IntPtr PyDict_Values(IntPtr pointer) => _interop.PyDict_Values(pointer);

        internal static IntPtr PyDict_Items(IntPtr pointer) => _interop.PyDict_Items(pointer);

        internal static IntPtr PyDict_Copy(IntPtr pointer) => _interop.PyDict_Copy(pointer);

        internal static int PyDict_Update(IntPtr pointer, IntPtr other) => _interop.PyDict_Update(pointer, other);

        internal static void PyDict_Clear(IntPtr pointer) => _interop.PyDict_Clear(pointer);

        internal static int PyDict_Size(IntPtr pointer) => _interop.PyDict_Size(pointer);


        //====================================================================
        // Python list API
        //====================================================================

        internal static bool PyList_Check(IntPtr ob)
        {
            return PyObject_TYPE(ob) == Runtime.PyListType;
        }

        internal static IntPtr PyList_New(int size) => _interop.PyList_New(size);

        internal static IntPtr PyList_AsTuple(IntPtr pointer) => _interop.PyList_AsTuple(pointer);

        internal static IntPtr PyList_GetItem(IntPtr pointer, int index) => _interop.PyList_GetItem(pointer, index);

        internal static int PyList_SetItem(IntPtr pointer, int index, IntPtr value)
            => _interop.PyList_SetItem(pointer, index, value);

        internal static int PyList_Insert(IntPtr pointer, int index, IntPtr value)
            => _interop.PyList_Insert(pointer, index, value);

        internal static int PyList_Append(IntPtr pointer, IntPtr value) => _interop.PyList_Append(pointer, value);

        internal static int PyList_Reverse(IntPtr pointer) => _interop.PyList_Reverse(pointer);

        internal static int PyList_Sort(IntPtr pointer) => _interop.PyList_Sort(pointer);

        internal static IntPtr PyList_GetSlice(IntPtr pointer, int start, int end)
            => _interop.PyList_GetSlice(pointer, start, end);

        internal static int PyList_SetSlice(IntPtr pointer, int start, int end, IntPtr value)
            => _interop.PyList_SetSlice(pointer, start, end, value);

        internal static int PyList_Size(IntPtr pointer) => _interop.PyList_Size(pointer);


        //====================================================================
        // Python tuple API
        //====================================================================

        internal static bool PyTuple_Check(IntPtr ob)
        {
            return PyObject_TYPE(ob) == Runtime.PyTupleType;
        }

        internal static IntPtr PyTuple_New(int size) => _interop.PyTuple_New(size);

        internal static IntPtr PyTuple_GetItem(IntPtr pointer, int index) => _interop.PyTuple_GetItem(pointer, index);

        internal static int PyTuple_SetItem(IntPtr pointer, int index, IntPtr value)
            => _interop.PyTuple_SetItem(pointer, index, value);

        internal static IntPtr PyTuple_GetSlice(IntPtr pointer, int start, int end)
            => _interop.PyTuple_GetSlice(pointer, start, end);

        internal static int PyTuple_Size(IntPtr pointer) => _interop.PyTuple_Size(pointer);


        //====================================================================
        // Python iterator API
        //====================================================================

        internal static bool PyIter_Check(IntPtr pointer) => _interop.PyIter_Check(pointer);

        internal static IntPtr PyIter_Next(IntPtr pointer) => _interop.PyIter_Next(pointer);

        //====================================================================
        // Python module API
        //====================================================================

        internal static IntPtr PyModule_New(string name) => _interop.PyModule_New(name);

        internal static string PyModule_GetName(IntPtr module) => _interop.PyModule_GetName(module);

        internal static IntPtr PyModule_GetDict(IntPtr module) => _interop.PyModule_GetDict(module);

        internal static string PyModule_GetFilename(IntPtr module) => _interop.PyModule_GetFilename(module);

        internal static IntPtr PyModule_Create2(IntPtr module, int apiver) => _interop.PyModule_Create2(module, apiver);

        internal static IntPtr PyImport_Import(IntPtr name) => _interop.PyImport_Import(name);

        internal static IntPtr PyImport_ImportModule(string name) => _interop.PyImport_ImportModule(name);

        internal static IntPtr PyImport_ReloadModule(IntPtr module) => _interop.PyImport_ReloadModule(module);

        internal static IntPtr PyImport_AddModule(string name) => _interop.PyImport_AddModule(name);

        internal static IntPtr PyImport_GetModuleDict() => _interop.PyImport_GetModuleDict();


        internal static void PySys_SetArgv(int argc, IntPtr argv) => _interop.PySys_SetArgv(argc, argv);

        internal static IntPtr PySys_GetObject(string name) => _interop.PySys_GetObject(name);

        internal static int PySys_SetObject(string name, IntPtr ob) => _interop.PySys_SetObject(name, ob);


        //====================================================================
        // Python type object API
        //====================================================================

        internal static bool PyType_Check(IntPtr ob)
        {
            return PyObject_TypeCheck(ob, Runtime.PyTypeType);
        }

        internal static void PyType_Modified(IntPtr type) => _interop.PyType_Modified(type);

        internal static bool PyType_IsSubtype(IntPtr t1, IntPtr t2) => _interop.PyType_IsSubtype(t1, t2);

        internal static bool PyObject_TypeCheck(IntPtr ob, IntPtr tp)
        {
            IntPtr t = PyObject_TYPE(ob);
            return (t == tp) || PyType_IsSubtype(t, tp);
        }

        internal static IntPtr PyType_GenericNew(IntPtr type, IntPtr args, IntPtr kw)
            => _interop.PyType_GenericNew(type, args, kw);

        internal static IntPtr PyType_GenericAlloc(IntPtr type, int n) => _interop.PyType_GenericAlloc(type, n);

        internal static int PyType_Ready(IntPtr type) => _interop.PyType_Ready(type);

        internal static IntPtr _PyType_Lookup(IntPtr type, IntPtr name) => _interop._PyType_Lookup(type, name);

        internal static IntPtr PyObject_GenericGetAttr(IntPtr obj, IntPtr name)
            => _interop.PyObject_GenericGetAttr(obj, name);

        internal static int PyObject_GenericSetAttr(IntPtr obj, IntPtr name, IntPtr value)
            => _interop.PyObject_GenericSetAttr(obj, name, value);

        internal static IntPtr _PyObject_GetDictPtr(IntPtr obj) => _interop._PyObject_GetDictPtr(obj);

        internal static IntPtr PyObject_GC_New(IntPtr tp) => _interop.PyObject_GC_New(tp);

        internal static void PyObject_GC_Del(IntPtr tp) => _interop.PyObject_GC_Del(tp);

        internal static void PyObject_GC_Track(IntPtr tp) => _interop.PyObject_GC_Track(tp);

        internal static void PyObject_GC_UnTrack(IntPtr tp) => _interop.PyObject_GC_UnTrack(tp);


        //====================================================================
        // Python memory API
        //====================================================================

        internal static IntPtr PyMem_Malloc(int size) => _interop.PyMem_Malloc(size);

        internal static IntPtr PyMem_Realloc(IntPtr ptr, int size) => _interop.PyMem_Realloc(ptr, size);

        internal static void PyMem_Free(IntPtr ptr) => _interop.PyMem_Free(ptr);


        //====================================================================
        // Python exception API
        //====================================================================

        internal static void PyErr_SetString(IntPtr ob, string message) => _interop.PyErr_SetString(ob, message);

        internal static void PyErr_SetObject(IntPtr ob, IntPtr message) => _interop.PyErr_SetObject(ob, message);

        internal static IntPtr PyErr_SetFromErrno(IntPtr ob) => _interop.PyErr_SetFromErrno(ob);

        internal static void PyErr_SetNone(IntPtr ob) => _interop.PyErr_SetNone(ob);

        internal static int PyErr_ExceptionMatches(IntPtr exception) => _interop.PyErr_ExceptionMatches(exception);

        internal static int PyErr_GivenExceptionMatches(IntPtr ob, IntPtr val)
            => _interop.PyErr_GivenExceptionMatches(ob, val);

        internal static void PyErr_NormalizeException(IntPtr ob, IntPtr val, IntPtr tb) => _interop.PyErr_NormalizeException(ob, val, tb);

        internal static int PyErr_Occurred() => _interop.PyErr_Occurred();

        internal static void PyErr_Fetch(ref IntPtr ob, ref IntPtr val, ref IntPtr tb)
            => _interop.PyErr_Fetch(ref ob, ref val, ref tb);

        internal static void PyErr_Restore(IntPtr ob, IntPtr val, IntPtr tb) => _interop.PyErr_Restore(ob, val, tb);

        internal static void PyErr_Clear() => _interop.PyErr_Clear();

        internal static void PyErr_Print() => _interop.PyErr_Print();


        //====================================================================
        // Miscellaneous
        //====================================================================

        internal static IntPtr PyMethod_Self(IntPtr ob) => _interop.PyMethod_Self(ob);

        internal static IntPtr PyMethod_Function(IntPtr ob) => _interop.PyMethod_Function(ob);
    }
}