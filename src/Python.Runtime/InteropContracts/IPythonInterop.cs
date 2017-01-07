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
    }
}