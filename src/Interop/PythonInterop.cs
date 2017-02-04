using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Python.Runtime.InteropContracts;

namespace Python.Runtime.Interop
{
    public class PythonInterop : IPythonInterop
    {
        private readonly IPythonRuntimeInterop _runtime = new PythonRuntimeInterop();
        private readonly IPythonNativeMethodsInterop _nativeMethods = new PythonNativeMethodsInterop();

        public IPythonNativeMethodsInterop NativeMethods
        {
            get { return _nativeMethods; }
        }

        public IPythonRuntimeInterop Runtime
        {
            get { return _runtime; }
        }

        public string TargetPlatform
        {
            get
            {
#if MONO_LINUX
                return "Linux";
#elif MONO_OSX
                return "OSX";
#else
                return "Windows";
#endif
            }
        }

        public bool IsPyDebug
        {
            get
            {
#if PYTHON_WITH_PYDEBUG
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
            Type type = typeof(TFrom);
            var actualValues = new Dictionary<string, FieldInfo>();
            foreach (FieldInfo field in type.GetFields())
                actualValues.Add(field.Name, field);

            Type targetType = typeof(TTo);
            FieldInfo[] targetFields = targetType.GetFields();
            foreach (FieldInfo t in targetFields)
            {
                FieldInfo actualField;
                if (actualValues.TryGetValue(t.Name, out actualField))
                {
                    t.SetValue(null, actualField.GetValue(null));
                }
            }
        }
    }
}
