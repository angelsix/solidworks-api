using System;
using System.Collections.Generic;
using System.Linq;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Helper methods for enumerators
    /// </summary>
    public static class EnumHelpers
    {
        /// <summary>
        /// Gets all flags from an enumerator
        /// </summary>
        /// <typeparam name="T">The type of enumerator</typeparam>
        /// <param name="enumerator">The value of the enumerator</param>
        /// <returns></returns>
        public static List<T> GetFlags<T>(this T enumerator)
            where T : struct, IConvertible
        {
            // Make sure the value passed in is an enumerator
            if (!typeof(T).IsEnum)
                throw new InvalidCastException("Value passed into GetFlags must be an enumerator");

            // Get underlying value
            var value = (int)(IConvertible)enumerator;

            // Get all enumerator values for this type
            var enumValues = Enum.GetValues(enumerator.GetType()).Cast<int>();

            // Check if we have the flag in the enumerator values
            // NOTE: 0 is a "None" value for flagged enums so ignore that
            var results = enumValues.Where(e => e > 0 && (e & value) == e).Cast<T>().ToList();

            // Return the results
            return results;
        }
    }
}
