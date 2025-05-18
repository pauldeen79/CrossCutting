namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class FunctionParserTests : TestBase<FunctionParser>
{
    public class ParseAsync : FunctionParserTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("no function")]
        [InlineData("object.ToString()")]
        public void Returns_NotFound_When_Expression_Is_Not_A_FunctionCall(string expression)
        {
            // Arrange
            var context = CreateContext(expression);
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.Value.ShouldBeNull();
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_Without_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction()");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

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
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBe(["argument1", "argument2", "argument3"]);
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Arguments_And_Post_Suffix()
        {
            // Arrange
            var context = CreateContext("MyFunction(argument1, argument2, argument3) this is not correct");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Input has additional characters after last close bracket");
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Generics_Without_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>()");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBe([typeof(string)]);
            result.Value.Arguments.ShouldBeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Nested_Generics_Without_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.Collections.Generic.IEnumerable<System.String>>()");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: System.Collections.Generic.IEnumerable<System.String>");
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Generics_And_Arguments()
        {
            // Arrange
            var context = CreateContext("MyFunction<System.String>(argument1, argument2, argument3)");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

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
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

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
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Function name may not contain whitespace");
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Spaces_Tabs_And_NewLines()
        {
            // Arrange
            var context = CreateContext("       MyFunction(\r\n\r\n\r\n   \t\t)\t\t\t");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_With_Quotes()
        {
            // Arrange
            var context = CreateContext("MyFunction(argument1, \"argument2, argument3\", argument4)");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Name.ShouldBe("MyFunction");
            result.Value.TypeArguments.ShouldBeEmpty();
            result.Value.Arguments.ShouldBe(["argument1", "\"argument2, argument3\"", "argument4"]);
        }

        [Fact]
        public void Returns_Invalid_When_Too_Many_Close_Brackets_Were_Found()
        {
            // Arrange
            var context = CreateContext("MyFunction())");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Input has additional characters after last close bracket");
        }

        [Fact]
        public void Returns_Invalid_When_Generic_Type_Is_Unknown()
        {
            // Arrange
            var context = CreateContext("MyFunction<unknowntype>()");
            var sut = CreateSut();

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: unknowntype");
        }
    }
}
