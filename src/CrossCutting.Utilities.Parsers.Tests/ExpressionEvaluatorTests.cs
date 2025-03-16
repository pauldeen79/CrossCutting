namespace CrossCutting.Utilities.Parsers.Tests;

public class ExpressionEvaluatorTests : IDisposable
{
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;
    private readonly IVariable _variable;
    private bool disposedValue;

    public ExpressionEvaluatorTests()
    {
        _variable = Substitute.For<IVariable>();
        _provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton(_variable)
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
    }

    public class Evaluate : ExpressionEvaluatorTests
    {
        [Fact]
        public void Parses_true_Correctly()
        {
            // Arrange
            var input = "true";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void Parses_false_Correctly()
        {
            // Arrange
            var input = "false";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(false);
        }

        [Fact]
        public void Parses_null_Correctly()
        {
            // Arrange
            var input = "null";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeNull();
        }

        [Fact]
        public void Parses_context_Correctly()
        {
            // Arrange
            var input = "context";

            // Act
            var result = CreateSut().Evaluate(input, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(CultureInfo.InvariantCulture), "context value");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo("context value");
        }

        [Fact]
        public void Parses_decimal_Correctly()
        {
            // Arrange
            var input = "1.5";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(1.5M);
        }

        [Fact]
        public void Parses_forced_decimal_Correctly()
        {
            // Arrange
            var input = "1M";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(1M);
        }

        [Fact]
        public void Parses_int_Correctly()
        {
            // Arrange
            var input = "2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(2);
        }

        [Fact]
        public void Parses_long_Correctly()
        {
            // Arrange
            var input = "3147483647";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(3147483647L);
        }

        [Fact]
        public void Parses_forced_long_Correctly()
        {
            // Arrange
            var input = "13L";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(13L);
        }

        [Fact]
        public void Parses_string_Correctly()
        {
            // Arrange
            var input = "\"Hello world!\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo("Hello world!");
        }

        [Fact]
        public void Parses_DateTime_Correctly()
        {
            // Arrange
            var input = "01/02/2019";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(new DateTime(2019, 1, 2, 0, 0, 0, DateTimeKind.Unspecified));
        }

        [Fact]
        public void Parses_Variable_Correctly()
        {
            // Arrange
            var input = "$classname";
            _variable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Success<object?>("HelloWorld"));

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo("HelloWorld");
        }

        [Fact]
        public void Parses_Variable_Correctly_Using_Context()
        {
            // Arrange
            var input = "$classname";
            var context = new MyContext("HelloWorldClass");
            _variable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(x => x.ArgAt<string>(0) == "classname"
                ? Result.Success<object?>((x.ArgAt<object?>(1) as MyContext)?.ClassName)
                : Result.Continue<object?>());

            // Act
            var result = CreateSut().Evaluate(input, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(CultureInfo.InvariantCulture), context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo("HelloWorldClass");
        }

        [Fact]
        public void Parses_Equals_Operator_Correctly()
        {
            // Arrange
            var input = "1 == 1";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }

        [Fact]
        public void Returns_NotSupported_On_Empty_String()
        {
            // Arrange
            var input = "";
            _variable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Success<object?>("HelloWorld"));

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Value is required");
        }

        [Fact]
        public void Returns_NotSupported_On_DollarSign()
        {
            // Arrange
            var input = "$";
            _variable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Success<object?>("HelloWorld"));

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: $");
        }

        [Fact]
        public void Returns_Invalid_On_Unknown_Variable()
        {
            // Arrange
            var input = "$unknownvariable";
            _variable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Continue<object?>());

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown variable found: unknownvariable");
        }

        [Fact]
        public void Returns_Success_On_TypeOf_Expression_When_Type_Is_Known()
        {
            // Arrange
            var input = "typeof(System.String)";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(string));
        }

        [Fact]
        public void Returns_Invalid_On_TypeOf_Expression_When_Type_Is_Unknown()
        {
            // Arrange
            var input = "typeof(unknowntype)";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: unknowntype");
        }
    }

    public class Validate : ExpressionEvaluatorTests
    {
        [Fact]
        public void Validates_true_Correctly()
        {
            // Arrange
            var input = "true";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Validates_false_Correctly()
        {
            // Arrange
            var input = "false";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Validates_null_Correctly()
        {
            // Arrange
            var input = "null";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
        }

        [Fact]
        public void Validates_context_Correctly()
        {
            // Arrange
            var input = "context";

            // Act
            var result = CreateSut().Validate(input, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(CultureInfo.InvariantCulture), "context value");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Validates_decimal_Correctly()
        {
            // Arrange
            var input = "1.5";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Validates_forced_decimal_Correctly()
        {
            // Arrange
            var input = "1M";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Parses_int_Correctly()
        {
            // Arrange
            var input = "2";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo(2);
        }

        [Fact]
        public void Validates_long_Correctly()
        {
            // Arrange
            var input = "3147483647";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Validates_forced_long_Correctly()
        {
            // Arrange
            var input = "13L";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Parses_string_Correctly()
        {
            // Arrange
            var input = "\"Hello world!\"";

            // Act
            var result = CreateSut().Evaluate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEquivalentTo("Hello world!");
        }

        [Fact]
        public void Validates_DateTime_Correctly()
        {
            // Arrange
            var input = "01/02/2019";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Validates_Variable_Correctly()
        {
            // Arrange
            var input = "$classname";
            _variable.Validate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.NoContent<Type>());

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
        }

        [Fact]
        public void Validates_Variable_Correctly_Using_Context()
        {
            // Arrange
            var input = "$classname";
            var context = new MyContext("HelloWorldClass");
            _variable.Validate(Arg.Any<string>(), Arg.Any<object?>()).Returns(x => x.ArgAt<string>(0) == "classname"
                ? Result.NoContent<Type>()
                : Result.Continue<Type>());

            // Act
            var result = CreateSut().Validate(input, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(CultureInfo.InvariantCulture), context);

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
        }

        [Fact]
        public void Returns_Invalid_On_Empty_String()
        {
            // Arrange
            var input = "";
            _variable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Success<object?>("HelloWorld"));

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Value is required");
        }

        [Fact]
        public void Returns_Invalid_On_DollarSign()
        {
            // Arrange
            var input = "$";
            _variable.Evaluate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Success<object?>("HelloWorld"));

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: $");
        }

        [Fact]
        public void Returns_Invalid_On_Unknown_Variable()
        {
            // Arrange
            var input = "$unknownvariable";
            _variable.Validate(Arg.Any<string>(), Arg.Any<object?>()).Returns(Result.Continue<Type>());

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown variable found: unknownvariable");
        }

        [Fact]
        public void Returns_Success_On_TypeOf_Expression_When_Type_Is_Known()
        {
            // Arrange
            var input = "typeof(System.String)";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(Type));
        }

        [Fact]
        public void Returns_Invalid_On_TypeOf_Expression_When_Type_Is_Unknown()
        {
            // Arrange
            var input = "typeof(unknowntype)";

            // Act
            var result = CreateSut().Validate(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: unknowntype");
        }
    }

    private IExpressionEvaluator CreateSut() => _scope.ServiceProvider.GetRequiredService<IExpressionEvaluator>();

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _scope.Dispose();
                _provider.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private sealed class MyContext(string className)
    {
        public string ClassName { get; } = className;
    }
}
