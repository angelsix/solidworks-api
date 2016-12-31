using System;
using System.Linq;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Helper methods for reflection and types
    /// </summary>
    public static class TypeHelpers
    {
        /// <summary>
        /// Checks if a type is a specific interface, or inherits it
        /// </summary>
        /// <typeparam name="T">The interface to check</typeparam>
        /// <param name="type">The type to check if it is or inherits the interface</param>
        /// <returns></returns>
        public static bool IsInterfaceOrHasInterface<T>(this Type type)
        {
            return type == typeof(T) || type.GetInterfaces().Contains(typeof(T));
        }
    }
}
