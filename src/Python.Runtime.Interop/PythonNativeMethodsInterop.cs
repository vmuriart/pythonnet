using System;

namespace Python.Runtime
{
    using System.Runtime.InteropServices;

    using InteropContracts;
    public class PythonNativeMethodsInterop: IPythonNativeMethodsInterop
    {
#if (MONO_LINUX || MONO_OSX)
        static public IntPtr LoadLibrary(string fileName) {
            return dlopen(fileName, RTLD_NOW | RTLD_SHARED);
        }

        static public void FreeLibrary(IntPtr handle) {
            dlclose(handle);
        }

        static public IntPtr GetProcAddress(IntPtr dllHandle, string name) {
            // look in the exe if dllHandle is NULL
            if (IntPtr.Zero == dllHandle)
                dllHandle = RTLD_DEFAULT;

            // clear previous errors if any
            dlerror();
            var res = dlsym(dllHandle, name);
            var errPtr = dlerror();
            if (errPtr != IntPtr.Zero) {
                throw new Exception("dlsym: " + Marshal.PtrToStringAnsi(errPtr));
            }
            return res;
        }

#if (MONO_OSX)
        static int RTLD_NOW = 0x2;
        static int RTLD_SHARED = 0x20;
        static IntPtr RTLD_DEFAULT = new IntPtr(-2);

        [DllImport("__Internal")]
        private static extern IntPtr dlopen(String fileName, int flags);

        [DllImport("__Internal")]
        private static extern IntPtr dlsym(IntPtr handle, String symbol);

        [DllImport("__Internal")]
        private static extern int dlclose(IntPtr handle);

        [DllImport("__Internal")]
        private static extern IntPtr dlerror();
#else
        static int RTLD_NOW = 0x2;
        static int RTLD_SHARED = 0x20;
        static IntPtr RTLD_DEFAULT = IntPtr.Zero;

        [DllImport("libdl.so")]
        private static extern IntPtr dlopen(String fileName, int flags);

        [DllImport("libdl.so")]
        private static extern IntPtr dlsym(IntPtr handle, String symbol);

        [DllImport("libdl.so")]
        private static extern int dlclose(IntPtr handle);

        [DllImport("libdl.so")]
        private static extern IntPtr dlerror();

        IntPtr IPythonNativeMethodsInterop.LoadLibrary(string dllToLoad)
        {
            return LoadLibrary(dllToLoad);
        }

        IntPtr IPythonNativeMethodsInterop.GetProcAddress(IntPtr hModule, string procedureName)
        {
            return GetProcAddress(hModule, procedureName);
        }

        bool IPythonNativeMethodsInterop.FreeLibrary(IntPtr hModule)
        {
            FreeLibrary(hModule);
            return true;
        }
#endif

#else
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        IntPtr IPythonNativeMethodsInterop.LoadLibrary(string dllToLoad)
        {
            return LoadLibrary(dllToLoad);
        }

        IntPtr IPythonNativeMethodsInterop.GetProcAddress(IntPtr hModule, string procedureName)
        {
            return GetProcAddress(hModule, procedureName);
        }

        bool IPythonNativeMethodsInterop.FreeLibrary(IntPtr hModule)
        {
            return FreeLibrary(hModule);
        }
#endif


    }
}
