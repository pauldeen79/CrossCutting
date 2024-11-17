namespace CrossCutting.Utilities.ObjectDumper
{
    public class DynamicTypeDescriptionProvider(Type type) : TypeDescriptionProvider
    {
        private readonly TypeDescriptionProvider provider = TypeDescriptor.GetProvider(type);
        private readonly List<PropertyDescriptor> properties = [];

        public IList<PropertyDescriptor> Properties => properties;

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
            => new DynamicCustomTypeDescriptor(this, provider.GetTypeDescriptor(objectType, instance));

        private sealed class DynamicCustomTypeDescriptor(DynamicTypeDescriptionProvider provider,
           ICustomTypeDescriptor descriptor) : CustomTypeDescriptor(descriptor)
        {
            private readonly DynamicTypeDescriptionProvider provider = provider;

            public override PropertyDescriptorCollection GetProperties() => GetProperties([]);

            public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                var properties = new PropertyDescriptorCollection(null);

                foreach (PropertyDescriptor property in base.GetProperties(attributes))
                {
                    properties.Add(property);
                }

                foreach (PropertyDescriptor property in provider.Properties)
                {
                    properties.Add(property);
                }
                return properties;
            }
        }
    }
}
