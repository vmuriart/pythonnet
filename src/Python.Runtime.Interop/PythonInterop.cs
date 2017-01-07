namespace Python.Runtime
{
    using Python.Runtime.InteropContracts;

    public class PythonInterop : IPythonInterop
    {
        public PythonInterop()
        {
            NativeMethods = new PythonNativeMethodsInterop();
        }

        public IPythonNativeMethodsInterop NativeMethods { get; }
    }
}