using System;
using System.Runtime.InteropServices;

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


        //=====================================================================
        // InteropContract definitions
        //=====================================================================

        int IPythonRuntimeInterop.UCS => UCS;

        string IPythonRuntimeInterop.PythonDll => dll;

        public string PyVersion { get; } = pyversion;

        public int PyVersionNumber { get; } = pyversionnumber;
    }
}
