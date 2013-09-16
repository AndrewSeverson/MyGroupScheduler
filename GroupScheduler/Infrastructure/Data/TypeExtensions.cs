using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupScheduler.Infrastructure.Data
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified type is concrete class.
        /// </summary>
        /// <param name="t">The type.</param>
        /// <returns>
        ///   true if this is not an abstract class/type definition/interface.
        /// </returns>
        public static bool IsConcrete(this Type t)
        {
            return !t.IsAbstract && !t.IsGenericTypeDefinition && !t.IsInterface;
        }
    }
}