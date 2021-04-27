namespace CrossCutting.Utilities.ObjectDumper.Contracts
{
    public interface IObjectDumperPartWithCallback : IObjectDumperPart
    {
        IObjectDumperCallback? Callback { get; set; }
    }
}
