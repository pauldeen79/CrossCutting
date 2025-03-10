﻿namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers;

/// <summary>
/// Initializes a new instance of the <see cref="ContextDictionary"/> class.
/// </summary>
/// <param name="commandHandler">The command handler.</param>
/// <param name="host">The host.</param>
/// <exception cref="ArgumentNullException">
/// commandHandler
/// or
/// host
/// </exception>
public class ContextDictionary(string custom1, int custom2) : IDictionary<string, object>
{
    private readonly IDictionary<string, object> _state = new Dictionary<string, object>();

    /// <summary>
    /// Gets the custom1.
    /// </summary>
    /// <value>
    /// The custom1.
    /// </value>
    public string Custom1 { get; } = custom1;

    /// <summary>
    /// Gets the custom2.
    /// </summary>
    /// <value>
    /// The custom2.
    /// </value>
    public int Custom2 { get; } = custom2;

    public ICollection<string> Keys => _state.Keys;

    public ICollection<object> Values => _state.Values;

    public int Count => _state.Count;

    public bool IsReadOnly => _state.IsReadOnly;

    public object this[string key]
    {
        get => _state[key];
        set => _state[key] = value;
    }

    public void Add(string key, object value) => _state.Add(key, value);

    public bool ContainsKey(string key) => _state.ContainsKey(key);

    public bool Remove(string key) => _state.Remove(key);

    public bool TryGetValue(string key, out object value) => _state.TryGetValue(key, out value!);

    public void Add(KeyValuePair<string, object> item) => _state.Add(item);

    public void Clear() => _state.Clear();

    public bool Contains(KeyValuePair<string, object> item) => _state.Contains(item);

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => _state.CopyTo(array, arrayIndex);

    public bool Remove(KeyValuePair<string, object> item) => _state.Remove(item);

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _state.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_state).GetEnumerator();
}
