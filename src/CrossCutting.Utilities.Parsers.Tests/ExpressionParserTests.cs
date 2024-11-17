namespace CrossCutting.Utilities.Parsers.Tests;

public class ExpressionParserTests : IDisposable
{
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;
    private readonly IVariable _variable;
    private bool disposedValue;

    public ExpressionParserTests()
    {
        _variable = Substitute.For<IVariable>();
        _provider = new ServiceCollection()
            .AddParsers()
            .AddSingleton(_variable)
            .BuildServiceProvider(true);
        _scope = _provider.CreateScope();
    }

    public class Parse : ExpressionParserTests
    {
        [Fact]
        public void Parses_true_Correctly()
        {
            // Arrange
            var input = "true";

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(true);
        }

        [Fact]
        public void Parses_false_Correctly()
        {
            // Arrange
            var input = "false";

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(false);
        }

        [Fact]
        public void Parses_null_Correctly()
        {
            // Arrange
            var input = "null";

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeNull();
        }

        [Fact]
        public void Parses_context_Correctly()
        {
            // Arrange
            var input = "context";

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture, "context value");

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo("context value");
        }

        [Fact]
        public void Parses_decimal_Correctly()
        {
            // Arrange
            var input = "1.5";

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(1.5M);
        }

        [Fact]
        public void Parses_forced_decimal_Correctly()
        {
            // Arrange
            var input = "1M";

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(1M);
        }

        [Fact]
        public void Parses_int_Correctly()
        {
            // Arrange
            var input = "2";

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(2);
        }

        [Fact]
        public void Parses_long_Correctly()
        {
            // Arrange
            var input = "3147483647";

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(3147483647L);
        }

        [Fact]
        public void Parses_forced_long_Correctly()
        {
            // Arrange
            var input = "13L";

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(13L);
        }

        [Fact]
        public void Parses_string_Correctly()
        {
            // Arrange
            var input = "\"Hello world!\"";

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo("Hello world!");
        }

        [Fact]
        public void Parses_DateTime_Correctly()
        {
            // Arrange
            var input = "01/02/2019";

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo(new DateTime(2019, 1, 2, 0, 0, 0, DateTimeKind.Unspecified));
        }

        [Fact]
        public void Parses_Variable_Correctly_Using_Context()
        {
            // Arrange
            var input = "$classname";
            var context = new MyContext("HelloWorldClass");
            _variable.Process(Arg.Any<string>(), Arg.Any<object?>()).Returns(x => x.ArgAt<string>(0) == "classname"
                ? Result.Success<object?>((x.ArgAt<object?>(1) as MyContext)?.ClassName)
                : Result.Continue<object?>());

            // Act
            var result = CreateSut().Parse(input, CultureInfo.InvariantCulture, context);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().BeEquivalentTo("HelloWorldClass");
        }

        private sealed class MyContext(string className)
        {
            public string ClassName { get; } = className;
        }
    }

    private IExpressionParser CreateSut() => _scope.ServiceProvider.GetRequiredService<IExpressionParser>();

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
}
