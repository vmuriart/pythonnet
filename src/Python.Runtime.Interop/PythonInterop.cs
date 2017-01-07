namespace Python.Runtime
{
    using System.Runtime.InteropServices;

    using Python.Runtime.InteropContracts;

    public class PythonInterop : IPythonInterop
    {
        public IPythonNativeMethodsInterop NativeMethods { get; } = new PythonNativeMethodsInterop();

        public IPythonRuntimeInterop Runtime { get; } = new PythonRuntimeInterop();

        public OSPlatform TargetPlatform
        {
            get
            {
#if MONO_LINUX
                return OSPlatform.Linux;
#elif MONO_OSX
                return OSPlatform.OSX;
#else
                return OSPlatform.Windows;
#endif
            }
        }

        public bool IsPyDebug
        {
            get
            {
#if (PYTHON_WITH_PYDEBUG)
                return true;
#else
                return false;
#endif
            }
        }
    }
}