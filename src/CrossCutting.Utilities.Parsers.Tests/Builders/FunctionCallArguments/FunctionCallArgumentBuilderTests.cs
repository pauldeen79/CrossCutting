namespace CrossCutting.Utilities.Parsers.Tests.Builders.FunctionCallArguments;

public class FunctionCallArgumentBuilderTests
{
    [Fact]
    public void Can_Convert_Value_To_Typed_FunctionCallArgumentBuilder_Using_Implicit_Operator()
    {
        // Act
        ConstantArgumentBuilder<string> result = "Hello world";

        // Assert
        result.ShouldBeOfType<ConstantArgumentBuilder<string>>();
        result.Value.ShouldBe("Hello world");
    }

    [Fact]
    public void Can_Convert_Result_To_Typed_FunctionCallArgumentBuilder_Using_Implicit_Operator()
    {
        // Act
        ConstantResultArgumentBuilder<string> result = Result.Success("Hello world");

        // Assert
        result.ShouldBeOfType<ConstantResultArgumentBuilder<string>>();
        result.Result.Value.ShouldBe("Hello world");
    }
}
