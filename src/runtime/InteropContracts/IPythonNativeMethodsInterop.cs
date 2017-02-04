using System;
using System.Runtime.InteropServices;

namespace Python.Runtime.InteropContracts
{
    public interface IPythonNativeMethodsInterop
    {
        bool FreeLibrary(IntPtr hModule);

        IntPtr LoadLibrary(string dllToLoad);

        IntPtr GetProcAddress(IntPtr hModule, string procedureName);
    }
}
