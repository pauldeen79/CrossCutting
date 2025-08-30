namespace CrossCutting.Utilities.ObjectDumper.Parts.Types;

public class DateTimeDumper : IObjectDumperPart
{
    public int Order => 50;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
    {
        builder = ArgumentGuard.IsNotNull(builder, nameof(builder));

        if (instance is DateTime dt)
        {
            builder.AddSingleValue(dt.ToString("yyyy-MM-dd HH:mm:ss"), instance.GetType());

            return true;
        }

        return false;
    }

    public IEnumerable<PropertyDescriptor> ProcessProperties(IEnumerable<PropertyDescriptor> source) => source;

    public bool ShouldProcess(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => true;

    public bool ShouldProcessProperty(object? instance, PropertyDescriptor propertyDescriptor) => true;

    public object? Transform(object? instance, IObjectDumperResultBuilder builder, int indent, int currentDepth) => instance;
}
