namespace CrossCutting.Utilities.ObjectDumper.Parts.Types;

public class ComplexTypeDumper : IObjectDumperPartWithCallback
{
    public IObjectDumperCallback? Callback { get; set; }

    public int Order => 99;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
    {
        if (instance is null)
        {
            return false;
        }

        builder.BeginNesting(indent, instanceType);
        builder.BeginComplexType(indent, instanceType);

        var first = IterateChildren
        (
            new ComplexTypeDumperState
            (
                true,
                builder,
                indent,
                currentDepth
            ),
            () => Callback?.GetProperties(instance),
            property => ((PropertyDescriptor)property).Name,
            property => ((PropertyDescriptor)property).GetValue(instance),
            property => ((PropertyDescriptor)property).PropertyType
        );

        first = IterateChildren
        (
            new ComplexTypeDumperState
            (
                first,
                builder,
                indent,
                currentDepth
            ),
            () => Callback?.GetFields(instance),
            property => ((FieldInfo)property).Name,
            property => ((FieldInfo)property).GetValue(instance),
            property => ((FieldInfo)property).FieldType
        );

        builder.EndComplexType(first, indent, instanceType);

        return true;
    }

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

    private bool IterateChildren
    (
        ComplexTypeDumperState state,
        Func<IEnumerable?> enumerableDelegate,
        Func<object, string> getNameDelegate,
        Func<object, object> getValueDelegate,
        Func<object, Type> getTypeDelegate
    )
    {
        var first = state.First;
        foreach (var item in enumerableDelegate() ?? Array.Empty<object>())
        {
            state.Builder.AddEnumerableItem(first, state.Indent, true);
            if (state.First)
            {
                first = false;
            }

            state.Builder.AddName(state.Indent, getNameDelegate(item));

            try
            {
                Callback?.Process(getValueDelegate(item), getTypeDelegate(item), state.Builder, state.Indent + 4, state.CurrentDepth + 1);
            }
            catch (Exception ex)
            {
                state.Builder.AddException(ex);
            }
        }

        return first;
    }

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor) => true;

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;

    private sealed class ComplexTypeDumperState(bool first, IObjectDumperResultBuilder builder, int indent, int currentDepth)
    {
        public bool First { get; } = first;
        public IObjectDumperResultBuilder Builder { get; } = builder;
        public int Indent { get; } = indent;
        public int CurrentDepth { get; } = currentDepth;
    }
}
