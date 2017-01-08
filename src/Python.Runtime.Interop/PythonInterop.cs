namespace Python.Runtime
{
    using System;
    using System.Linq;
    using System.Reflection;
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

        public void InitTypeOffset()
        {
            CopyStaticFields<Interop.TypeOffset, TypeOffset>();
        }

        private void CopyStaticFields<TFrom, TTo>()
        {
            Type type = typeof(TFrom);
            var actualValues = type.GetFields().ToDictionary(x => x.Name);

            Type targetType = typeof(TTo);
            var targetFields = targetType.GetFields();
            for (int i = 0; i < targetFields.Length; i++)
            {
                FieldInfo actualField;
                if (actualValues.TryGetValue(targetFields[i].Name, out actualField))
                {
                    targetFields[i].SetValue(null, actualField.GetValue(null));
                }
            }
        }

        public void InitExceptionOffset()
        {
            CopyStaticFields<Interop.ExceptionOffset, ExceptionOffset>();
        }
    }
}