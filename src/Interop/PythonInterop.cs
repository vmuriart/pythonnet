using System;
using System.Runtime.InteropServices;
using Python.Runtime.InteropContracts;

namespace Python.Runtime.Interop
{
    public class PythonInterop : IPythonInterop
    {
        private readonly IPythonNativeMethodsInterop _nativeMethods = new PythonNativeMethodsInterop();
        private readonly IPythonRuntimeInterop _runtime = new PythonRuntimeInterop();

        public IPythonNativeMethodsInterop NativeMethods
        {
            get { return _nativeMethods; }
        }

        public IPythonRuntimeInterop Runtime
        {
            get { return _runtime; }
        }

        public string TargetPlatform
        {
            get
            {
#if MONO_LINUX
                return "Linux";
#elif MONO_OSX
                return "OSX";
#else
                return "Windows";
#endif
            }
        }

        public bool IsPyDebug
        {
            get
            {
#if PYTHON_WITH_PYDEBUG
                return true;
#else
                return false;
#endif
            }
        }
    }
}
