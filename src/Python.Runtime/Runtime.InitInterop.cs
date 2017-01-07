namespace Python.Runtime
{
    using System;
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
#if NET46
            InitInterop("c:\\projects\\pythonnet\\r6\\subprojects\\pythonnet\\src\\Python.Runtime.Interop\\bin\\Debug\\net46\\Python.Runtime.Interop.dll");
#else
            InitInterop("c:\\projects\\pythonnet\\r6\\subprojects\\pythonnet\\src\\Python.Runtime.Interop\\bin\\Debug\\netstandard1.5\\Python.Runtime.Interop.dll");
#endif
        }

        internal static void InitInterop(IPythonInterop pythonInterop)
        {
            NativeMethods.InitInterop(pythonInterop.NativeMethods);
            _interop = pythonInterop.Runtime;

            UCS = pythonInterop.Runtime.UCS;
            pyversion = pythonInterop.Runtime.PyVersion;
            pyversionnumber = pythonInterop.Runtime.PyVersionNumber;
            IsPython3 = pyversionnumber >= 30;
            IsWindows = pythonInterop.TargetPlatform == OSPlatform.Windows;
            IsOSX = pythonInterop.TargetPlatform == OSPlatform.OSX;
            IsLinux = pythonInterop.TargetPlatform == OSPlatform.Linux;
            IsPosix = IsLinux || IsOSX;
            IsPyDebug = pythonInterop.IsPyDebug;
            ObjectOffset.Init();
        }

        public static void InitInterop(string interopDllPath)
        {
#if NET46
            var assembly = Assembly.LoadFrom(interopDllPath);
#else
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(interopDllPath);
#endif
            var type = assembly.GetType("Python.Runtime.PythonInterop");
            var pythonInterop = (IPythonInterop)Activator.CreateInstance(type);
            InitInterop(pythonInterop);
        }
    }
}