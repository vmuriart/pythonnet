using System;
using System.Collections;
using System.Reflection;
#if NET46
using System.Security.Permissions;
#endif
namespace Python.Runtime
{
    /// <summary>
    /// Module level properties (attributes)
    /// </summary>
    internal class ModulePropertyObject : ExtensionType
    {
        public ModulePropertyObject(PropertyInfo md) : base()
        {
            throw new NotImplementedException("ModulePropertyObject");
        }
    }
}