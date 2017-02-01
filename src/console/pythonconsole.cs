using System;
using System.Reflection;
using System.Collections.Generic;
using Python.Runtime;
#if !NET46
using System.Runtime.Loader;
#endif
namespace Python.Runtime
{
    public sealed class PythonConsole
    {
        private PythonConsole()
        {
        }

        [STAThread]
        public static int Main(string[] args)
        {
            // reference the static assemblyLoader to stop it being optimized away
            AssemblyLoader a = assemblyLoader;

            string[] cmd = Environment.GetCommandLineArgs();
            PythonEngine.Initialize();

            int i = Runtime.Py_Main(cmd.Length, cmd);
            PythonEngine.Shutdown();

            return i;
        }

        // Register a callback function to load embedded assmeblies.
        // (Python.Runtime.dll is included as a resource)
        private sealed class AssemblyLoader
        {
            Dictionary<string, Assembly> loadedAssemblies;

            public AssemblyLoader()
            {
                loadedAssemblies = new Dictionary<string, Assembly>();

                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    string shortName = args.Name.Split(',')[0];
                    String resourceName = shortName + ".dll";

                    if (loadedAssemblies.ContainsKey(resourceName))
                    {
                        return loadedAssemblies[resourceName];
                    }

                    // looks for the assembly from the resources and load it
                    using (var stream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName))
                    {
                        if (stream != null)
                        {
#if NET46
                            Byte[] assemblyData = new Byte[stream.Length];
                            stream.Read(assemblyData, 0, assemblyData.Length);
                            Assembly assembly = Assembly.Load(assemblyData);
#else
                            Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(stream);
#endif
                            loadedAssemblies[resourceName] = assembly;
                            return assembly;
                        }
                    }

                    return null;
                };
            }
        };

        private static AssemblyLoader assemblyLoader = new AssemblyLoader();
    };
}