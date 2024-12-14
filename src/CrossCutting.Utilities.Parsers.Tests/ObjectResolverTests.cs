namespace CrossCutting.Utilities.Parsers.Tests;

public class ObjectResolverTests
{
    public class Resolve : ObjectResolverTests
    {
        [Fact]
        public void Returns_Result_When_Type_Is_Not_Unknown()
        {
            // Arrange
            var myVariable = Substitute.For<IObjectResolverProcessor>();
            myVariable.Resolve<string>(Arg.Any<object?>()).Returns(Result.Success("result value"));
            var sut = new ObjectResolver([myVariable]);

            // Act
            var result = sut.Resolve<string>("some context");

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().Be("result value");
        }

        [Fact]
        public void Returns_NotFound_When_Type_Is_Unknown()
        {
            // Arrange
            var sut = new ObjectResolver([]);

            // Act
            var result = sut.Resolve<string>("some context");

            // Assert
            result.Status.Should().Be(ResultStatus.NotFound);
            result.ErrorMessage.Should().Be("Could not resolve type System.String from System.String");
        }

        [Fact]
        public void Returns_NotFound_When_Context_Is_Null()
        {
            // Arrange
            var sut = new ObjectResolver([]);

            // Act
            var result = sut.Resolve<string>(null);

            // Assert
            result.Status.Should().Be(ResultStatus.NotFound);
        }
    }
}
