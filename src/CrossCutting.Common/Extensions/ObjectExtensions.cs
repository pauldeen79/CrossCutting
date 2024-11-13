namespace CrossCutting.Common.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Validate the object.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <exception cref="ValidationException">
    /// </exception>
    public static void Validate(this object instance)
        => Validator.ValidateObject(instance, new ValidationContext(instance, null, null), true);

    /// <summary>
    /// Validate the object.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <param name="validationResults">The validation results.</param>
    /// <exception cref="ValidationException">
    /// </exception>
    public static bool TryValidate(this object instance, ICollection<ValidationResult> validationResults)
        => Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), validationResults, true);

    /// <summary>
    /// Disposes this object, when it implements IDisposable.
    /// </summary>
    /// <param name="instance">The instance.</param>
    public static void TryDispose(this object? instance)
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
    public static string ToStringWithNullCheck(this object? value)
        => value is null
            ? string.Empty
            : value.ToString();

    /// <summary>
    /// Converts an object value to string with default value if null.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="defaultValueDelegate">The default value delegate.</param>
    /// <returns>
    /// value.ToString() when te value is not null, defaultValueDelegate result otherwise.
    /// </returns>
    public static string ToStringWithDefault(this object? value, Func<string> defaultValueDelegate)
    {
        ArgumentGuard.IsNotNull(defaultValueDelegate, nameof(defaultValueDelegate));

        return value.ToStringWithDefault(defaultValueDelegate.Invoke());
    }

    /// <summary>
    /// Converts an object value to string with default value if null.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>
    /// value.ToString() when te value is not null, defaultValue otherwise.
    /// </returns>
    public static string ToStringWithDefault(this object? value, string defaultValue = "")
        => value is null
            ? defaultValue
            : value.ToString();

    public static string ToString(this object? value, IFormatProvider formatProvider, string defaultValue = "")
    {
        if (value is null)
        {
            return defaultValue;
        }

        if (value is IFormattable f)
        {
            return f.ToString(null, formatProvider);
        }

        return value.ToString();
    }

    /// <summary>
    /// Determines whether the specified instance is true.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <returns></returns>
    public static bool IsTrue(this object? instance)
        => (instance is bool x && x) || instance.ToStringWithDefault().IsTrue();

    public static bool IsTrue<T>(this T instance, Func<T, bool> predicate)
        => predicate?.Invoke(instance) ?? false;

    /// <summary>
    /// Determines whether the specified instance is false.
    /// </summary>
    /// <param name="instance">The instance.</param>
    /// <returns></returns>
    public static bool IsFalse(this object? instance)
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
    {
        ArgumentGuard.IsNotNull(values, nameof(values));

        return values.Any(i => i?.Equals(value) == true);
    }

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
    {
        ArgumentGuard.IsNotNull(values, nameof(values));

        return Array.Exists(values, i => i?.Equals(value) == true);
    }

    public static ExpandoObject ToExpandoObject(this object? instance)
    {
        var expandoObject = new ExpandoObject();

        if (instance is not null)
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

    public static T Chain<T>(this T instance, Action action) => instance.Then(action);

    public static T Chain<T>(this T instance, Action<T> action) => instance.With(action);

    public static T Then<T>(this T instance, Action action)
    {
        ArgumentGuard.IsNotNull(action, nameof(action));

        action.Invoke();

        return instance;
    }

    public static TTarget Transform<TSource, TTarget>(this TSource instance, Func<TSource, TTarget> transformDelegate)
    {
        ArgumentGuard.IsNotNull(transformDelegate, nameof(transformDelegate));

        return transformDelegate.Invoke(instance);
    }

    public static T With<T>(this T instance, Action<T> action)
    {
        ArgumentGuard.IsNotNull(action, nameof(action));

        action.Invoke(instance);

        return instance;
    }

    public static T WithAll<T, TItem>(this T instance, IEnumerable<TItem> enumerable, Action<TItem> action)
    {
        ArgumentGuard.IsNotNull(enumerable, nameof(enumerable));
        ArgumentGuard.IsNotNull(action, nameof(action));

        foreach (var item in enumerable)
        {
            action.Invoke(item);
        }

        return instance;
    }

    public static T WithAll<T, TItem>(this T instance, Func<T, IEnumerable<TItem>> enumerable, Action<TItem> action)
    {
        ArgumentGuard.IsNotNull(enumerable, nameof(enumerable));
        ArgumentGuard.IsNotNull(action, nameof(action));

        foreach (var item in enumerable.Invoke(instance))
        {
            action.Invoke(item);
        }

        return instance;
    }

    public static Result<T> ToResult<T>(this T? instance) where T : class
        => Result.FromInstance(instance);

    public static Result<T> ToResult<T>(this T? instance, string errorMessage) where T : class
        => Result.FromInstance(instance, errorMessage);

    public static Result<T> ToResult<T>(this T? instance, IEnumerable<ValidationError> validationErrors) where T : class
        => Result.FromInstance(instance, validationErrors);

    public static Result<T> ToResult<T>(this T? instance, string errorMessage, IEnumerable<ValidationError> validationErrors) where T : class
        => Result.FromInstance(instance, errorMessage, validationErrors);
}
