namespace DataFramework.ModelFramework.Poc.Tests;

public sealed partial class IntegrationTests : IDisposable
{
    private ICatalogRepository Repository { get; }
    private IDatabaseEntityRetriever<Catalog> Retriever { get; }
    private IQueryProcessor QueryProcessor { get; }
    private DbConnection Connection { get; }
    private ServiceProvider ServiceProvider { get; }
    private IServiceScope Scope { get; }

    public IntegrationTests()
    {
        Connection = new DbConnection();
        Connection.AddResultForDataReader(cmd => cmd.CommandText.StartsWith("SELECT") && cmd.CommandText.Contains(" FROM [ExtraField]"),
                                          () => new[] { new ExtraField("Catalog", "MyField", null, 1, typeof(string).FullName, true) });

        ServiceProvider = new ServiceCollection()
            .AddCrossCuttingDataSql()
            .AddQueryEvaluatorSql()
            .AddExpressionEvaluator()
            .AddPdcNet()
            .AddSingleton<System.Data.Common.DbConnection>(Connection)
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        Scope = ServiceProvider.CreateScope();
        Repository = Scope.ServiceProvider.GetRequiredService<ICatalogRepository>();
        Retriever = Scope.ServiceProvider.GetRequiredService<IDatabaseEntityRetriever<Catalog>>();
        QueryProcessor = Scope.ServiceProvider.GetRequiredService<IQueryProcessor>();
    }

    public void Dispose()
    {
        Scope.Dispose();
        ServiceProvider.Dispose();
        Connection.Dispose();
    }
}
