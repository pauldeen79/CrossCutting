namespace CrossCutting.Utilities.ObjectDumper;

public sealed class ComposableObjectDumper : IObjectDumperCallback
{
    private readonly IObjectDumperPart[] _parts;

    public ComposableObjectDumper(IEnumerable<IObjectDumperPart> typeHandlers)
        => _parts = [.. typeHandlers
            .Select(x => x.PerformActionOnType<IObjectDumperPart, IObjectDumperPartWithCallback>(a => a.Callback = this))
            .OrderBy(x => x.Order)];

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
    {
        builder = ArgumentGuard.IsNotNull(builder, nameof(builder));

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            foreach (var part in _parts)
            {
                if (part is IObjectDumperPartWithCallback initializer && initializer.Callback is null)
                {
                    initializer.Callback = this;
                }
                instance = part.Transform(instance, builder, indent, currentDepth);
            }

            if (!Array.TrueForAll(_parts, part => part.ShouldProcess(instance, builder, indent, currentDepth)))
            {
                return true;
            }

            return Array.Find(_parts, part => part.Process(instance, instanceType, builder, indent, currentDepth)) is not null;
        }
        catch (Exception ex)
        {
            builder.AddException(ex);

            return true;
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    IEnumerable<PropertyDescriptor> IObjectDumperCallback.GetProperties(object instance)
        => _parts.Aggregate
            (
                GetPropertyDescriptors(instance),
                (seed, part) => part.ProcessProperties(seed)
            );

    IEnumerable<FieldInfo> IObjectDumperCallback.GetFields(object instance)
        => instance
            .GetType()
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
                .Where(fi => !fi.Name.EndsWithAny(">i__Field", ">k__BackingField"));

    private IEnumerable<PropertyDescriptor> GetPropertyDescriptors(object instance)
        => TypeDescriptor
            .GetProperties(instance)
            .Cast<PropertyDescriptor>()
            .Where(prop => Array.TrueForAll(_parts, part => part.ShouldProcessProperty(instance, prop)));
}
