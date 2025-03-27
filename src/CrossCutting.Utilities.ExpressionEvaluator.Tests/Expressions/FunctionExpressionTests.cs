namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class FunctionExpressionTests : TestBase
{
    protected IFunction Function { get; }

    protected FunctionExpression CreateSut(IFunction? function = null)
        => new FunctionExpression([function ?? Function]);

    public FunctionExpressionTests()
    {
        Function = Substitute.For<IFunction>();
    }

    public class Evaluate : FunctionExpressionTests
    {
        //no regex match (parse result not found) -> continue
        //not succesfully parsed -> parse result
        //no function match -> not found
        //function match -> result of function Evaluate
    }

    public class Parse : FunctionExpressionTests
    {
        //no regex match (parse result not found) -> continue
        //not succesfully parsed -> parse result
        //parse successful, no function match -> not found
        //parse successful, function match -> ok, result type with result of function Evaluate
    }

    public class ParseFunctionCall : FunctionExpressionTests
    {
        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_Without_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction()");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction(argument1, argument2, argument3)");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBe(["argument1", "argument2", "argument3"]);
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Generics_Without_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>()");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBe([typeof(string)]);
            result.Value.Arguments.ShouldBeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Generics_And_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>(argument1, argument2, argument3)");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBe([typeof(string)]);
            result.Value.Arguments.ShouldBe(["argument1", "argument2", "argument3"]);
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Nested_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction(argument1, MySubFunction(argument2), argument3)");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBe(["argument1", "MySubFunction(argument2)", "argument3"]);
        }

        [Fact]
        public void Returns_Invalid_On_FunctionName_With_WhiteSpace()
        {
            // Arrange
            var context = CreateContext("My Function()");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Function name may not contain whitespace");
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Spaces_Tabs_And_NewLines()
        {
            // Arrange
            var context = CreateContext("       MyFunction(\r\n\r\n\r\n   \t\t)\t\t\t");

            // Act
            var result = FunctionExpression.ParseFunctionCall(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBeEmpty();
        }

        //"Missing open bracket" (before generics)
        //"Missing open bracket" (after generics)
        //"Too many close brackets found"
        //"Input has additional characters after last close bracket"
        //"Missing open bracket" (name not complete)
        //"Generic type name is not properly ended"
        //"Missing close bracket"
        //invalid type argument (Unknown type: xxx)
    }
}
