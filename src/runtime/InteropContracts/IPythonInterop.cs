using System;
using System.Runtime.InteropServices;

namespace Python.Runtime.InteropContracts
{
    /// <summary>
    /// Interop methods adapter contracts root.
    /// </summary>
    public interface IPythonInterop
    {
        IPythonNativeMethodsInterop NativeMethods { get; }

        IPythonRuntimeInterop Runtime { get; }

        string TargetPlatform { get; }

        bool IsPyDebug { get; }

        /// <summary>
        /// Initializes values in the <see cref="TypeOffset"/>.
        /// </summary>
        void InitTypeOffset();

        /// <summary>
        /// Initializes values in the <see cref="ExceptionOffset"/>.
        /// </summary>
        void InitExceptionOffset();
    }
}
