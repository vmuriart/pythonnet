using System;

namespace Python.Runtime.InteropContracts
{
    public interface IPythonNativeMethodsInterop
    {
        IntPtr LoadLibrary(string dllToLoad);

        IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        bool FreeLibrary(IntPtr hModule);
    }
}
