namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void Can_Build_ServiceProvider_Without_Any_Errors()
    {
        // Arrange
        var serviceCollection = new ServiceCollection().AddExpressionEvaluator();

        // Act & Assert
        Action a = () => _ = serviceCollection.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        a.ShouldNotThrow();
    }

    [Theory]
    [MemberData(nameof(GetAllExpressions))]
    public void Can_Resolve_Expression(Type expressionType)
    {
        // Arrange
        var serviceCollection = new ServiceCollection().AddExpressionEvaluator();
        using var provider = serviceCollection.BuildServiceProvider();

        // Act
        var expression = provider.GetServices<IExpression>().FirstOrDefault(x => x.GetType() == expressionType);

        // Assert
        expression.ShouldNotBeNull($"Expression {expressionType.FullName} could not be resolved, did you forget to register this?");
    }

    public static TheoryData<Type> GetAllExpressions()
    {
        var data = new TheoryData<Type>();
        foreach (var t in typeof(IExpression).Assembly.GetExportedTypes().Where(x => !x.IsInterface && !x.IsAbstract && x.GetAllInterfaces().Contains(typeof(IExpression))))
        {
            data.Add(t);
        }

        return data;
    }
}
