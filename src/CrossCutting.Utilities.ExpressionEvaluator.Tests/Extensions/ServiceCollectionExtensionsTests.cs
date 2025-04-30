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
    public void Can_Resolve_ExpressionComponents(Type expressionType)
    {
        // Arrange
        var serviceCollection = new ServiceCollection().AddExpressionEvaluator();
        using var provider = serviceCollection.BuildServiceProvider();

        // Act
        var expression = provider.GetServices<IExpressionComponent>().FirstOrDefault(x => x.GetType() == expressionType);

        // Assert
        expression.ShouldNotBeNull($"Expression {expressionType.FullName} could not be resolved, did you forget to register this?");
    }

    [Theory]
    [MemberData(nameof(GetAllMembers))]
    public void Can_Resolve_Member(Type memberType)
    {
        // Arrange
        var serviceCollection = new ServiceCollection().AddExpressionEvaluator();
        using var provider = serviceCollection.BuildServiceProvider();

        // Act
        var function = provider.GetServices<IMember>().FirstOrDefault(x => x.GetType() == memberType);

        // Assert
        function.ShouldNotBeNull($"Member {memberType.FullName} could not be resolved, did you forget to register this?");
    }

    [Theory]
    [MemberData(nameof(GetAllDotExpressionComponents))]
    public void Can_Resolve_DotExpressionComponent(Type dotExpressionComponentType)
    {
        // Arrange
        var serviceCollection = new ServiceCollection().AddExpressionEvaluator();
        using var provider = serviceCollection.BuildServiceProvider();

        // Act
        var dotExpressionComponent = provider.GetServices<IDotExpressionComponent>().FirstOrDefault(x => x.GetType() == dotExpressionComponentType);

        // Assert
        dotExpressionComponent.ShouldNotBeNull($"Function {dotExpressionComponentType.FullName} could not be resolved, did you forget to register this?");
    }

    public static TheoryData<Type> GetAllExpressions()
    {
        var data = new TheoryData<Type>();
        foreach (var t in typeof(IExpressionComponent).Assembly.GetExportedTypes().Where(x => !x.IsInterface && !x.IsAbstract && x.GetAllInterfaces().Contains(typeof(IExpressionComponent))))
        {
            data.Add(t);
        }

        return data;
    }

    public static TheoryData<Type> GetAllMembers()
    {
        var data = new TheoryData<Type>();
        foreach (var t in typeof(IMember).Assembly.GetExportedTypes().Where(x => !x.IsInterface && !x.IsAbstract && x.GetAllInterfaces().Contains(typeof(IMember))))
        {
            data.Add(t);
        }

        return data;
    }

    public static TheoryData<Type> GetAllDotExpressionComponents()
    {
        var data = new TheoryData<Type>();
        foreach (var t in typeof(IDotExpressionComponent).Assembly.GetExportedTypes().Where(x => !x.IsInterface && !x.IsAbstract && x.GetAllInterfaces().Contains(typeof(IDotExpressionComponent))))
        {
            data.Add(t);
        }

        return data;
    }
}
