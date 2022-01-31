namespace CrossCutting.Data.Abstractions.Tests.Extensions;

public class DatabaseCommandTypeExtensionsTests
{
    [Theory]
    [InlineData(DatabaseCommandType.Text, CommandType.Text)]
    [InlineData(DatabaseCommandType.StoredProcedure, CommandType.StoredProcedure)]
    public void ToCommandType_Returns_Correct_Value(DatabaseCommandType input, CommandType expectedResult)
    {
        // Act
        var actual = input.ToCommandType();

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Fact]
    public void ToCommandType_Throws_On_Unknown_DbCommandType()
    {
        // Arrange
        var unknownType = (DatabaseCommandType)123456;

        // Act
        unknownType.Invoking(x => x.ToCommandType())
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.ParamName.Should().Be("instance");
    }
}
