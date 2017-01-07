namespace Python.Runtime.InteropContracts
{
    using System;
    using System.Runtime.InteropServices;

    public interface IPythonRuntimeInterop
    {
        /// <summary>
        /// UCS mode (2 or 4).
        /// </summary>
        int UCS { get; }

        string PyVersion { get; }

        int PyVersionNumber { get; }

        // Py_IncRef and Py_DecRef are taking care of the extra payload
        // in Py_DEBUG builds of Python like _Py_RefTotal
        void Py_IncRef(IntPtr ob);

        void Py_DecRef(IntPtr ob);

        void Py_Initialize();

        int Py_IsInitialized();

        void Py_Finalize();

        IntPtr Py_NewInterpreter();

        void Py_EndInterpreter(IntPtr threadState);

        IntPtr PyThreadState_New(IntPtr istate);

        IntPtr PyThreadState_Get();

        IntPtr PyThread_get_key_value(IntPtr key);

        int PyThread_get_thread_ident();

        int PyThread_set_key_value(IntPtr key, IntPtr value);

        IntPtr PyThreadState_Swap(IntPtr key);


        IntPtr PyGILState_Ensure();

        void PyGILState_Release(IntPtr gs);


        IntPtr PyGILState_GetThisThreadState();

        int Py_Main(int argc, string[] argv);

        void PyEval_InitThreads();

        int PyEval_ThreadsInitialized();

        void PyEval_AcquireLock();

        void PyEval_ReleaseLock();

        void PyEval_AcquireThread(IntPtr tstate);

        void PyEval_ReleaseThread(IntPtr tstate);

        IntPtr PyEval_SaveThread();

        void PyEval_RestoreThread(IntPtr tstate);

        IntPtr PyEval_GetBuiltins();

        IntPtr PyEval_GetGlobals();

        IntPtr PyEval_GetLocals();

        string Py_GetProgramName();

        void Py_SetProgramName(string name);

        string Py_GetPythonHome();

        void Py_SetPythonHome(string home);

        string Py_GetPath();

        void Py_SetPath(string home);

        string Py_GetVersion();

        string Py_GetPlatform();

        string Py_GetCopyright();

        string Py_GetCompiler();

        string Py_GetBuildInfo();

        int PyRun_SimpleString(string code);

        IntPtr PyRun_String(string code, IntPtr st, IntPtr globals, IntPtr locals);

        IntPtr Py_CompileString(string code, string file, IntPtr tok);

        IntPtr PyImport_ExecCodeModule(string name, IntPtr code);

        IntPtr PyCFunction_NewEx(IntPtr ml, IntPtr self, IntPtr mod);

        IntPtr PyCFunction_Call(IntPtr func, IntPtr args, IntPtr kw);

        IntPtr PyClass_New(IntPtr bases, IntPtr dict, IntPtr name);

        IntPtr PyInstance_New(IntPtr cls, IntPtr args, IntPtr kw);

        IntPtr PyInstance_NewRaw(IntPtr cls, IntPtr dict);

        IntPtr PyMethod_New(IntPtr func, IntPtr self, IntPtr cls);

        int PyObject_HasAttrString(IntPtr pointer, string name);

        IntPtr PyObject_GetAttrString(IntPtr pointer, string name);

        int PyObject_SetAttrString(IntPtr pointer, string name, IntPtr value);

        int PyObject_HasAttr(IntPtr pointer, IntPtr name);

        IntPtr PyObject_GetAttr(IntPtr pointer, IntPtr name);

        int PyObject_SetAttr(IntPtr pointer, IntPtr name, IntPtr value);

        IntPtr PyObject_GetItem(IntPtr pointer, IntPtr key);

        int PyObject_SetItem(IntPtr pointer, IntPtr key, IntPtr value);

        int PyObject_DelItem(IntPtr pointer, IntPtr key);

        IntPtr PyObject_GetIter(IntPtr op);

        IntPtr PyObject_Call(IntPtr pointer, IntPtr args, IntPtr kw);

        IntPtr PyObject_CallObject(IntPtr pointer, IntPtr args);

        int PyObject_Compare(IntPtr value1, IntPtr value2);

        int PyObject_RichCompareBool(IntPtr value1, IntPtr value2, int opid);

        int PyObject_IsInstance(IntPtr ob, IntPtr type);

        int PyObject_IsSubclass(IntPtr ob, IntPtr type);

        int PyCallable_Check(IntPtr pointer);

        int PyObject_IsTrue(IntPtr pointer);

        int PyObject_Not(IntPtr pointer);

        int PyObject_Size(IntPtr pointer);

        IntPtr PyObject_Hash(IntPtr op);

        IntPtr PyObject_Repr(IntPtr pointer);

        IntPtr PyObject_Str(IntPtr pointer);

        IntPtr PyObject_Unicode(IntPtr pointer);

        IntPtr PyObject_Dir(IntPtr pointer);

        IntPtr PyNumber_Int(IntPtr ob);

        IntPtr PyNumber_Long(IntPtr ob);

        IntPtr PyNumber_Float(IntPtr ob);

        bool PyNumber_Check(IntPtr ob);

        IntPtr PyInt_FromLong(IntPtr value);

        int PyInt_AsLong(IntPtr value);

        IntPtr PyInt_FromString(string value, IntPtr end, int radix);

        int PyInt_GetMax();

        IntPtr PyLong_FromLong(long value);

        IntPtr PyLong_FromUnsignedLong(uint value);

        IntPtr PyLong_FromDouble(double value);

        IntPtr PyLong_FromLongLong(long value);

        IntPtr PyLong_FromUnsignedLongLong(ulong value);

        IntPtr PyLong_FromString(string value, IntPtr end, int radix);

        int PyLong_AsLong(IntPtr value);

        uint PyLong_AsUnsignedLong(IntPtr value);

        long PyLong_AsLongLong(IntPtr value);

        ulong PyLong_AsUnsignedLongLong(IntPtr value);

        IntPtr PyFloat_FromDouble(double value);

        IntPtr PyFloat_FromString(IntPtr value, IntPtr junk);

        double PyFloat_AsDouble(IntPtr ob);

        IntPtr PyNumber_Add(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_Subtract(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_Multiply(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_Divide(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_And(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_Xor(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_Or(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_Lshift(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_Rshift(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_Power(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_Remainder(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_InPlaceAdd(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_InPlaceSubtract(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_InPlaceMultiply(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_InPlaceDivide(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_InPlaceAnd(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_InPlaceXor(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_InPlaceOr(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_InPlaceLshift(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_InPlaceRshift(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_InPlacePower(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_InPlaceRemainder(IntPtr o1, IntPtr o2);

        IntPtr PyNumber_Negative(IntPtr o1);

        IntPtr PyNumber_Positive(IntPtr o1);

        IntPtr PyNumber_Invert(IntPtr o1);

        //====================================================================
        // Python sequence API
        //====================================================================

        bool PySequence_Check(IntPtr pointer);

        IntPtr PySequence_GetItem(IntPtr pointer, int index);

        int PySequence_SetItem(IntPtr pointer, int index, IntPtr value);

        int PySequence_DelItem(IntPtr pointer, int index);

        IntPtr PySequence_GetSlice(IntPtr pointer, int i1, int i2);

        int PySequence_SetSlice(IntPtr pointer, int i1, int i2, IntPtr v);

        int PySequence_DelSlice(IntPtr pointer, int i1, int i2);

        int PySequence_Size(IntPtr pointer);

        int PySequence_Contains(IntPtr pointer, IntPtr item);

        IntPtr PySequence_Concat(IntPtr pointer, IntPtr other);

        IntPtr PySequence_Repeat(IntPtr pointer, int count);

        int PySequence_Index(IntPtr pointer, IntPtr item);

        int PySequence_Count(IntPtr pointer, IntPtr value);

        IntPtr PySequence_Tuple(IntPtr pointer);

        IntPtr PySequence_List(IntPtr pointer);

        IntPtr PyBytes_FromString(string op);

        int PyBytes_Size(IntPtr op);

        IntPtr PyBytes_AS_STRING(IntPtr ob);

        IntPtr PyUnicode_FromStringAndSize(IntPtr value, int size);


        IntPtr PyString_FromStringAndSize(string value, int size);

        IntPtr PyString_AS_STRING(IntPtr op);

        int PyString_Size(IntPtr pointer);

        IntPtr PyUnicode_FromEncodedObject(IntPtr ob, IntPtr enc, IntPtr err);

        IntPtr PyUnicode_FromKindAndString(int kind, string s, int size);

        IntPtr PyUnicode_FromUnicode(string s, int size);

        int PyUnicode_GetSize(IntPtr ob);
                        
        IntPtr PyUnicode_FromObject(IntPtr ob);

        unsafe char* PyUnicode_AsUnicode(IntPtr ob);

        IntPtr PyUnicode_AS_UNICODE(IntPtr op);

        IntPtr PyUnicode_FromOrdinal(int c);

        IntPtr PyUnicode_FromString(string s);

        string GetManagedString(IntPtr op);

        IntPtr PyUnicode_FromKindAndString(int kind, IntPtr s, int size);

        //====================================================================
        // Python dictionary API
        //====================================================================

        IntPtr PyDict_New();

        IntPtr PyDictProxy_New(IntPtr dict);

        IntPtr PyDict_GetItem(IntPtr pointer, IntPtr key);

        IntPtr PyDict_GetItemString(IntPtr pointer, string key);

        int PyDict_SetItem(IntPtr pointer, IntPtr key, IntPtr value);

        int PyDict_SetItemString(IntPtr pointer, string key, IntPtr value);

        int PyDict_DelItem(IntPtr pointer, IntPtr key);

        int PyDict_DelItemString(IntPtr pointer, string key);

        int PyMapping_HasKey(IntPtr pointer, IntPtr key);

        IntPtr PyDict_Keys(IntPtr pointer);

        IntPtr PyDict_Values(IntPtr pointer);

        IntPtr PyDict_Items(IntPtr pointer);

        IntPtr PyDict_Copy(IntPtr pointer);

        int PyDict_Update(IntPtr pointer, IntPtr other);

        void PyDict_Clear(IntPtr pointer);

        int PyDict_Size(IntPtr pointer);


        //====================================================================
        // Python list API
        //====================================================================

        IntPtr PyList_New(int size);

        IntPtr PyList_AsTuple(IntPtr pointer);

        IntPtr PyList_GetItem(IntPtr pointer, int index);

        int PyList_SetItem(IntPtr pointer, int index, IntPtr value);

        int PyList_Insert(IntPtr pointer, int index, IntPtr value);

        int PyList_Append(IntPtr pointer, IntPtr value);

        int PyList_Reverse(IntPtr pointer);

        int PyList_Sort(IntPtr pointer);

        IntPtr PyList_GetSlice(IntPtr pointer, int start, int end);

        int PyList_SetSlice(IntPtr pointer, int start, int end, IntPtr value);

        int PyList_Size(IntPtr pointer);


        //====================================================================
        // Python tuple API
        //====================================================================

        IntPtr PyTuple_New(int size);

        IntPtr PyTuple_GetItem(IntPtr pointer, int index);

        int PyTuple_SetItem(IntPtr pointer, int index, IntPtr value);

        IntPtr PyTuple_GetSlice(IntPtr pointer, int start, int end);

        int PyTuple_Size(IntPtr pointer);

        //====================================================================
        // Python iterator API
        //====================================================================

        bool PyIter_Check(IntPtr pointer);

        IntPtr PyIter_Next(IntPtr pointer);

        //====================================================================
        // Python module API
        //====================================================================

        IntPtr PyModule_New(string name);

        string PyModule_GetName(IntPtr module);

        IntPtr PyModule_GetDict(IntPtr module);

        string PyModule_GetFilename(IntPtr module);

        IntPtr PyModule_Create2(IntPtr module, int apiver);

        IntPtr PyImport_Import(IntPtr name);

        IntPtr PyImport_ImportModule(string name);

        IntPtr PyImport_ReloadModule(IntPtr module);

        IntPtr PyImport_AddModule(string name);

        IntPtr PyImport_GetModuleDict();


        void PySys_SetArgv(int argc, IntPtr argv);

        IntPtr PySys_GetObject(string name);

        int PySys_SetObject(string name, IntPtr ob);


        //====================================================================
        // Python type object API
        //====================================================================

        void PyType_Modified(IntPtr type);

        bool PyType_IsSubtype(IntPtr t1, IntPtr t2);

        IntPtr PyType_GenericNew(IntPtr type, IntPtr args, IntPtr kw);

        IntPtr PyType_GenericAlloc(IntPtr type, int n);

        int PyType_Ready(IntPtr type);

        IntPtr _PyType_Lookup(IntPtr type, IntPtr name);

        IntPtr PyObject_GenericGetAttr(IntPtr obj, IntPtr name);

        int PyObject_GenericSetAttr(IntPtr obj, IntPtr name, IntPtr value);

        IntPtr _PyObject_GetDictPtr(IntPtr obj);

        IntPtr PyObject_GC_New(IntPtr tp);

        void PyObject_GC_Del(IntPtr tp);

        void PyObject_GC_Track(IntPtr tp);

        void PyObject_GC_UnTrack(IntPtr tp);


        //====================================================================
        // Python memory API
        //====================================================================

        IntPtr PyMem_Malloc(int size);

        IntPtr PyMem_Realloc(IntPtr ptr, int size);

        void PyMem_Free(IntPtr ptr);


        //====================================================================
        // Python exception API
        //====================================================================

        void PyErr_SetString(IntPtr ob, string message);

        void PyErr_SetObject(IntPtr ob, IntPtr message);

        IntPtr PyErr_SetFromErrno(IntPtr ob);

        void PyErr_SetNone(IntPtr ob);

        int PyErr_ExceptionMatches(IntPtr exception);

        int PyErr_GivenExceptionMatches(IntPtr ob, IntPtr val);

        void PyErr_NormalizeException(IntPtr ob, IntPtr val, IntPtr tb);

        int PyErr_Occurred();

        void PyErr_Fetch(ref IntPtr ob, ref IntPtr val, ref IntPtr tb);

        void PyErr_Restore(IntPtr ob, IntPtr val, IntPtr tb);

        void PyErr_Clear();

        void PyErr_Print();
                        

        //====================================================================
        // Miscellaneous
        //====================================================================

        IntPtr PyMethod_Self(IntPtr ob);

        IntPtr PyMethod_Function(IntPtr ob);
                        
    }
}