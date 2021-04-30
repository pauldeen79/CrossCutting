using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CrossCutting.Utilities.ObjectDumper.Contracts;

namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers
{
#pragma warning disable CA1710 // Identifiers should have correct suffix
    public class ContextClass : IDictionary<string, object>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        private readonly IDictionary<string, object> _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextClass"/> class.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentNullException">
        /// commandHandler
        /// or
        /// host
        /// </exception>
        public ContextClass(string custom1, int custom2)
        {
            Custom1 = custom1;
            Custom2 = custom2;
            _state = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the custom1.
        /// </summary>
        /// <value>
        /// The custom1.
        /// </value>
        public string Custom1 { get; }

        /// <summary>
        /// Gets the custom2.
        /// </summary>
        /// <value>
        /// The custom2.
        /// </value>
        public int Custom2 { get; }

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

        public bool TryGetValue(string key, out object value) => _state.TryGetValue(key, out value);

        public void Add(KeyValuePair<string, object> item) => _state.Add(item);

        public void Clear() => _state.Clear();

        public bool Contains(KeyValuePair<string, object> item) => _state.Contains(item);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => _state.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<string, object> item) => _state.Remove(item);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _state.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_state).GetEnumerator();
    }

    public class ContextClassTypeHandler : IObjectDumperPartWithCallback
    {
        public IObjectDumperCallback Callback { get; set; }

        public int Order => 39;

        public bool Process(object instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
        {
            if (instance is ContextClass contextClass)
            {
                builder.BeginNesting(indent, instanceType);
                builder.BeginComplexType(indent, instanceType);


                builder.AddEnumerableItem(true, indent, true);
                builder.AddName(indent, nameof(ContextClass.Custom1));
                Callback?.Process(contextClass.Custom1, typeof(string), builder, indent + 4, currentDepth + 1);
                builder.AddEnumerableItem(false, indent, true);
                builder.AddName(indent, nameof(ContextClass.Custom2));
                Callback?.Process(contextClass.Custom2, typeof(int), builder, indent + 4, currentDepth + 1);

                foreach (var item in contextClass)
                {
                    builder.AddEnumerableItem(false, indent, false);
                    Callback?.Process(item, item.GetType() ?? instanceType.GetGenericArguments()[0], builder, indent + 4, currentDepth + 1);
                }

                builder.EndComplexType(false, indent, instanceType);

                return true;
            }

            return false;
        }

        public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

        public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

        public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor) => true;

        public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
    }
}
