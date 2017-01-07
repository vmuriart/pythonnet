namespace Python.Runtime.InteropContracts
{
    /// <summary>
    /// Interop methods adapter contracts root.
    /// </summary>
    public interface IPythonInterop
    {
        IPythonNativeMethodsInterop NativeMethods { get; }
    }
}