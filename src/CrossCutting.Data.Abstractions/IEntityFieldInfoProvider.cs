namespace CrossCutting.Data.Abstractions;

public interface IEntityFieldInfoProvider
{
    Result<IEntityFieldInfo> Create(object query);
}
