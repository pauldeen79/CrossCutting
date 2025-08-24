namespace CrossCutting.Utilities.ObjectDumper.Parts.Types;

public class EnumerableDumper : IObjectDumperPartWithCallback
{
    public IObjectDumperCallback? Callback { get; set; }

    public int Order => 40;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
    {
        builder = ArgumentGuard.IsNotNull(builder, nameof(builder));
        instanceType = ArgumentGuard.IsNotNull(instanceType, nameof(instanceType));

        if (instance is not string and IEnumerable enumerable)
        {
            builder.BeginNesting(indent, instanceType);
            builder.BeginEnumerable(indent, instanceType);

            var firstEnum = true;
            foreach (var item in enumerable)
            {
                builder.AddEnumerableItem(firstEnum, indent, false);
                if (firstEnum)
                {
                    firstEnum = false;
                }

                Callback?.Process(item, item?.GetType() ?? instanceType.GetGenericArguments()[0], builder, indent + 4, currentDepth + 1);
            }

            builder.EndEnumerable(indent, enumerable.GetType());

            return true;
        }

        return false;
    }

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor) => true;

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
}
