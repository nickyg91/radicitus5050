using System;
using System.Linq;

namespace Radicitus.Models.ReferenceMapper
{
    public static class ReferenceMapper
    {
        /// <summary>
        /// Maps an object TIn to a new instance of TOut using a common interface for properties to copy.
        /// This only does a shallow copy.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static TOut MapToNewInstance<TIn, TOut, TInterface>(TIn item)
        {
            var convertedTo = Activator.CreateInstance<TOut>();

            var properties = item.GetType().GetInterfaces().ToList().First()?.GetProperties();

            if (properties == null)
            {
                throw new Exception("No properties found on type.");
            }

            foreach (var property in properties)
            {
                convertedTo.GetType().GetProperty(property.Name)?.SetValue(convertedTo, property.GetValue(item));
            }

            return convertedTo;
        }
    }
}
