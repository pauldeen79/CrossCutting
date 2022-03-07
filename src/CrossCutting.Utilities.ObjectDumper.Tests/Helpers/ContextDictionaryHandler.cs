namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers;

public class ContextDictionaryHandler : IObjectDumperPartWithCallback
{
    public IObjectDumperCallback? Callback { get; set; }

    public int Order => 39;

    public bool Process(object? instance, Type instanceType, IObjectDumperResultBuilder builder, int indent, int currentDepth)
    {
        if (instance is ContextDictionary contextClass)
        {
            builder.BeginNesting(indent, instanceType);
            builder.BeginComplexType(indent, instanceType);


            builder.AddEnumerableItem(true, indent, true);
            builder.AddName(indent, nameof(ContextDictionary.Custom1));
            Callback?.Process(contextClass.Custom1, typeof(string), builder, indent + 4, currentDepth + 1);
            builder.AddEnumerableItem(false, indent, true);
            builder.AddName(indent, nameof(ContextDictionary.Custom2));
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
