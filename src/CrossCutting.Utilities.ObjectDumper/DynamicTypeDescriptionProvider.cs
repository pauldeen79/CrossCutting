namespace CrossCutting.Utilities.ObjectDumper
{
    public class DynamicTypeDescriptionProvider : TypeDescriptionProvider
    {
        private readonly TypeDescriptionProvider provider;
        private readonly List<PropertyDescriptor> properties = new List<PropertyDescriptor>();

        public DynamicTypeDescriptionProvider(Type type) => provider = TypeDescriptor.GetProvider(type);

        public IList<PropertyDescriptor> Properties => properties;

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
            => new DynamicCustomTypeDescriptor(this, provider.GetTypeDescriptor(objectType, instance));

        private sealed class DynamicCustomTypeDescriptor : CustomTypeDescriptor
        {
            private readonly DynamicTypeDescriptionProvider provider;

            public DynamicCustomTypeDescriptor(DynamicTypeDescriptionProvider provider,
               ICustomTypeDescriptor descriptor)
                  : base(descriptor) => this.provider = provider;

            public override PropertyDescriptorCollection GetProperties() => GetProperties(Array.Empty<Attribute>());

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
