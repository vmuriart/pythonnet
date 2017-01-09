namespace Python.Runtime
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
#if !NET46
    using System.Runtime.Loader;
#endif

    using InteropContracts;

    public partial class NativeMethods
    {
        private static IPythonNativeMethodsInterop _interop;
        internal static void InitInterop(IPythonNativeMethodsInterop interop)
        {
            _interop = interop;
        }
    }

    public partial class Runtime
    {
        static Runtime()
        {
            var binLocation = typeof(Runtime).GetTypeInfo().Assembly.Location;
            var interopDllPath = Path.Combine(Path.GetDirectoryName(binLocation), "Python.Runtime.Interop.dll");
            InitInterop(interopDllPath);
        }

        internal static void InitInterop(IPythonInterop pythonInterop)
        {
            NativeMethods.InitInterop(pythonInterop.NativeMethods);
            _interop = pythonInterop.Runtime;
            pythonInterop.InitTypeOffset();
            pythonInterop.InitExceptionOffset();

            UCS = pythonInterop.Runtime.UCS;
            pyversion = pythonInterop.Runtime.PyVersion;
            pyversionnumber = pythonInterop.Runtime.PyVersionNumber;
            IsPython3 = pyversionnumber >= 30;
            IsPython2 = pyversionnumber < 30;
            IsWindows = pythonInterop.TargetPlatform == OSPlatform.Windows;
            IsOSX = pythonInterop.TargetPlatform == OSPlatform.OSX;
            IsLinux = pythonInterop.TargetPlatform == OSPlatform.Linux;
            IsPosix = IsLinux || IsOSX;
            IsPyDebug = pythonInterop.IsPyDebug;
            dll = pythonInterop.Runtime.PythonDll;
            TypeFlags.Init();
            Interop.Init();
            ObjectOffset.Init();
        }

        public static void InitInterop(string interopDllPath)
        {
#if NET46
            var assembly = Assembly.LoadFrom(interopDllPath);
#else
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(interopDllPath);
#endif
            var type = assembly.GetType("Python.Runtime.Interop.PythonInterop");
            var pythonInterop = (IPythonInterop)Activator.CreateInstance(type);
            InitInterop(pythonInterop);
        }
    }
}