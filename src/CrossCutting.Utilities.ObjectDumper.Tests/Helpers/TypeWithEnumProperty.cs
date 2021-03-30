namespace CrossCutting.Utilities.ObjectDumper.Tests.Helpers
{
    public enum MyEnumeration { A, B, C }

    public class TypeWithEnumProperty
    {
        public string Property1 { get; set; }
        public MyEnumeration Property2 { get; set; }
    }
}
