using System;
using System.Runtime.InteropServices;
using Python.Runtime.InteropContracts;

namespace Python.Runtime.Interop
{
    public class PythonNativeMethodsInterop : IPythonNativeMethodsInterop
    {
#if MONO_LINUX || MONO_OSX
        private static int RTLD_NOW = 0x2;
        private static int RTLD_SHARED = 0x20;
#if MONO_OSX
        private static IntPtr RTLD_DEFAULT = new IntPtr(-2);
        private const string NativeDll = "__Internal";
#elif MONO_LINUX
        private static IntPtr RTLD_DEFAULT = IntPtr.Zero;
        private const string NativeDll = "libdl.so";
#endif

        public static IntPtr LoadLibrary(string dllToLoad)
        {
            return dlopen(dllToLoad, RTLD_NOW | RTLD_SHARED);
        }

        public static bool FreeLibrary(IntPtr hModule)
        {
            dlclose(hModule);
            return true;
        }

        public static IntPtr GetProcAddress(IntPtr hModule, string procedureName)
        {
            // look in the exe if hModule is NULL
            if (hModule == IntPtr.Zero)
            {
                hModule = RTLD_DEFAULT;
            }

            // clear previous errors if any
            dlerror();
            IntPtr res = dlsym(hModule, procedureName);
            IntPtr errPtr = dlerror();
            if (errPtr != IntPtr.Zero)
            {
                throw new Exception("dlsym: " + Marshal.PtrToStringAnsi(errPtr));
            }
            return res;
        }

        [DllImport(NativeDll)]
        private static extern IntPtr dlopen(String fileName, int flags);

        [DllImport(NativeDll)]
        private static extern IntPtr dlsym(IntPtr handle, String symbol);

        [DllImport(NativeDll)]
        private static extern int dlclose(IntPtr handle);

        [DllImport(NativeDll)]
        private static extern IntPtr dlerror();
#else // Windows
        private const string NativeDll = "kernel32.dll";

        [DllImport(NativeDll)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport(NativeDll)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(NativeDll)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
#endif

        IntPtr IPythonNativeMethodsInterop.LoadLibrary(string dllToLoad)
        {
            return LoadLibrary(dllToLoad);
        }

        bool IPythonNativeMethodsInterop.FreeLibrary(IntPtr hModule)
        {
            return FreeLibrary(hModule);
        }

        IntPtr IPythonNativeMethodsInterop.GetProcAddress(IntPtr hModule, string procedureName)
        {
            return GetProcAddress(hModule, procedureName);
        }
    }
}
