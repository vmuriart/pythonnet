using System;
using System.Runtime.InteropServices;
using Python.Runtime.InteropContracts;

namespace Python.Runtime.Interop
{
    public class PythonInterop : IPythonInterop
    {
        private readonly IPythonNativeMethodsInterop _nativeMethods = new PythonNativeMethodsInterop();

        public IPythonNativeMethodsInterop NativeMethods
        {
            get { return _nativeMethods; }
        }
    }
}
