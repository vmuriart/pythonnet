namespace Python.Runtime.Interop
{
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    using InteropContracts;

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

        public void InitExceptionOffset()
        {
            CopyStaticFields<ExceptionOffset, Python.Runtime.ExceptionOffset>();
        }

        public void InitTypeOffset()
        {
            CopyStaticFields<TypeOffset, Python.Runtime.TypeOffset>();
        }

        private void CopyStaticFields<TFrom, TTo>()
        {
            var type = typeof(TFrom);
            var actualValues = type.GetFields().ToDictionary(x => x.Name);

            var targetType = typeof(TTo);
            var targetFields = targetType.GetFields();
            for (var i = 0; i < targetFields.Length; i++)
            {
                FieldInfo actualField;
                if (actualValues.TryGetValue(targetFields[i].Name, out actualField))
                {
                    targetFields[i].SetValue(null, actualField.GetValue(null));
                }
            }
        }
    }
}