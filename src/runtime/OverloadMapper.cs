using System;
using System.Reflection;

namespace Python.Runtime
{
    /// <summary>
    /// Implements the __overloads__ attribute of method objects. This object
    /// supports the [] syntax to explicitly select an overload by signature.
    /// </summary>
    internal class OverloadMapper : ExtensionType
    {
        private readonly MethodObject _method;
        private readonly IntPtr _target;

        public OverloadMapper(MethodObject method, IntPtr target)
        {
            Runtime.XIncref(target);
            _target = target;
            _method = method;
        }

        /// <summary>
        /// Implement explicit overload selection using subscript syntax ([]).
        /// </summary>
        public static IntPtr mp_subscript(IntPtr tp, IntPtr idx)
        {
            var self = (OverloadMapper)GetManagedObject(tp);

            // Note: if the type provides a non-generic method with N args
            // and a generic method that takes N params, then we always
            // prefer the non-generic version in doing overload selection.

            Type[] types = Runtime.PythonArgsToTypeArray(idx);
            if (types == null)
            {
                return Exceptions.RaiseTypeError("type(s) expected");
            }

            MethodInfo mi = MethodBinder.MatchSignature(self._method.info, types);
            if (mi == null)
            {
                return Exceptions.RaiseTypeError("No match found for signature");
            }

            var mb = new MethodBinding(self._method, self._target) { info = mi };
            Runtime.XIncref(mb.pyHandle);
            return mb.pyHandle;
        }

        /// <summary>
        /// OverloadMapper  __repr__ implementation.
        /// </summary>
        public static IntPtr tp_repr(IntPtr op)
        {
            var self = (OverloadMapper)GetManagedObject(op);
            IntPtr doc = self._method.GetDocString();
            Runtime.XIncref(doc);
            return doc;
        }

        /// <summary>
        /// OverloadMapper dealloc implementation.
        /// </summary>
        public new static void tp_dealloc(IntPtr ob)
        {
            var self = (OverloadMapper)GetManagedObject(ob);
            Runtime.XDecref(self._target);
            FinalizeObject(self);
        }
    }
}
