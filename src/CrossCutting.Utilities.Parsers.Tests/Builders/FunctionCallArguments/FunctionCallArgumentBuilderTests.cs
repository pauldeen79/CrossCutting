namespace CrossCutting.Utilities.Parsers.Tests.Builders.FunctionCallArguments;

public class FunctionCallArgumentBuilderTests
{
    [Fact]
    public void Can_Convert_Value_To_Typed_FunctionCallArgumentBuilder_Using_Implicit_Operator()
    {
        // Act
        FunctionCallArgumentBuilder<string> result = "Hello world";

        // Assert
        result.Should().BeOfType<ConstantArgumentBuilder<string>>();
        ((ConstantArgumentBuilder<string>)result).Value.Should().Be("Hello world");
    }

    [Fact]
    public void Can_Convert_Result_To_Typed_FunctionCallArgumentBuilder_Using_Implicit_Operator()
    {
        // Act
        FunctionCallArgumentBuilder<string> result = Result.Success("Hello world");

        // Assert
        result.Should().BeOfType<ConstantResultArgumentBuilder<string>>();
        ((ConstantResultArgumentBuilder<string>)result).Result.Value.Should().Be("Hello world");
    }
}
