using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;

namespace CrossCutting.Common.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="ValidationException">
        /// </exception>
        public static void Validate(this object instance)
        {
            var context = new ValidationContext(instance, null, null);
            Validator.ValidateObject(instance, context, true);
        }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="validationResults">The validation results.</param>
        /// <exception cref="ValidationException">
        /// </exception>
        public static bool TryValidate(this object instance, ICollection<ValidationResult> validationResults)
        {
            var context = new ValidationContext(instance, null, null);
            return Validator.TryValidateObject(instance, context, validationResults, true);
        }

        /// <summary>
        /// Disposes this object, when it implements IDisposable.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static void TryDispose(this object instance)
        {
            if (instance is IDisposable disp)
            {
                disp.Dispose();
            }
        }

        /// <summary>
        /// Converts an object value to string with null check.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// value.ToString() when the value is not null, string.Empty otherwise.
        /// </returns>
        public static string ToStringWithNullCheck(this object value)
            => value == null
                ? string.Empty
                : value.ToString();

        /// <summary>
        /// Converts an object value to string with default value if null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// value.ToString() when te value is not null, defaultValue otherwise.
        /// </returns>
        public static string ToStringWithDefault(this object value, string defaultValue = "")
            => value == null
                ? defaultValue
                : value.ToString();

        /// <summary>
        /// Determines whether the specified instance is true.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool IsTrue(this object instance)
            => (instance is bool x && x) || instance.ToStringWithDefault().IsTrue();

        public static bool IsTrue<T>(this T instance, Func<T, bool> predicate)
            => predicate?.Invoke(instance) ?? false;

        /// <summary>
        /// Determines whether the specified instance is false.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool IsFalse(this object instance)
            => (instance is bool x && !x) || instance.ToStringWithDefault().IsFalse();

        public static bool IsFalse<T>(this T instance, Func<T, bool> predicate)
            => !predicate?.Invoke(instance) ?? false;

        /// <summary>
        /// Determines whether the specified value is contained within the specified sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to search for.</param>
        /// <param name="values">The sequence to search in.</param>
        /// <returns>
        /// true when found, otherwise false.
        /// </returns>
        public static bool In<T>(this T value, IEnumerable<T> values)
            => values.Any(i => i?.Equals(value) == true);

        /// <summary>
        /// Determines whether the specified value is contained within the specified sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value to search for.</param>
        /// <param name="values">The sequence to search in.</param>
        /// <returns>
        /// true when found, otherwise false.
        /// </returns>
        public static bool In<T>(this T value, params T[] values)
            => values.Any(i => i?.Equals(value) == true);

        public static ExpandoObject ToExpandoObject(this object instance)
        {
            var expandoObject = new ExpandoObject();

            if (instance != null)
            {
                var dictionary = (IDictionary<string, object>)expandoObject;
                if (instance is IEnumerable<KeyValuePair<string, object>> kvpEnum)
                {
                    dictionary.AddRange(kvpEnum);
                }
                else
                {
                    var properties = TypeDescriptor.GetProperties(instance).Cast<PropertyDescriptor>();
                    foreach (var prop in properties.OrderBy(p => p.Name))
                    {
                        dictionary.Add(prop.Name, prop.GetValue(instance));
                    }
                }
            }

            return expandoObject;
        }

        public static T GuardNull<T>(this T value, string argumentName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName);
            }

            return value;
        }
    }
}
