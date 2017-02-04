using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Python.Runtime.InteropContracts;

namespace Python.Runtime
{
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
            InitInterop();
        }

        public static void InitInterop()
        {
            Assembly assembly = Assembly.Load(new AssemblyName("Python.Runtime.Interop"));
            Type type = assembly.GetType("Python.Runtime.Interop.PythonInterop");

            var pythonInterop = (IPythonInterop)Activator.CreateInstance(type);
            InitInterop(pythonInterop);
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
            IsWindows = true;
            IsOSX = false;
            IsLinux = false;
            IsPosix = IsLinux || IsOSX;
            IsPyDebug = pythonInterop.IsPyDebug;
            dll = pythonInterop.Runtime.PythonDll;

            TypeFlags.Init();
            Interop.Init();
            ObjectOffset.Init();
        }
    }
}
