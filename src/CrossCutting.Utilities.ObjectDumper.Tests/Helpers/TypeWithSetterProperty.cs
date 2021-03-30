namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers
{
    internal class TypeWithSetterProperty
    {
        public string Name { get; set; }
        public string Error { set { } }
    }
}
