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
        }
    }
}
