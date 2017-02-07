using System;
using System.Runtime.InteropServices;

namespace Python.Runtime.InteropContracts
{
    public interface IPythonRuntimeInterop
    {
        /// <summary>
        /// UCS mode (2 or 4).
        /// </summary>
        int UCS { get; }

        string PyVersion { get; }

        int PyVersionNumber { get; }

        string PythonDll { get; }
    }
}
