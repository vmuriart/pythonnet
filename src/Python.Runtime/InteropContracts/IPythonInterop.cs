namespace Python.Runtime.InteropContracts
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Interop methods adapter contracts root.
    /// </summary>
    public interface IPythonInterop
    {
        IPythonNativeMethodsInterop NativeMethods { get; }

        IPythonRuntimeInterop Runtime { get; }

        OSPlatform TargetPlatform { get; }

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